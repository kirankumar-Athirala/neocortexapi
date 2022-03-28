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
    public class SpatialPoolerUpdateMinDutyCyclesTest
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

        public void setupDefaultParameters()
        {
            parameters = Parameters.getAllDefaultParameters();
            parameters.Set(KEY.INPUT_DIMENSIONS, new int[] { 32, 32 });
            parameters.Set(KEY.COLUMN_DIMENSIONS, new int[] { 64, 64 });
            parameters.Set(KEY.POTENTIAL_RADIUS, 16);
            parameters.Set(KEY.POTENTIAL_PCT, 0.5);
            parameters.Set(KEY.GLOBAL_INHIBITION, false);
            parameters.Set(KEY.LOCAL_AREA_DENSITY, -1.0);
            parameters.Set(KEY.NUM_ACTIVE_COLUMNS_PER_INH_AREA, 10.0);
            parameters.Set(KEY.STIMULUS_THRESHOLD, 0.0);
            parameters.Set(KEY.SYN_PERM_INACTIVE_DEC, 0.008);
            parameters.Set(KEY.SYN_PERM_ACTIVE_INC, 0.05);
            parameters.Set(KEY.SYN_PERM_CONNECTED, 0.10);
            parameters.Set(KEY.MIN_PCT_OVERLAP_DUTY_CYCLES, 0.001);
            parameters.Set(KEY.MIN_PCT_ACTIVE_DUTY_CYCLES, 0.001);
            parameters.Set(KEY.DUTY_CYCLE_PERIOD, 1000);
            parameters.Set(KEY.MAX_BOOST, 10.0);
            parameters.Set(KEY.SEED, 42);
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
         * Testing Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCycleLocal without wrap around
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateMinDutyCycleLocalwithoutWrapAround()
        {
            setupDefaultParameters();
            parameters.setInputDimensions(new int[] { 5 });
            parameters.setColumnDimensions(new int[] { 8 });
            parameters.Set(KEY.WRAP_AROUND, false);
            InitTestSPInstance();

            sp.InhibitionRadius = 2;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0.7, 0.1, 0.5, 0.01, 0.78, 0.55, 0.1, 0.001 };
            mem.HtmConfig.ActiveDutyCycles = new double[] { 0.9, 0.3, 0.5, 0.7, 0.1, 0.01, 0.08, 0.12 };
            mem.HtmConfig.MinPctActiveDutyCycles = 0.1;
            mem.HtmConfig.MinPctOverlapDutyCycles = 0.2;
            sp.UpdateMinDutyCyclesLocal(mem);

            double[] resultantMinActiveDutyCycles = mem.HtmConfig.MinActiveDutyCycles;
            double[] expectedactivedutycycles = { 0.09, 0.09, 0.09, 0.07, 0.07, 0.07, 0.012, 0.012 };

            for (var i = 0; i < expectedactivedutycycles.Length; i++)
            {
                Assert.IsTrue(Math.Abs(expectedactivedutycycles[i] - resultantMinActiveDutyCycles[i]) <= 0.01);
            }

            double[] resultMinOverlapDutyCycles = mem.HtmConfig.MinOverlapDutyCycles;
            double[] expectedoverlapdutycycles = new double[] { 0.14, 0.14, 0.156, 0.156, 0.156, 0.156, 0.156, 0.11 };

            for (var i = 0; i < expectedoverlapdutycycles.Length; i++)
            {
                Assert.IsTrue(Math.Abs(expectedoverlapdutycycles[i] - resultMinOverlapDutyCycles[i]) <= 0.01);
            }
        }
        /**
         * Testing Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCycleLocal with wraparound
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateMinDutyCycleLocalwithWrapAround()
        {
            setupDefaultParameters();
            parameters.setInputDimensions(new int[] { 5 });
            parameters.setColumnDimensions(new int[] { 8 });
            parameters.Set(KEY.WRAP_AROUND, true);
            InitTestSPInstance();

            sp.InhibitionRadius = 2;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0.7, 0.1, 0.5, 0.01, 0.78, 0.55, 0.1, 0.001 };
            mem.HtmConfig.ActiveDutyCycles = new double[] { 0.9, 0.3, 0.5, 0.7, 0.1, 0.01, 0.08, 0.12 };
            mem.HtmConfig.MinPctActiveDutyCycles = 0.1;
            mem.HtmConfig.MinPctOverlapDutyCycles = 0.2;
            sp.UpdateMinDutyCyclesLocal(mem);

            double[] resultantMinActiveDutyCycles = mem.HtmConfig.MinActiveDutyCycles;
            double[] expectedactivedutycycles = { 0.09, 0.09, 0.09, 0.07, 0.07, 0.07, 0.09, 0.09 };

            for (var i = 0; i < expectedactivedutycycles.Length; i++)
            {
                Assert.IsTrue(Math.Abs(expectedactivedutycycles[i] - resultantMinActiveDutyCycles[i]) <= 0.01);
            }

            double[] resultMinOverlapDutyCycles = mem.HtmConfig.MinOverlapDutyCycles;
            double[] expectedoverlapdutycycles = new double[] { 0.14, 0.14, 0.156, 0.156, 0.156, 0.156, 0.156, 0.14 };

            for (var i = 0; i < expectedoverlapdutycycles.Length; i++)
            {
                Assert.IsTrue(Math.Abs(expectedoverlapdutycycles[i] - resultMinOverlapDutyCycles[i]) <= 0.01);
            }
        }
        /**
         * Testing duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 1
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateMinDutyCycleGlobal()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 5 });

            parameters.setColumnDimensions(new int[] { 5 });
            InitTestSPInstance();

            mem.HtmConfig.MinPctOverlapDutyCycles = 0.06;
            mem.HtmConfig.MinPctActiveDutyCycles = 0.08;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 1, 2, 3, 4, 5 };
            mem.HtmConfig.ActiveDutyCycles = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 };

            sp.UpdateMinDutyCyclesGlobal(mem);
            double[] expectedMinActiveDutyCycles = new double[mem.HtmConfig.NumColumns];
            ArrayUtils.InitArray(expectedMinActiveDutyCycles, 0.08 * 0.5);
            double[] expectedMinOverlapDutyCycles = new double[mem.HtmConfig.NumColumns];
            ArrayUtils.InitArray(expectedMinOverlapDutyCycles, 0.06 * 5);
            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                Assert.IsTrue(Math.Abs(expectedMinOverlapDutyCycles[i] - mem.HtmConfig.MinOverlapDutyCycles[i]) <= 0.01);
                Assert.IsTrue(Math.Abs(expectedMinActiveDutyCycles[i] - mem.HtmConfig.MinActiveDutyCycles[i]) <= 0.01);
            }
        }

    }
}
