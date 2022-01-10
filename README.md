# Flow Field Pathfinding
![image](https://user-images.githubusercontent.com/78808866/148690567-36aed7bd-ec31-439b-8f26-fe4dfc0e992e.png)

# short explanation
A flow field or a vector field is a grid where every grid cell is filled with a vector.<br/>
Each vector points towards their neighbor node closest to the goal.<br/>
When an unit goes over this cell the vector will influence the final velocity of the unit.

# When to use a flow field
* You have hunderds of units attempting to find the same location<br/>
* You need to constantly change the positions of the units<br/>
* The enviorment is dynamic

# Calculating the flow field
Calculating the flow field requires 3 steps:
* Create a cost field
* Generate an integration field
* Generate the flow field

## The cost field
![image](https://user-images.githubusercontent.com/78808866/148693004-ef6b1912-2a29-4973-8b20-969103f63f9f.png)<br/>
Each cell in the cost field represents a value.<br/>
The units will go to the cells with the lowest values.<br/>
If the value is really high then the units can't walk on that cell.<br/>
Like in this image where grey is ground (value of 1), brown is mud (value of 3) and blue is water (a really high value).

## The integration field
The integration field is where most of the work in the flow field calculation is done.<br/>
In order to create the integration field I used a modified version of Dijkstra’s algorithm.<br/>
* will add steps later!!!!!!!

## The flow field
The flow field takes the result of the integration field’s calculations and uses it to determine the direction of the vectors in the field.<br/>
It does this by going true all the cells and comparing the cell's value to that of its neighbors to find the lowest value.<br/>
Then we store a vector with the index that points to the lowest neighbor in that cell.
