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

        /// <summary>
        /// create htmconfig with default parameters required for the unit tests 
        /// and also create connection instance for spatial pooler intialization
        /// </summary>
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

        /// <summary>
        /// It makes sure that duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method when period is 500
        /// This test ensures that duty cycles values are calculated as per the formula and updated accordingly.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testCalcEventFrequencyP500()
        {
            int inpBits = 10;
            int numCols = 10;
            InitTestSPInstance(inpBits, numCols);

            double[] dutycycles = new double[10];
            
            // Intializing dutycycles array of size 10 with value 1000
            ArrayUtils.InitArray(dutycycles, 1000.0);
            // Intializing new value array with default value 0
            double[] newvalues = new double[10];
            int period = 500;
            // executing CalcEventFrequency method with dutycycles, newvalues, period 
            double[] newDutyCycles = SpatialPooler.CalcEventFrequency(dutycycles, newvalues, period);
            // Expected duty cycle values are calculated manually using the formula ( (period - 1)*dutyCycle + newValue ) / period
            double[] expectedDutyCycles = new double[] { 998, 998, 998, 998, 998, 998, 998, 998, 998, 998 };
            // Veriying manually calculated duty cycle values and dutycycle values from CalcEventFrequency method are equall
            Assert.IsTrue(expectedDutyCycles.SequenceEqual(newDutyCycles));
        }

        /// <summary>
        /// It makes sure that duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method when period is 1000
        /// This test ensures that duty cycles values are calculated as per the formula and updated accordingly.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testCalcEventFrequencyP1000()
        {
            int inpBits = 15;
            int numCols = 15;
            InitTestSPInstance(inpBits, numCols);

            double[] dutycycles = new double[15];

            // Intializing dutycycles array of size 15 with value 10000
            ArrayUtils.InitArray(dutycycles, 10000.0);
            double[] newvalues = new double[15];
            // Intializing new value array of size 15 with value 1
            ArrayUtils.InitArray(newvalues, 1.0);
            int period = 1000;
            // executing CalcEventFrequency method with dutycycles, newvalues, period 
            double[] newDutyCycles = SpatialPooler.CalcEventFrequency(dutycycles, newvalues, period);
            // Expected duty cycle values are calculated manually using the formula ( (period - 1)*dutyCycle + newValue ) / period
            double[] expectedDutyCycles = new double[] { 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001, 9990.001 };
            // Veriying manually calculated duty cycle values and dutycycle values from CalcEventFrequency method are equall
            Assert.IsTrue(expectedDutyCycles.SequenceEqual(newDutyCycles));
        }

        /// <summary>
        /// It makes sure that duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method when period is 1
        /// This test ensures that duty cycles values are calculated as per the formula and updated accordingly.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Prod")]
        public void testCalcEventFrequencyP1()
        {
            int inpBits = 20;
            int numCols = 20;
            InitTestSPInstance(inpBits, numCols);

            double[] dutycycles = new double[20];
            // Intializing dutycycles array of size 20 with value 100
            ArrayUtils.InitArray(dutycycles, 100.0);
            double[] newvalues = new double[20];
            // Intializing new value array of size 20 with value 5000
            ArrayUtils.InitArray(newvalues, 5000.0);
            int period = 1;
            // executing CalcEventFrequency method with dutycycles, newvalues, period 
            double[] newDutyCycles = SpatialPooler.CalcEventFrequency(dutycycles, newvalues, period);
            // Expected duty cycle values are calculated manually using the formula ( (period - 1)*dutyCycle + newValue ) / period
            double[] expectedDutyCycles = new double[] { 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000, 5000 };
            // Veriying manually calculated duty cycle values and dutycycle values from CalcEventFrequency method are equall
            Assert.IsTrue(expectedDutyCycles.SequenceEqual(newDutyCycles));
        }
        
    }
}
