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

        private SpatialPooler sp;
        private Connections mem;


        private void InitTestSPInstance(int inputbits, int columns)
        {
            var htmConfig = new HtmConfig(new int[] { inputbits }, new int[] { columns })
            {
                PotentialRadius = 5,
                PotentialPct = 0.5,
                GlobalInhibition = true,
                LocalAreaDensity = -1,
                NumActiveColumnsPerInhArea = 3,
                StimulusThreshold = 0.0,
                SynPermActiveInc = 0.1,
                SynPermInactiveDec = 0.01,
                SynPermConnected = 0.1,
                MinPctActiveDutyCycles = 0.1,
                MinPctOverlapDutyCycles = 0.1,
                DutyCyclePeriod = 10,
                MaxBoost = 10,
                Random = new ThreadSafeRandom(42),
            };

            mem = new Connections(htmConfig);
            sp = new SpatialPoolerMT();
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
            int inpBits = 10;
            int numCols = 10;
            InitTestSPInstance(inpBits, numCols);

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
            int inpBits = 15;
            int numCols = 15;
            InitTestSPInstance(inpBits, numCols);

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
            int inpBits = 20;
            int numCols = 20;
            InitTestSPInstance(inpBits, numCols);

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
