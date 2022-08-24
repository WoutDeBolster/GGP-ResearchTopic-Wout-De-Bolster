# Flow Field Pathfinding

https://user-images.githubusercontent.com/78808866/186507902-b1ffcf55-71fe-4243-8154-9302faef8b6e.mp4

# Short explanation
A flow field or a vector field is a grid where every grid cell is filled with a vector.<br/>
Each vector points towards their neighbor node closest to the goal.<br/>
When an unit goes over this cell the vector will influence the final velocity of the unit.

# When to use a flow field
* You have hunderds of units attempting to find the same location
* You need to constantly change the positions of the units
* The enviorment is dynamic

# Where do they use a flow field
* Roguelikes
* Zombie survivals
* Crowd flows
* enz...

# Calculating the flow field
Calculating the flow field requires 3 steps:<br/>
* Create a cost field
* Generate an integration field
* Generate the flow field

## The cost field
![Unity_25WLk8FuU4](https://user-images.githubusercontent.com/78808866/186485857-1de267ce-8fdd-4ebc-a948-f0dbe19ac518.png)<br/>
Each cell in the cost field represents a value.<br/>
The units will go to the cells with the lowest values.<br/>
If the value is really high then the units can't walk on that cell.<br/>
Like in this image where grey is ground (value of 1), brown is mud (value of 3) and blue is water (a really high value).<br/>
I used a grid to reperecent this, but you can also do this with tiling or other methodes.

## The integration field
![Unity_L9jfM9HygN](https://user-images.githubusercontent.com/78808866/186484662-074dca8e-4cbf-455a-824d-3631eac3fbb3.png)<br/>
The integration field is where most of the work in the flow field calculation is done.<br/>
In order to create the integration field I used a modified version of Dijkstra’s algorithm.<br/>
In mijn eigen woorden uitgelegt wat er gebeurt is:
* First we put the finnish cell int a queue
* Than we check witch neigbor cell has the cheapest cost
* Than from that finish cell we look at the closest neigbor and sum up the costs of the cells before that and the neigbor cell itself (costCellsBefore + neigborCurrentlyCheckingCost)
* And than we do it all over again untile all the tiles are numberd

## The flow field
![image](https://user-images.githubusercontent.com/78808866/186484829-cc9aebbe-5fd3-4516-ad6e-6dfc56b9d7e9.png)<br/>
The flow field takes the result of the integration field’s calculations and uses it to determine the direction of the vectors in the field.<br/>
It does this by going true all the cells and comparing the cell's value to that of its neighbors to find the lowest value.<br/>
Then we store a vector with the index that points to the lowest neighbor in that cell.

# Useful links
basic explenations:<br/>
* https://www.reddit.com/r/gamedev/comments/jfg3gf/the_power_of_flow_field_pathfinding/
* https://leifnode.com/2013/12/flow-field-pathfinding/
* https://howtorts.github.io/2014/01/04/basic-flow-fields.html

High level example with the game Planetary Annihilation:<br/>
* http://youtu.be/5Qyl7h7D1Q8?t=24m24s

More real life example with Crowd Flows:<br/>
* http://grail.cs.washington.edu/projects/crowd-flows/
