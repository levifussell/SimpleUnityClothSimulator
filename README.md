![Cloth Interacting With Ball](https://github.com/levifussell/SimpleUnityClothSimulator/blob/master/Images/ClothToSphere.png)

# Simple Unity Cloth Simulator (SUCS)
A standard Verlet point-mass cloth simulator. Relatively fast for C#, but not perfect (...yet). More stable, in terms of collisions, than Unity's standard cloth simulation.

Based on [Jakobsen 2003](https://web.mat.upc.edu/toni.susin/files/AdvancedCharacterPhysics.pdf).

Supports sphere and capsule collision.

![Cloth Interacting With Ball](https://github.com/levifussell/SimpleUnityClothSimulator/blob/master/Images/ClothToCapsule.png)

Supports 1000+ vertex _unpinned_ blouses (if that's what you need in life). I managed to get a realtime albeit stretchy blouse working with timestep=0.005 and substeps=1:

![Cloth Interacting With Ball](https://github.com/levifussell/SimpleUnityClothSimulator/blob/master/Images/ClothBlouse.png)

The outcome of this project is not a realtime cloth simulation, but one that focuses on stable collisions, with no pin requirements, yet is still reasonably quick (e.g. 10-30 FPS). I found Unity's standard cloth simulator frustratingly unstable for many-body collisions. Note that you can still get realtime performance if you greatly reduce the number of substeps; the simulation will be stable for small perturbations only, and the cloth will experience some stretching. A list of working features is below. 

### Smart Static Collisions (SSCs)

![Cloth Interacting With Ball](https://github.com/levifussell/SimpleUnityClothSimulator/blob/master/Images/ClothBlouseSmartCollisions.png)

Collision resolution is the most expensive part of cloth simulation. So if we can reduce the number of Collision Constraint objects, the better. There is a large amount of research in this area (see [BVH](https://en.wikipedia.org/wiki/Bounding_volume_hierarchy) for example), but we use a straight forward solution for the case where the likely collisions will be the same during the simulation, for example clothing on a character. Using a "Smart Static Collision" the vertices interacting with the collision object are pre-baked during the first frames of interaction. This can be updated periodically.

TODO:
* ~cloth-to-sphere interaction~.
* ~capsule-to-cloth interaction~.
* ~fast static collisions~.
* scalability test.
* port to C++ and use .dll.
