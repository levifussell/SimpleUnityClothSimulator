![Cloth Interacting With Ball](https://github.com/levifussell/SimpleUnityClothSimulator/blob/master/Images/ClothToSphere.png)

# SimpleUnityClothSimulator (SUCS)
A simple Verlet point-mass cloth simulator. Relatively fast for C#, but not perfect (yet). More stable in terms of collisions than Unity's standard cloth sim. at least.

Based on [Jakobsen 2003](https://web.mat.upc.edu/toni.susin/files/AdvancedCharacterPhysics.pdf).

Supports sphere and capsule collision.

![Cloth Interacting With Ball](https://github.com/levifussell/SimpleUnityClothSimulator/blob/master/Images/ClothToCapsule.png)

The outcome of this project is not a realtime cloth simulation, but one that is stable with collision, yet still fast. I found Unity's base cloth simulator frustratingly unstable for many-body interactions. A list of working features is below. 

TODO:
* ~cloth-to-sphere interaction~.
* ~capsule-to-cloth interaction~.
* scalability test.
* port to C++ and use .dll.
