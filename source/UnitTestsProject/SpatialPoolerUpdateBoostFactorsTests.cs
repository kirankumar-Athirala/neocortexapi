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
         * Testing Boost Factors are updated as per the mathematical formula defined in UpdateBoostFactors method maxboost 10
         */
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateBoostFactorsB10()
        {
            int inpBits = 10;
            int numCols = 10;
            InitTestSPInstance(inpBits, numCols);

            mem.HtmConfig.MaxBoost = 10;
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
            int inpBits = 10;
            int numCols = 10;
            InitTestSPInstance(inpBits, numCols);

            mem.HtmConfig.MaxBoost = 10;

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

            int inpBits = 10;
            int numCols = 10;
            InitTestSPInstance(inpBits, numCols);

            mem.HtmConfig.MaxBoost = 1.0;
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
