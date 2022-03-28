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
    public class SpatialPoolerUpdateBoostFactorsTest
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
         * Testing Boost Factors are updated as per the mathematical formula defined in UpdateBoostFactors method maxboost 10
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateBoostFactorsB10()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 10 });
            parameters.setMaxBoost(10.0);
            parameters.setRandom(new ThreadSafeRandom(42));
            InitTestSPInstance();

            mem.HtmConfig.NumColumns = 10;

            double[] minActiveDutyCycles = new double[10];
            ArrayUtils.InitArray(minActiveDutyCycles, 0.1);
            mem.HtmConfig.MinActiveDutyCycles = minActiveDutyCycles;

            double[] activeDutyCycles = new double[10];
            ArrayUtils.InitArray(activeDutyCycles, 0.01);
            mem.HtmConfig.ActiveDutyCycles = activeDutyCycles;

            double[] ExpectedBoostFactors = new double[] { 9.1, 9.1, 9.1, 9.1, 9.1, 9.1, 9.1, 9.1, 9.1, 9.1 };
            sp.UpdateBoostFactors(mem);
            double[] boostFactors = mem.BoostFactors;
            for (int i = 0; i < boostFactors.Length; i++)
            {
                Assert.IsTrue(Math.Abs(ExpectedBoostFactors[i] - boostFactors[i]) <= 0.1D);
            }
        }
        /**
         * Testing Boost Factors are not updated when all minActiveDutyCycles are 0
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateBoostFactorsMDC0()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 10 });
            parameters.setMaxBoost(10.0);
            parameters.setRandom(new ThreadSafeRandom(42));
            InitTestSPInstance();

            mem.HtmConfig.NumColumns = 10;

            double[] minActiveDutyCycles = new double[10];
            ArrayUtils.InitArray(minActiveDutyCycles, 0);
            mem.HtmConfig.MinActiveDutyCycles = minActiveDutyCycles;

            double[] activeDutyCycles = new double[10];
            ArrayUtils.InitArray(activeDutyCycles, 0.1);
            mem.HtmConfig.ActiveDutyCycles = activeDutyCycles;

            double[] BoostFactors = new double[10];
            ArrayUtils.InitArray(BoostFactors, 1);
            mem.BoostFactors = BoostFactors;

            double[] ExpectedBoostFactors = new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            sp.UpdateBoostFactors(mem);
            Assert.IsTrue(mem.BoostFactors.SequenceEqual(ExpectedBoostFactors));
        }
        /**
         *  Testing Boost Factors are updated as per the mathematical formula defined in UpdateBoostFactors method maxboost 1
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateBoostFactorsB1()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 10 });
            parameters.setMaxBoost(1.0);
            parameters.setRandom(new ThreadSafeRandom(42));
            InitTestSPInstance();

            mem.HtmConfig.NumColumns = 10;

            double[] minActiveDutyCycles = new double[10];
            ArrayUtils.InitArray(minActiveDutyCycles, 1);
            mem.HtmConfig.MinActiveDutyCycles = minActiveDutyCycles;

            double[] activeDutyCycles = new double[10];
            ArrayUtils.InitArray(activeDutyCycles, 1);
            mem.HtmConfig.ActiveDutyCycles = activeDutyCycles;

            double[] ExpectedBoostFactors = new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            sp.UpdateBoostFactors(mem);
            double[] boostFactors = mem.BoostFactors;
            for (int i = 0; i < boostFactors.Length; i++)
            {
                Assert.IsTrue(Math.Abs(ExpectedBoostFactors[i] - boostFactors[i]) <= 0.1D);
            }
        }

    }
}