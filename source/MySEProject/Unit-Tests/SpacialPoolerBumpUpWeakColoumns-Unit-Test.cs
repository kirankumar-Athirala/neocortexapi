// Copyright (c) Damir Dobric. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortex;
using NeoCortexApi;
using NeoCortexApi.Entities;
using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestsProject
{
    [TestClass]
    public class SpatialPoolerBumpUpWeakColumnsTest
    {

        private Parameters parameters;
        private SpatialPooler sp;
        private Connections mem;

        public void setupParameters()
        {
            parameters = Parameters.getAllDefaultParameters();
            parameters.Set(KEY.INPUT_DIMENSIONS, new int[] { 5 });
            parameters.Set(KEY.COLUMN_DIMENSIONS, new int[] { 5 });
            parameters.Set(KEY.POTENTIAL_RADIUS, 5);
            parameters.Set(KEY.POTENTIAL_PCT, 0.5);
            parameters.Set(KEY.GLOBAL_INHIBITION, false);
            parameters.Set(KEY.LOCAL_AREA_DENSITY, -1.0);
            parameters.Set(KEY.NUM_ACTIVE_COLUMNS_PER_INH_AREA, 3.0);
            parameters.Set(KEY.STIMULUS_THRESHOLD, 0.0);
            parameters.Set(KEY.SYN_PERM_INACTIVE_DEC, 0.01);
            parameters.Set(KEY.SYN_PERM_ACTIVE_INC, 0.1);
            parameters.Set(KEY.SYN_PERM_CONNECTED, 0.1);
            parameters.Set(KEY.MIN_PCT_OVERLAP_DUTY_CYCLES, 0.1);
            parameters.Set(KEY.MIN_PCT_ACTIVE_DUTY_CYCLES, 0.1);
            parameters.Set(KEY.DUTY_CYCLE_PERIOD, 10);
            parameters.Set(KEY.MAX_BOOST, 10.0);
            parameters.Set(KEY.RANDOM, new ThreadSafeRandom(42));
        }

        private void InitTestSPInstance()
        {
            sp = new SpatialPoolerMT();
            mem = new Connections();
            parameters.apply(mem);
            sp.Init(mem);
        }

        /**
         * Testing permanence values are updated correctly in BumpUpWeakColumns method with SynPermBelowStimulusInc as 0.01
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testBumpUpWeakColumns_1()
        {

            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 5 });
            InitTestSPInstance();

            mem.HtmConfig.SynPermBelowStimulusInc = 0.01;
            mem.HtmConfig.SynPermTrimThreshold = 0.05;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0, 0.009, 0.1, 0.001, 0.002 };
            mem.HtmConfig.MinOverlapDutyCycles = new double[] { .01, .01, .01, .01, .01 };



            int[][] testingPools = new int[][] {
                new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1 },
                new int[] { 1, 0, 0, 0, 1, 1, 0, 1, 1, 1 },
                new int[] { 0, 0, 1, 0, 1, 1, 1, 0, 0, 0 },
                new int[] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            double[][] testingpermanences = new double[][] {
                new double[] { 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000,0.300,0.400 },
                new double[] { 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450,0.160,0.190 },
                new double[] { 0.000, 0.000, 0.014, 0.000, 0.032, 0.044, 0.110, 0.000,0.000,0.000 },
                new double[] { 0.041, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000,0.000,0.000 },
                new double[] { 0.100, 0.738, 0.045, 0.002, 0.050, 0.008, 0.208, 0.034,0.200,0.300 }
            };

            double[][] ExpectedPermanences = new double[][] {
            new double[] { 0.210, 0.130, 0.100, 0.000, 0.000, 0.000, 0.000, 0.000, 0.310, 0.410 },
            new double[] { 0.160, 0.000, 0.000, 0.000, 0.190, 0.130, 0.000, 0.460, 0.170, 0.200 },
            new double[] { 0.000, 0.000, 0.014, 0.000, 0.032, 0.044, 0.110, 0.000, 0.000, 0.000 },
            new double[]  { 0.051, 0.000, 0.000, 0.000, 0.000, 0.000, 0.188,0.000, 0.000, 0.000 },
            new double[] { 0.110, 0.748, 0.055, 0.000, 0.060, 0.000, 0.218, 0.000, 0.210, 0.310 }
        };


            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(testingPools[i], n => n == 1);

                // int[] indexes = ArrayUtils.where(potentialPools[i], cond);
                mem.GetColumn(i).SetProximalConnectedSynapsesForTest(mem, indexes);
                mem.GetColumn(i).SetPermanences(mem.HtmConfig, testingpermanences[i]);
            }

            //Execute method being tested
            sp.BumpUpWeakColumns(mem);

            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                double[] calculatedperms = mem.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(mem.HtmConfig.NumInputs);
                for (int j = 0; j < ExpectedPermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(ExpectedPermanences[i][j] - calculatedperms[j]) <= 0.01);
                }
            }
        }
        /**
         * Testing permanence values are updated correctly in BumpUpWeakColumns method with SynPermBelowStimulusInc as 0.02
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testBumpUpWeakColumns_2()
        {

            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 5 });
            InitTestSPInstance();

            mem.HtmConfig.SynPermBelowStimulusInc = 0.02;
            mem.HtmConfig.SynPermTrimThreshold = 0.01;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0, 0.009, 0.1, 0.001, 0.002 };
            mem.HtmConfig.MinOverlapDutyCycles = new double[] { .01, .01, .01, .01, .01 };



            int[][] testingPools = new int[][] {
                new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1 },
                new int[] { 1, 0, 0, 0, 1, 1, 0, 1, 1, 1 },
                new int[] { 0, 0, 1, 0, 1, 1, 1, 0, 0, 0 },
                new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            double[][] testingpermanences = new double[][] {
                new double[] { 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000,0.300,0.400 },
                new double[] { 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450,0.160,0.190 },
                new double[] { 0.000, 0.000, 0.014, 0.000, 0.032, 0.044, 0.110, 0.000,0.000,0.000 },
                new double[] { 0.041, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000,0.000,0.000 },
                new double[] { 0.100, 0.738, 0.045, 0.002, 0.050, 0.008, 0.208, 0.034,0.200,0.300 }
            };

            double[][] ExpectedPermanences = new double[][] {
            new double[] { 0.220, 0.140, 0.110, 0.060, 0.000, 0.000, 0.000, 0.000, 0.320, 0.420 },
            new double[] { 0.170, 0.000, 0.000, 0.000, 0.200, 0.140, 0.000, 0.470, 0.180, 0.210 },
            new double[] { 0.000, 0.000, 0.014, 0.000, 0.032, 0.044, 0.110, 0.000, 0.000, 0.000 },
            new double[]  { 0.061, 0.000, 0.000, 0.000, 0.000, 0.000, 0.198,0.000, 0.000, 0.000 },
            new double[] { 0.120, 0.758, 0.065, 0.022, 0.070, 0.028, 0.228, 0.054, 0.220, 0.320 }
        };


            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(testingPools[i], n => n == 1);

                // int[] indexes = ArrayUtils.where(potentialPools[i], cond);
                mem.GetColumn(i).SetProximalConnectedSynapsesForTest(mem, indexes);
                mem.GetColumn(i).SetPermanences(mem.HtmConfig, testingpermanences[i]);
            }

            //Execute method being tested
            sp.BumpUpWeakColumns(mem);

            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                double[] calculatedperms = mem.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(mem.HtmConfig.NumInputs);
                for (int j = 0; j < ExpectedPermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(ExpectedPermanences[i][j] - calculatedperms[j]) <= 0.01);
                }
            }
        }
        /**
         * Testing permanence values are not updated  when All OverlapDutyCycles are greater than MinOverlapDutyCycles
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testBumpUpWeakColumns_3()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 5 });
            InitTestSPInstance();

            mem.HtmConfig.SynPermBelowStimulusInc = 0.02;
            mem.HtmConfig.SynPermTrimThreshold = 0.01;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0.1, 0.1, 0.1, 0.1, 0.1 };
            mem.HtmConfig.MinOverlapDutyCycles = new double[] { .01, .01, .01, .01, .01 };



            int[][] testingPools = new int[][] {
                new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1 },
                new int[] { 1, 0, 0, 0, 1, 1, 0, 1, 1, 1 },
                new int[] { 0, 0, 1, 0, 1, 1, 1, 0, 0, 0 },
                new int[] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            double[][] testingpermanences = new double[][] {
                new double[] { 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000,0.300,0.400 },
                new double[] { 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450,0.160,0.190 },
                new double[] { 0.000, 0.000, 0.014, 0.000, 0.032, 0.044, 0.110, 0.000,0.000,0.000 },
                new double[] { 0.041, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000,0.000,0.000 },
                new double[] { 0.100, 0.738, 0.045, 0.002, 0.050, 0.008, 0.208, 0.034,0.200,0.300 }
            };

            double[][] ExpectedPermanences = new double[][] {
                new double[] { 0.200, 0.120, 0.090, 0.040, 0.000, 0.000, 0.000, 0.000,0.300,0.400 },
                new double[] { 0.150, 0.000, 0.000, 0.000, 0.180, 0.120, 0.000, 0.450,0.160,0.190 },
                new double[] { 0.000, 0.000, 0.014, 0.000, 0.032, 0.044, 0.110, 0.000,0.000,0.000 },
                new double[] { 0.041, 0.000, 0.000, 0.000, 0.000, 0.000, 0.178, 0.000,0.000,0.000 },
                new double[] { 0.100, 0.738, 0.045, 0.002, 0.050, 0.008, 0.208, 0.034,0.200,0.300 }
        };


            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                int[] indexes = ArrayUtils.IndexWhere(testingPools[i], n => n == 1);

                // int[] indexes = ArrayUtils.where(potentialPools[i], cond);
                mem.GetColumn(i).SetProximalConnectedSynapsesForTest(mem, indexes);
                mem.GetColumn(i).SetPermanences(mem.HtmConfig, testingpermanences[i]);
            }

            //Execute method being tested
            sp.BumpUpWeakColumns(mem);

            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                double[] calculatedperms = mem.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(mem.HtmConfig.NumInputs);
                for (int j = 0; j < ExpectedPermanences[i].Length; j++)
                {
                    Assert.IsTrue(Math.Abs(ExpectedPermanences[i][j] - calculatedperms[j]) <= 0.01);
                }
            }
        }

    }
}
