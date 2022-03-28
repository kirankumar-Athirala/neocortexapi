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
    public class SpatialPoolerCalcEventFrequencyTest
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
         * Testing duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 500
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testCalcEventFrequencyP500()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 10 });
            parameters.setColumnDimensions(new int[] { 10 });
            InitTestSPInstance();

            double[] dutycycles = new double[10];
            ArrayUtils.InitArray(dutycycles, 1000.0);
            double[] newvalues = new double[10];
            int period = 500;
            double[] newDutyCycles = SpatialPooler.CalcEventFrequency(dutycycles, newvalues, period);
            double[] expectedDutyCycles = new double[] { 998, 998, 998, 998, 998, 998, 998, 998, 998, 998 };
            Assert.IsTrue(expectedDutyCycles.SequenceEqual(newDutyCycles));
        }
        /**
         * Testing duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 1000
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testCalcEventFrequencyP1000()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 15 });
            parameters.setColumnDimensions(new int[] { 15 });
            InitTestSPInstance();

            double[] dutycycles = new double[15];

            dutycycles = new double[15];
            ArrayUtils.InitArray(dutycycles, 10000.0);
            double[] newvalues = new double[15];
            ArrayUtils.InitArray(newvalues, 1.0);
            int period = 1000;
            double[] newDutyCycles = SpatialPooler.CalcEventFrequency(dutycycles, newvalues, period);

            double[] expectedDutyCycles = new double[] { 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001 };
            Assert.IsTrue(expectedDutyCycles.SequenceEqual(newDutyCycles));
        }
        /**
         * Testing duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 1
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testCalcEventFrequencyP1()
        {
            setupParameters();
            parameters.setInputDimensions(new int[] { 20 });
            parameters.setColumnDimensions(new int[] { 20 });
            InitTestSPInstance();

            double[] dutycycles = new double[20];

            dutycycles = new double[20];
            ArrayUtils.InitArray(dutycycles, 100.0);
            double[] newvalues = new double[20];
            ArrayUtils.InitArray(newvalues, 5000.0);
            int period = 1;
            double[] newDutyCycles = SpatialPooler.CalcEventFrequency(dutycycles, newvalues, period);

            double[] expectedDutyCycles = new double[] { 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000 };
            Assert.IsTrue(expectedDutyCycles.SequenceEqual(newDutyCycles));
        }

    }
}

