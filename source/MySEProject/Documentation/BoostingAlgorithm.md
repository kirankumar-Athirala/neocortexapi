# Introduction

This Document provides brief details of investigated articles on Boosting and definitions of the parameters included in it.

# 1. Hierarchical Temporal Memory (HTM) 

The Hierarchical Temporal Memory Cortical Learning Algorithm (HTM CLA) is an algorithm inspired by the biological functioning of the neocortex, which combines spatial pattern recognition and temporal sequence learning. It organises neurons in layers of column-like units built from many neurons, such that the units are connected into structures called areas. Areas, columns and mini-columns are hierarchically organised and can further be connected in more complex networks, which implement higher cognitive functions like invariant representations, pattern- and sequence-recognition etc. HTM CLA in general consists of two major algorithms: Spatial Pooler and Temporal Memory.The Spatial Pooler operates on mini-columns connected to sensory inputs. It is responsible to learn spatial patterns by encoding the pattern into the sparse distributed representation (SDR). The created SDR, which represents the encoded spatial pattern is further used as the input for the Temporal Memory (TM)algorithm.

- The idea behind reverse engineering the Neocortex consists of the following steps:


### 1a. Sparse Distributed Representations:
Sparse Distributed Representations (SDRs) are binary representation i.e. an array consisting of large number of bits where small percentage are 1’s represents an active neuron and 0 an inactive one. Each bit typically has some meaning (such as the presence of an edge at a particular location and orientation). This means that if two vectors have 1s in the same position they are semantically similar in that attribute.

### 1b. Encoding:
The encoder converts the native format of the data into an SDR that can be fed into an HTM system, it is the in charge of identifying which output bits should be ones and which should be zeros for a particular input value in order to capture the data's significant semantic properties. SDRs with similar input values should have a high degree of overlap.

### 1c. Spatial Pooling:

Spatial pooling Algorithm is for solving problems to represent the input active neurons (comping from sensory or motor organs) to make that the cortex learn the pattern of sequences. It accepts the input vector of different sizes and represents it into sparse vectors of same size(It kind of normalises it). The output of the Spatial Pooler represents the mini columns i.e the pyramidal neuron in the cortices. It posses certain properties like maintaining a fixed sparsity i.e no matter how many bits are on and off in the input the spatial pooler need to maintain the fixed sparsity and to maintain the overlap properties i.e two similar inputs should produce similar outputs in the columns.

#### Spacial pooler algorithm working aproach:
-  Starting with an SDR with randomly dispersed connections from each column to the input space, the process begins. A random persistence value between 0 and 1 is assigned to each link between an input bit and an output column. If the connected persistence threshold p set in the parameter initialisation is larger than the connected permanence threshold p, the input bits connected to the given column are active. If a particular amount of input bits associated to that column are activated in the input space, that column will be active. With these connections, Spatial Pooler will be able to represent various inputs as SDRs while still preserving the input's semantic content. The overlap score of the output columns is used to determine how comparable the information is. The more overlap there is between the two outputs, the closer they are.
		   
-  Only the columns with the highest overlap score will be selected as active and permitted to learn, while the remaining columns will remain unmodified. The active columns' connections that overlap the input will be strengthened by increasing the synaptic persistence value. Meanwhile, the permanence value of other connections that do not match the input will be reduced.


<img src=https://github.com/sandhyabagadi/neocortexapi/blob/Devil_Coders/source/MySEProject/All_images-used/SpacialPooling.png width="300" height="200">

### 1d. Learning:
Learning happens only in those columns of the Spatial Pooler which are active. All the connections that are falling in the input space, the permeance value will increase for them i,e the synaptic connection between them will strengthen while any connections that fall outside of the input space for those the permeance value will be decremented . Learning Spatial Pooler learns better in comparison to the Random Spatial Pooler. This step has been basically derived from the concept ‘Hebbian Learning: Neurons that fire together wire together ’.
For instance, in the diagram given below for any given column in a Spatial Pooler those cell that are connected to the active cells in the input space (i.e which are in green) their permeance will be incremented else those connected outside the input space their permeance will be decremented(the one’s in grey). No inactive column will learn anything.


<img src=https://github.com/sandhyabagadi/neocortexapi/blob/5bbc26be8a0f92203cb1657bf3fe65d3c6beacd8/source/MySEProject/All_images-used/Learning.png width="300" height="200">

### 1e. Boosting:
In order for a column in a Spatial pooler to exist it should be a winning column i.e the overlap score should be above some threshold value while non-winning columns are inhibited from learning. Only the winner columns can update their permanence values. Boosting helps to change the overlap score before the inhibition occurs giving less active columns a better chance to express themselves and diminishing columns that seem overactive. Boosting on better enables the learning of input data i.e it improves the efficiency. In other words we can say that the columns that have low overlap score are boosted so that they can better express themselves and all the columns with higher overlap score are inhibited because they are expressing themselves too much.

### 1f. Temporal Pooling:
Temporal Pooling enables us to understand the sequential pattern over time. It learns the sequences of the active column from the Spatial Pooler and predicts what spatial pattern in coming next based on the temporal context of each input .


# 2 Boosting

- Normally, Spatial Pooler will only have a few active columns that represent distinct inputs, or their active duty-cycles will be near to one. During the whole learning process, other inactive columns will never be active. This means that the output SDR can only explain a limited amount of information about the input set. More columns will be able to participate in expressing the input space thanks to the boosting technique.
			
- The boosting approach of Spatial Pooler allows all columns to be used consistently across all patterns. Even though the columns have previously learnt patterns, the boosting process is still active, causing the Spatial Pooler to forget the input. To address this issue, the Spatial Pooler now includes a new homeostatic plasticity controller that turns off boosting once the learning has reached a stable state. The output SDRs of the Spatial Pooler do not change over time, according to the research.



## 2a Description of Boosting with parameters for more understanding

Boosting can be helpful in driving columns to compete for activation. Boosting is monitored by both the activity and overlap duty cycles Following inhibition, if a column’s active duty cycle falls below the active duty cycles of neighboring columns, then its internal boost factor will increase above one. If a column's active duty cycle arises above the active duty cycles of neighboring columns, its boost factor will decrease below one, This helps drive the competition amongst columns and achieve the spatial pooling goal of using all the columns. Before inhibition, if a column’s overlap duty cycle is below its minimum acceptable value (calculated dynamically as a function of minPctOverlapDutyCycle and the overlap duty cycle of neighboring columns), then all its permanence values are boosted by the increment amount. A subpar duty cycle implies either a column's previously learned inputs are no longer ever active, or the vast majority of them have been "hijacked" by other columns. By raising all synapse permanences in response to a subpar duty cycle before inhibition, we enable a column to search for new inputs.

## 2b Boosting and HTM Spatial Pooler with Homeostatic Plasticity Control

1. The Spatial Pooler works using mini-columns that are linked to sensory inputs .It is in charge of learning spatial patterns by encoding them into a sparse distributed representation (SDR). The SDR that was constructed to represent the encoded spatial pattern is then utilised as input for the Temporal Memory (TM) method.

2. The TM is in charge of learning sequences from the SDR. As per this research Learned patterns will be forgotten and re-learned during the learning process. The Spatial Pooler oscillates between stable and unstable stable, as per the results. Furthermore, it is found that the instability is linked to a single pattern rather than a group of patterns.

4. The Spatial Pooler method uses a column boosting technique inspired by homeostatic plasticity. This process affects the balance of excitation and inhibition in brain cells and is likely vital for maintaining the cortical state of stability.

5. The Spatial Pooler's boosting keeps track of column activity and ensures that all columns are used consistently throughout all patterns. Because this process is active all of the time, it can increase columns that have previously learnt SDRs. After then, the Spatial Pooler will "forget" certain learnt patterns for a short time. If the SP is shown the lost pattern again, it will begin to learn it again.
		
6. The boosting was disabled by setting DUTY_CYCLE_PERIOD and MAX_BOOST to zero value. These two values disable boosting algorithm in the Spatial Pooler. Results show that the SP with these parameters produces stable SDRs. The SP learns the pattern and encodes it to SDR in few iterations (typically 2-3) and keeps it unchanged (stable) during the entire life cycle of the SP instance. By following this result, the stable SP can be achieved by disabling of the boosting algorithm.
		
7. Unfortunately, without the boosting mechanism, the SP generates SDR-s with unpredictive number of active mini-columns.
		
8. The further processing of memorised SDR-s will be badly effected if the number of active mini-columns in an SDR for distinct inputs is considerably variable. In this situation, SDRs with a large number of active columns would statistically yield more overlaps than SDRs with fewer active cells, which is not balanced.

9. The parameter NUM_ACTIVE_COLUMNS_PER_INH_AREA defines the percentage of columns in the inhibition area, which will be activated by the encoding of every single input pattern. Inspired by the neocortex, this value is typically set on 2%. By using the global inhibition in these experiments by the entire column set of 2048 columns the SP will generate SDRs with approx 40 active columns. The boosting mechanism inspired by homeostatic plasticity in neo-cortex solves this problem by consequent boosting of passive minicolumns and inhibiting active mini-columns. As long the learning is occurring, the SP will continuously boost mini-columns. Every time the boosting takes a place, some learned patterns (SDRs) might be forgotten, and learning will continue when the same pattern appears the next time.
		
10. It may be concluded that the boosting process has an impact on the SP's stability. The SP can reach a stable state, but SDRs with a drastically different number of active minicolumns will result. If boosting is turned on, the SP will activate mini-columns equitably, but the learning will be unstable.
		
12. The fundamental concept behind this study is to add an extra algorithm to SP that does not interfere with the present SP method in order to stabilize the SP and maintain exploiting the plasticity. The Homeostatic Plasticity Controller, a new component, implements the method used by the expanded Spatial Pooler. The controller is "connected" to the Spatial Pooler's current implementation.The input pattern and related SDR are provided from the SP to the controller after each iteration's computation. The controller keeps the boosting going until the SP reaches a stable state, which is measured in repetitions.During this period, the SP is in a stage known as "new-born" and will yield outcomes.
		
13. When the SP reaches a stable state, the new algorithm turns off the boosting and informs the application of the change. The controller keeps track of how many mini-columns are present in the overall pattern. The SP has entered the stable stage when the controller detects that all minicolumns are almost evenly employed and all visible SDRs are encoded with roughly the same amount of active mini-columns. The SP will then exit the neonatal stage and resume normal operations, albeit without the boosting.

## 2c The HTM CLA provides an algorithm inside of the SP and This algorithm consists of two parts:

### Synaptic Boost of inactive mini-columns

A mini-column is defined as inactive (weak) if the number of its connected synapses at the proximal dendrite segment is not sufficient in a learning cycle. If the number of connected synapses of a mini-column in the cycle to the current input is less than the stimulus threshold, then permanence values of all potential synapses of a mini-column will be slightly incremented by parameter si (stimulus increment).

After the permanence values are adapted, permanences of mini-columns with too less connected synapses will be slightly increased. Permanence values of all synapses of the mini-column will be incremented if the number of connected synapses is lower. The value  must be carefully chosen concerning the number of input bits N and the potential radius. In most experiments the value were typically chosen between 20-50% of the number of input bits N. Choosing higher or lower values prevents the SP to converge to the stable state.

- Bump Up Weak Columns [Refer for details](https://github.com/sandhyabagadi/neocortexapi/blob/Devil_Coders/source/MySEProject/Documentation/ReadMe.md)

### Uniform Activation of mini-columns

This implement makes sure that all mini columns in the HTM area become uniformly activated. The absence of this kind of plasticity leads to very different sparsity in the HTM area, which leads to incorrect prediction and inaccurate learning. To ensure the uniform participation of mini columns in the learning, the overall column overlap, and activation are considered. If the value of synaptic permeances defines energy stored in the synaptic connection, the goal here is to keep that energy uniformly distributed across the entire cortical area. 

The following methods contribute to Uniform Activation of mini-columns.

- Update Boost Factors 

- Update Duty Cycles 

- Update Min Duty Cycles 

[Refer for details](https://github.com/sandhyabagadi/neocortexapi/blob/Devil_Coders/source/MySEProject/Documentation/ReadMe.md)




# 3 Important parameters and their usage in spatial pooler and Boosting

|Parameter Name | Meaning |
|--- |--- |
|MAX_BOOST | The Maximum overlap boost factor, Each columns overlap gets multiplied by a boost factor before its considered for inhibition. The actual boost factor for a column is between 1.0 and the maxboost.|
|GLOBAL_INHIBITION | If TRUE global inhibition algorithm will be used. If FALSE local inhibition algorithm will be used.|
|INHIBITION_RADIUS | Defines neighbourhood radius of a column.|
|DUTY_CYCLE_PERIOD | Number of iterations. The period used to calculate duty cycles. Higher values make it take longer to respond to changes in boost. Shorter values make it more unstable and likely to oscillate.|
|WRAP_AROUND | Determines if inputs at the beginning and end of an input dimension should be considered neighbours when mapping columns to inputs.|
|LOCAL_AREA_DENSITY | Density of active columns inside of local inhibition radius. If set on value < 0, explicit number of active columns (NUM_ACTIVE_COLUMNS_PER_INH_AREA) will be used.|
|POTENTIAL_RADIUS | Defines the radius in number of input cells visible to column cells. It is important to choose this value, so every input neuron is connected to at least a single column. For example, if the input has 50000 bits and the column topology is 500, then you must choose some value larger than 50000/500 > 100.|
|MAX_SYNAPSES_PER_SEGMENT | The Maximum number of synapses allowed on a given segment.|
|SYN_PER_BELOW_STIMULUS_INC | Synapses of weak mini-columns will be stimulated by the boosting mechanism. The stimulation is done by adding of this increment value to the current permanence value of the synapse.|
|SYN_PERM_TRIM_THRESHOLD | This value is used by SP. When some permanence is under this value, it is set on zero. In this case the synapse remains the potential one and still can participate in learning. By following structural plasticity principal the synapse would become disconnected from the mini-column.|
|STIMULUS_THRESHOLD | One mini-column is active if its overlap exceeds overlap threshold θo of connected synapses.|
|IS_BUMPUPWEAK_COLOUMNS_DISABLED | Controls bumping-up of weak columns if it is true, weak columns are not updated and if it is false bumping up of weak columns take place|
|POTENTIAL_PCT | Defines the percent of of inputs within potential radius, which can/should be connected to the column.|
|UPDATE_ACTIVE_DUTYCYCLE | Computes a moving average of how often column c has been active after inhibition.|
|UPDATE_OVERLAPE_DUTY_CYCLE | Computes a moving average of how often column c has overlap greater than stimulusThreshold.| 
|ACTIVE_DUTY_CYCLE | A moving average denoting the frequency of column activation.|
|OVERLAP_DUTY_CYCLE | A moving average denoting the frequency of the column’s overlap value being at least equal to the proximal segment activation threshold.|

# 4 References

[1] Article on understanding HTM: https://medium.com/@rockingrichie1994/understanding-hierarchal-temporal-memory-f6a1be38e07e

[2] D. Dobric, "Github," [Online]. Available: https://github.com/ddobric/neocortexapi

[3] Improved HTM Spatial Pooler with Homeostatic Plasticity Control Author: Damir Dobric, Andreas Pech, Bogdan Ghitaand Thomas Wennekers. https://www.academia.edu/45090289/Improved_HTM_Spatial_Pooler_with_Homeostatic_Plasticity_control

[4] Deep Learning Author: Mariette Awad, Rahul Khanna https://link.springer.com/chapter/10.1007/978-1-4302-5990-9_9

[5] "Numenta," [Online]. Available: https://www.youtube.com/watch?v=MSwoNAODrgk

[6] Team_DevilCoders, "Github," 2022. [Online]. Available: https://github.com/sandhyabagadi/neocortexapi/blob/Devil_Coders/source/MySEProject/Documentation/ReadMe.md.