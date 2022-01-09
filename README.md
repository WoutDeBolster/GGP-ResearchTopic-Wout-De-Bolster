# Flow Field Pathfinding
![image](https://user-images.githubusercontent.com/78808866/148690567-36aed7bd-ec31-439b-8f26-fe4dfc0e992e.png)

# short explanation
A flow field ore a vector field is a grid where every grid cell is filled with a vector.
each vector points towards their neighbor node closest to the goal.
When an unit goes over this cell the vector will influence the final velocity of the unit.

# When to use a flow field
* you have hunderds of units attemting to fint the same location
* you need to constantly change the positions of units
* the enviorment is dynamic

# Calculating the flow field
there are 3 steps in calculating a flow field
* Create a cost field
* Generate tan integration field
* generate the flow field

## The cost field
![image](https://user-images.githubusercontent.com/78808866/148693004-ef6b1912-2a29-4973-8b20-969103f63f9f.png)
Each cell int the cost field represents a value.
The units will go to the cells with the lower values.
if the value is realy high than the units can't walk on that cell.
Like in this image where grey isground (value of 1), brown is mud (value of 3) and blue is water (a really high value).

## The integration field
The integration field is where most of the work in the flow field calculation is done.
In order to create the integration field I used a modified version of Dijkstra’s algorithm.
* will add steps later!!!!!!!

## The flow field
flow field takes the result of the integration field’s calculations and uses it to determine the direction of the vectors in the field.
It does this by going true all the cells and comparing the cell's value to that of its neighbors to find the lowest value.
than we store a vector withe the index that points to the lowest neighbor in that cell.
