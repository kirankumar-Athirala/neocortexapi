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

        private SpatialPooler sp;
        private Connections mem;
        private bool DefaultFlag = false;
        private HtmConfig htmConfig;

        /// <summary>
        /// create htmconfig with default parameters required for the unit tests 
        /// and also create connection instance for spatial pooler intialization
        /// </summary>
        private void InitTestSPInstance()
        {
            if (DefaultFlag == false)
            {
                htmConfig = new HtmConfig(new int[] { 5 }, new int[] { 8 })
                {
                    PotentialRadius = 5,
                    PotentialPct = 0.5,
                    GlobalInhibition = false,
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
            }
            else
            {
                htmConfig = new HtmConfig(new int[] { 5 }, new int[] { 8 })
                {
                    PotentialRadius = 16,
                    PotentialPct = 0.5,
                    GlobalInhibition = false,
                    LocalAreaDensity = -1,
                    NumActiveColumnsPerInhArea = 10.0,
                    StimulusThreshold = 0.0,
                    SynPermActiveInc = 0.05,
                    SynPermInactiveDec = 0.008,
                    SynPermConnected = 0.10,
                    MinPctActiveDutyCycles = 0.001,
                    MinPctOverlapDutyCycles = 0.001,
                    DutyCyclePeriod = 1000,
                    MaxBoost = 10,
                    RandomGenSeed = 42,
                    Random = new ThreadSafeRandom(42),
                };
                DefaultFlag = false;
            }
            mem = new Connections(htmConfig);
            sp = new SpatialPoolerMT();
            sp.Init(mem);
        }

        /// <summary>
        /// It makes sure that Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCyclesLocal method when wrap around is false
        /// This test ensures that Min Duty cycles values are calculated as per the formula and updated accordingly.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateMinDutyCycleLocalwithoutWrapAround()
        {
            DefaultFlag = true;
            InitTestSPInstance();

            mem.HtmConfig.WrapAround = false;

            sp.InhibitionRadius = 2;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0.7, 0.1, 0.5, 0.01, 0.78, 0.55, 0.1, 0.001 };
            mem.HtmConfig.ActiveDutyCycles = new double[] { 0.9, 0.3, 0.5, 0.7, 0.1, 0.01, 0.08, 0.12 };
            mem.HtmConfig.MinPctActiveDutyCycles = 0.1;
            mem.HtmConfig.MinPctOverlapDutyCycles = 0.2;
            // executing UpdateMinDutyCyclesLocal method with mem connection
            sp.UpdateMinDutyCyclesLocal(mem);
            // calculated MinActiveDutyCycle values from the spatialpooler UpdateMinDutyCyclesLocal method
            double[] resultantMinActiveDutyCycles = mem.HtmConfig.MinActiveDutyCycles;
            // Expected minactivedutycycles values calculated manually as described MinPctActiveDutyCycles * Maximal Active Duty Cycles in the cortical column when wrap around is false.
            double[] expectedactivedutycycles = { 0.09, 0.09, 0.09, 0.07, 0.07, 0.07, 0.012, 0.012 };

            for (var i = 0; i < expectedactivedutycycles.Length; i++)
            {
                // Veriying absolute values of manually calculated minactivedutycycles vales and activedutycycles values from UpdateMinDutyCyclesLocal method
                Assert.IsTrue(Math.Abs(expectedactivedutycycles[i] - resultantMinActiveDutyCycles[i]) <= 0.01);
            }
            // calculated minOverlapDutyCycles values from the spatialpooler UpdateMinDutyCyclesLocal method
            double[] resultMinOverlapDutyCycles = mem.HtmConfig.MinOverlapDutyCycles;
            // Expected minactivedutycycles values calculated manually as described MinPctOverlapDutyCycles * Maximal Overlap in the cortical column when wrap around is false.
            double[] expectedoverlapdutycycles = new double[] { 0.14, 0.14, 0.156, 0.156, 0.156, 0.156, 0.156, 0.11 };

            for (var i = 0; i < expectedoverlapdutycycles.Length; i++)
            {
                // Veriying absolute values of manually calculated overlapdutycycles vales and overlapdutycycles values from UpdateMinDutyCyclesLocal method
                Assert.IsTrue(Math.Abs(expectedoverlapdutycycles[i] - resultMinOverlapDutyCycles[i]) <= 0.01);
            }
        }

        /// <summary>
        /// It makes sure that Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCyclesLocal method when wrap around is TRUE
        /// This test ensures that Min Duty cycles values are calculated as per the formula and updated accordingly.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateMinDutyCycleLocalwithWrapAround()
        {
            DefaultFlag = true;
            InitTestSPInstance();

            mem.HtmConfig.WrapAround = true;

            sp.InhibitionRadius = 2;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 0.7, 0.1, 0.5, 0.01, 0.78, 0.55, 0.1, 0.001 };
            mem.HtmConfig.ActiveDutyCycles = new double[] { 0.9, 0.3, 0.5, 0.7, 0.1, 0.01, 0.08, 0.12 };
            mem.HtmConfig.MinPctActiveDutyCycles = 0.1;
            mem.HtmConfig.MinPctOverlapDutyCycles = 0.2;
            // executing UpdateMinDutyCyclesLocal method with mem connection
            sp.UpdateMinDutyCyclesLocal(mem);
            // calculated MinActiveDutyCycle values from the spatialpooler UpdateMinDutyCyclesLocal method
            double[] resultantMinActiveDutyCycles = mem.HtmConfig.MinActiveDutyCycles;
            // Expected minactivedutycycles values calculated manually as described MinPctActiveDutyCycles * Maximal Active Duty Cycles in the cortical column when wrap around is true.
            double[] expectedactivedutycycles = { 0.09, 0.09, 0.09, 0.07, 0.07, 0.07, 0.09, 0.09 };

            for (var i = 0; i < expectedactivedutycycles.Length; i++)
            {
                // Veriying absolute values of manually calculated minactivedutycycles vales and minactivedutycycles values from UpdateMinDutyCyclesLocal method
                Assert.IsTrue(Math.Abs(expectedactivedutycycles[i] - resultantMinActiveDutyCycles[i]) <= 0.01);
            }
            // calculated minOverlapDutyCycles values from the spatialpooler UpdateMinDutyCyclesLocal method
            double[] resultMinOverlapDutyCycles = mem.HtmConfig.MinOverlapDutyCycles;
            // Expected minactivedutycycles values calculated manually as described MinPctOverlapDutyCycles * Maximal Overlap in the cortical column when wrap around is true.
            double[] expectedoverlapdutycycles = new double[] { 0.14, 0.14, 0.156, 0.156, 0.156, 0.156, 0.156, 0.14 };

            for (var i = 0; i < expectedoverlapdutycycles.Length; i++)
            {
                // Veriying absolute values of manually calculated minoverlapdutycycles vales and minoverlapdutycycles values from UpdateMinDutyCyclesLocal method
                Assert.IsTrue(Math.Abs(expectedoverlapdutycycles[i] - resultMinOverlapDutyCycles[i]) <= 0.01);
            }
        }

        /// <summary>
        /// It makes sure that Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCyclesGlobal method
        /// This test ensures that Min Duty cycles values are calculated as per the formula and updated accordingly.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testUpdateMinDutyCycleGlobal()
        {
            InitTestSPInstance();

            mem.HtmConfig.MinPctOverlapDutyCycles = 0.06;
            mem.HtmConfig.MinPctActiveDutyCycles = 0.08;
            mem.HtmConfig.OverlapDutyCycles = new double[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            mem.HtmConfig.ActiveDutyCycles = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8 };
            // executing UpdateMinDutyCyclesGlobal method with mem connection
            sp.UpdateMinDutyCyclesGlobal(mem);
            // Expected minactivedutycycles values calculated manually as described  MinPctActiveDutyCycles * Maximal Active Duty Cycles in the cortical column.
            double[] expectedMinActiveDutyCycles = new double[mem.HtmConfig.NumColumns];
            // Intializing MinactiveDutyCycles array of size 8 with value MinPctActiveDutyCycles * Maximal Active Duty Cycles
            ArrayUtils.InitArray(expectedMinActiveDutyCycles, 0.08 * 0.8);
            double[] expectedMinOverlapDutyCycles = new double[mem.HtmConfig.NumColumns];
            // Intializing MinOverlapDutyCycles array of size 8 with value MinPctMinOverlapDutyCycles * Maximal Active Duty Cycles
            ArrayUtils.InitArray(expectedMinOverlapDutyCycles, 0.06 * 8);
            for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
            {
                // Veriying absolute values of manually calculated minoverlapdutycycles vales and minoverlapdutycycles values from UpdateMinDutyCyclesGlobal method
                Assert.IsTrue(Math.Abs(expectedMinOverlapDutyCycles[i] - mem.HtmConfig.MinOverlapDutyCycles[i]) <= 0.01);
                // Veriying absolute values of manually calculated minactivedutycycles vales and minactivedutycycles values from UpdateMinDutyCyclesLocal method
                Assert.IsTrue(Math.Abs(expectedMinActiveDutyCycles[i] - mem.HtmConfig.MinActiveDutyCycles[i]) <= 0.01);
            }
        }
        
    }
}
