# Analyse and describe boosting algorithm (WS21/22)
-----------------
**Team Members**

- Chinmaya Nithin Dimmiti, chinmaya.dimmiti@stud.fra-uas.de
- KiranKumar Athirala, kirankumar.athirala@stud.fra-uas.de
- Sandhya Bagadi, sandhya.bagadi@stud.fra-uas.de


# Project Description


Boosting Algorithm uses Spatial Pooler and Spatial Pattern learning, This algorithm uses internally a boosting algorithm which makes sure that the unused mini columns and weak synapses periodically get boosted.

The main idea behind boosting is that the SDR's of symbols uses a wider range of cells by making the most active cells less active and least active cells more active. This is done via scalar multiplication of a boosting matrix over the spatial pooler to change permeances of each cell.

## 1. Objective


- Is to Analyse which methods are related to boosting.

- Identify which parameters of the HtmConfig class are used by the boosting algorithm.

- Execute SpatialLearning experiment and document what changes when changing some of boosting relevant parameters.

- and to Document boosting

## 2. Approach

### 2a. Executed Test Cases
 Executed the program using Spatial Pattern Learning experiment by updating/changing different boosting parameters to observe the affect of these parameters on overall stability, the following cases are shown below.

Case1: Changed the Duty Cycle Period and max Boost to (50000 & 1.0) by having other values as constant.
[parameter values](https://github.com/sandhyabagadi/neocortexapi/blob/cb9d44f65099e3e148d6d04ac5f4551175851ac6/source/MySEProject/All_images-used/change%20of%20parameter%20values.png)

Case2: With Duty Cycle Period and max Boost as (100000 & 5.0).
[values-image](https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/All_images-used/case2.png)

Case3: Changed Duty Cycle Period and max Boost to (150000 & 10.0).
[values-image](https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/All_images-used/case3.png)

Case4: Cycle Period, max Boost as (5.0 & 100000) and IsBumpUpWeakColumns (true)
[values-image](https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/All_images-used/case4.png)

Case5: Changed Duty Cycle Period- 10000, max Boost- 5.0, inputBits- 100, numColumns -1024, double max- 50.
[values-image](https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/All_images-used/case5.png)

Case6: Changed SynPermBelowStimulusInc, SynPermTrimThreshold (0.1 & 0.08), inputBits- 100, numColumns- 1042, double max- 50.
[values-image](https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/All_images-used/case6.png)

Case7: Changed SynPermBelowStimulusInc and SynPermTrimThreshold (0.5 & 0.09) with other values intact.
[values-image](https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/All_images-used/case7.png)

### 2b Code wrtten to generate CSV files for each method used.

- Generated CSV files for individual methods mentioned below with necessary data which will help in the analysis of boosting algorithm. 

[Code to generate CSV files](https://github.com/sandhyabagadi/neocortexapi/blob/43358e4ee36cb1d8cf44bb39384ca7b2e62ad24d/source/MySEProject/SpatialPooler.cs#L767)
- In Spatial Pooler '2 types' of methods are used boosting algorithm.
Those methods are as follows:

### Synaptic Boost of inactive mini-columns
A mini-column is defined as inactive (weak) if the number of its connected synapses at the proximal dendrite segment is not sufficient in a learning cycle. If the number of connected synapses of a mini-column in the cycle to the current input is less than the stimulus threshold, then permanence values of all potential synapses of a mini-column will be slightly incremented by parameter si (stimulus increment).

 - ### Bump Up Weak Columns comes under Synaptic boosting:

 [CSV-code for Bump Up Weak Columns](https://github.com/sandhyabagadi/neocortexapi/blob/43358e4ee36cb1d8cf44bb39384ca7b2e62ad24d/source/MySEProject/SpatialPooler.cs#L767)

### Uniform Activation of mini-columns

The second part of the plasticity implementation makes sure that all mini-columns in the HTM area become uniformly activated and following 3 comes under this.

- ### Update Duty Cycles :

[CSV-code for updatedutycycles](https://github.com/sandhyabagadi/neocortexapi/blob/43358e4ee36cb1d8cf44bb39384ca7b2e62ad24d/source/MySEProject/SpatialPooler.cs#L566)                

- ### Update Boost Factors :

[CSV-code for Update Boost Factors](https://github.com/sandhyabagadi/neocortexapi/blob/43358e4ee36cb1d8cf44bb39384ca7b2e62ad24d/source/MySEProject/SpatialPooler.cs#L1358)

- ### Update Min Duty Cycles :

[CSV-code for Update Min Duty Cycles](https://github.com/sandhyabagadi/neocortexapi/blob/43358e4ee36cb1d8cf44bb39384ca7b2e62ad24d/source/MySEProject/SpatialPooler.cs#L389)


## Unit- Test for each Method

### 1. BumpUpWeakColoumns.

This method increases the permanence values of synapses of columns whose overlap level is too low. Such columns are identified by having an overlap duty cycle (activation frequency) that drops too much below those of their peers. The permanence values for such columns are increased.

#### Description: 

- First by using the parameters setInputDimensions and setColumnDimensions we set the test synapses of columns. In case1 we considered SynPermBelowStimulusInc = 0.01; and SynPermTrimThreshold = 0.05;

- Then the following condition is tested on a pool of test permanence values(testingpermanences):

**Condition :** If 'OverlapDutyCycles < MinOverlapDutyCycles' the column is considered as a weak column and hence the permanence values of synapses of columns are increased. Using this condition Expected permanences are calculated.
```csharp 
mem.HtmConfig.OverlapDutyCycles = new double[] { 0, 0.009, 0.1, 0.001, 0.002 };
mem.HtmConfig.MinOverlapDutyCycles = new double[] { .01, .01, .01, .01, .01 };
```
- Both the test and expected pool of permanence values are fed into the Bumpupweakcoloumns method in spacial pooler to compare and conclude the test case.
```csharp
sp.BumpUpWeakColumns(mem);
for (int i = 0; i < mem.HtmConfig.NumColumns; i++)
{
double[] calculatedperms = mem.GetColumn(i).ProximalDendrite.RFPool.GetDensePermanences(mem.HtmConfig.NumInputs);
for (int j = 0; j < ExpectedPermanences[i].Length; j++)
{
Assert.IsTrue(Math.Abs(ExpectedPermanences[i][j] - calculatedperms[j]) <= 0.01);
}
```
[link to BumpUpWeakColoumnUnitTest](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialPoolerBumpUpWeakColoumns-Unit-Test.cs#L51)
#### 1a. SynPermBelowStimulusInc :

Synapses of weak mini-columns will be stimulated by the boosting mechanism. The stimulation is done by adding of this increment value to the current permanence value of the synapse.

#### 1b. SynPermTrimThreshold :

This value is used by SP. When some permanence is under this value, it is set on zero. In this case the synapse remains the potential one and still can participate in learning. By following structural plasticity principal the synapse would become disconnected from the mini-column.

**Case1:** 
Tested if the permanence values are getting updated correctly in Bumpupweakcoloumns method with 
SynPermBelowStimulusInc = 0.01;
SynPermTrimThreshold = 0.05;

[link to the case1 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialPoolerBumpUpWeakColoumns-Unit-Test.cs#L52)

**Case2:** 
Testing to make sure the permanence values doesn’t get updated when all OverlapDutyCycles are greater than MinOverlapDutyCycles (as it will not be considered as a weak column with this condition the permanence values won’t get updated).

[link to the case2 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialPoolerBumpUpWeakColoumns-Unit-Test.cs#L188)

### 2. UpdateBoostFactor.

The boost factors are used to increase the overlap of active columns to improve their chances of becoming active and hence encourage participation of more columns in the learning process. This means that columns that have been active enough have a boost factor of 1, meaning their overlap is not boosted. Columns whose active-duty cycle drops too much below that of their neighbours are boosted depending on how infrequently they have been active. The more infrequent, the more they are boosted.

Description:

This unit-test helps in verifying the absolute values of manually calculated Boost Factors from the below mentioned formulae and Boost Factor values from UpdateBoostFactors method.

**Formula :** ` boost =(1-maxBoost)/minDuty*activeDutyCycle + maxBoost `

[link to UpdateBoostFactorUnitTest](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerUpdateBoostfactor-Unit-Test.cs#L52)

#### MaxBoost: 
The Maximum overlap boost factor, Each columns overlap gets multiplied by a boost factor before its considered for inhibition. The actual boost factor for a column is between 1.0 and the maxboost.

**Case1:** 
Tested Boost Factors are updated as per the mathematical formula defined in UpdateBoostFactors method with maxboost 10

[Link to case1 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerUpdateBoostfactor-Unit-Test.cs#L52)

**Case2:** 
Tested Boost Factors are updated as per the mathematical formula defined in UpdateBoostFactors method with maxboost 1

[Link to case2 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerUpdateBoostfactor-Unit-Test.cs#L128)

**Case3:** 
Tested Boost Factors are not updated when all minActiveDutyCycles are 0 ( A boost factor of 1.0 is used if the duty cycle is >= minOverlapDutycycle )

[Link to case3 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerUpdateBoostfactor-Unit-Test.cs#L89)

### 3. CalcEventFrequency.


Calculates the normalised counter value of the frequency of an event. Event can be overlap or the activation of the column.  Updates a duty cycle estimate with a new value. This is a helper function that is used to update several duty cycle variables in the Column class, such as: overlapDutyCucle, activeDutyCycle, minPctDutyCycleBeforeInh, minPctDutyCycleAfterInh, etc. returns the updated duty cycle.

Description: This Unit-test helps in Verifying if manually calculated duty cycle values and dutycycle values from CalcEventFrequency method are equal or not with the help of below mentioned formulae.


   ` dutyCycle = (period - 1)*dutyCycle + newValue / period `       
                  

[link to CalcEventFrequencyUnitTest](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerCalcEventFrequency-Unit-Test.cs#L80)             

**Case1:** 
Tested duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 1000

[Link to case1 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerCalcEventFrequency-Unit-Test.cs#L80)

**Case2:** 
Tested duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 1

[Link to case2 code](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialpoolerCalcEventFrequency-Unit-Test.cs#L109)

### 4. UpdateMinDutyCycles.

It updates the minimum duty cycles for SP that uses global inhibition and sets the minimum duty cycles for the overlap and activation of all columns to be a percent of the maximum in the region, specified by Min Overlap Duty Cycles and min Pct Active-Duty Cycle respectively.

**Description :**

Whenever the UpdateMinDutyCycles method is called in the SP its either Local or Global Inhibition. Through inhibition radius we get the size of the local neighborhood, Then the Maximum value in the neighborhood of maxActiveDuty and maxOverlapDuty is multiplied with MinPctActiveDutyCycles and MinPctOverlapDutyCycles Respectively to be a percent of the maximum in the region.

```csharp 
{
int[] neighborhood = GetColumnNeighborhood(c, i, this.InhibitionRadius);

double maxActiveDuty = ArrayUtils.Max(ArrayUtils.ListOfValuesByIndicies(c.HtmConfig.ActiveDutyCycles, neighborhood));
double maxOverlapDuty = ArrayUtils.Max(ArrayUtils.ListOfValuesByIndicies(c.HtmConfig.OverlapDutyCycles, neighborhood));

c.HtmConfig.MinActiveDutyCycles[i] = maxActiveDuty * c.HtmConfig.MinPctActiveDutyCycles;

c.HtmConfig.MinOverlapDutyCycles[i] = maxOverlapDuty * c.HtmConfig.MinPctOverlapDutyCycles;
```
[link to UpdateMinDutyCyclesUnitTest](https://github.com/sandhyabagadi/neocortexapi/blob/be591e016338b1e2f28751e27094c1f962109f89/source/MySEProject/Unit-Tests/SpacialPoolerUpdateMinDutyCycles-Unit-Test.cs#L77)   

#### InhibitionRadius: 
The inhibition radius determines the size of a column's local neighborhood. A cortical column must overcome the overlap score of columns in its neighborhood in order to become active. This radius is updated every learning round. It grows and shrinks with the average number of connected synapses per column.
#### Wraparound:
 Determines if inputs at the beginning and end of an input dimension should be considered neighbours when mapping columns to inputs.

**Case1:** (LOCAL) Testing Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCycleLocal without wrap around

**Case2:** (LOCAL) Testing Min Duty cycles are updated as per the mathematical formula defined in UpdateMinDutyCycleLocal with wraparound

**Case3:** (GLOBAL) Testing duty cycles are updated as per the mathematical formula defined in CalcEventFrequency method with period 1

[Link to code](https://github.com/sandhyabagadi/neocortexapi/blob/a14e6e7cba0224b0ddb2e3c30bd948159be6f45a/source/MySEProject/Unit-Tests/SpacialPoolerUpdateMinDutyCycles-Unit-Test.cs#L75)


## 3. Observations

Results of the changing BoostFactor parameters are updated in the CSV files for further graph generation and analysis.

Case 1: In this case the boost value is kept 1.0 and duty cycle period value 50000. By changing max boost and duty cycle period the stability is attained at cycle=0153, i=82, cols=41, s=100 and we observed that initially the SDR is not representing complete values and the program is slow. When the boosting parameter is activated the SDR is representing complete values and the total time to complete the program is 46:00 minutes.

- Example: The below image shows the output and graphs show the output of Permeance values and active-Duty Cycles vs boost Factors.

<img src=https://github.com/sandhyabagadi/neocortexapi/blob/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/Output%20files/StableState%20Output%203.PNG width="800" height="300">

<img src=https://github.com/sandhyabagadi/neocortexapi/blob/47333fe2cd5d1ca2d43d21c3e1c1298a4dd5a24d/source/MySEProject/All_images-used/case1%20permanence.png width="700" height="400">

<img src=https://github.com/sandhyabagadi/neocortexapi/blob/47333fe2cd5d1ca2d43d21c3e1c1298a4dd5a24d/source/MySEProject/All_images-used/case1%20activedutycycle%20vs%20boost.png width="700" height="400">

Case 2: cycle=0232, i=0, cols=40, s=100 we again observed that initially the SDR is not representing complete values and the program is slow. When the boosting parameter is activated the SDR is representing complete values and the total time to complete the program is same when compared with case1.  

Case 3: cycle=096, i=0, cols=41, s=100 and the total time taken to complete the program is 28:00 minutes When the boosting parameter is activated the SDR is representing complete values and the total time to complete the program is reduced when compared with case1 and case2 (42 minutes).

Case 4: In this case we decreased the boost value to 5.0 and duty cycle period value 100000 and changed one more parameter which plays a vital role for making it stable is “BUMP UP WEAK COLUMNS” to true. By changing max boost, duty cycle period and Bump Up Weak Colum to true the stability is attaining at cycle=096, i=0, cols=41, s=100 and this time we observed that it disables all the weak columns

Case 5: cycle=0331, i=41, cols=20, s=100 and the total time taken to complete the program is 2 minutes. We observed that when we reduce these values the stability increases, and it reduces the execution time. 

Case 6: By changing Syn Perm Below Stimulus Inc, Syn Perm Trim Threshold, inputBits, numColumns, double max, the stability is attaining at cycle=0100, i=40, cols=20, s=100 and the total time taken to complete the program is 14 minutes.

[Further more the detailed description of the output observation can be found in this PDF](https://github.com/sandhyabagadi/neocortexapi/blob/Devil_Coders/source/MySEProject/Documentation/Analyze_and_Describe_BoostingAgorithm.pdf)
                  
## 4.Goals Achieved

- Experiments with different variations of MaxBoost, DutyCyclePeriod,and SynPermBelowStimulusInc have been done to analyze the changes in the SDR.

- The experimental data and graphical analysis can be found from this [link](https://github.com/sandhyabagadi/neocortexapi/tree/4dae45f4992a9d6fa8d585516b3e882a4ce255cf/source/MySEProject/Output%20files)

- We have written Unit-Tests for each method involved in boosting from htmconfig.

- Updated the code to generate CSV files for output storage which helps in graphical representation and analysis.


## 6.References


[1] The HTM Spatial Pooler—A Neocortical Algorithm for Online Sparse Distributed Coding Author: Yuwei Cui, Subutai Ahmad, Jeff Hawkins| Numenta Inc.

[2] Improved HTM Spatial Pooler with Homeostatic Plasticity Control Author: Damir Dobric, Andreas Pech, Bogdan Ghitaand Thomas Wennekers.

[3] "Numenta," [Online]. Available: https://numenta.com/resources/biological-and-machine-intelligence/spatial-pooling-algorithm/.

[4] Article on understanding HTM: https://medium.com/@rockingrichie1994/understanding-hierarchal-temporal-memory-f6a1be38e07e

[5] "Numenta," [Online]. Available: https://www.youtube.com/watch?v=MSwoNAODrgk

[6] D. Dobric, "Github," [Online]. Available: https://github.com/ddobric/neocortexapi-classification.

[7] Team_Devil_coders, "Github(BoostingAlgorithm)," 2022. [Online]. Available: https://github.com/sandhyabagadi/neocortexapi/blob/Devil_Coders/source/MySEProject/Documentation/BoostingAlgorithm.md.