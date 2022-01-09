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

# Calculating the flow Field
there are 3 steps in calculating a flow field
* Create a cost field
* Generate tan integration field
* generate the flow field

