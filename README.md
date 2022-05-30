# Custom Graphs
this is a unity package I have been developing to have a robust graph theory toolset across my projects. It is currently in its first open source alpha version 0.0.2. 
The current features of the pacakage are classes for representing directed / undirected graphs - and the special cases of trees and rooted trees. Classes for representing the components of a graph such as edges and nodes. Static classes for solving Tarjan's Strongly Connected Component on directed graphs, and for preforming Khan's Topological Sort or finding shortest paths on Directed Acyclic Graphs (called DAG's going forward). And lastly a D-ary heap data structure to test a use case of the rooted tree class. 

Documentation for the package is a work in progress however it will be my primary develoment on this project focus over the next two weeks.

## Project Goals
Before I go in depth into the current state of the project and my plans for it going forward, I want to state what the goals of this project is and what use cases it is designed for. 

As stated above - this project is intended to be used as a graph theory toolset for game development in Unity. Its focus is on being robust and extendable so that it can be as broadly useful as possible. As such, it prioitizes robustness of systems rather than raw efficiency in its features (for example creating an entire class for edges rather than implicitly in the nodes class as a list of connected nodes). It is not intended to be a highly specialized and efficient implementation of these algorithims, but rather "efficient enough" for use on the scale of game systems - say, sparse graphs with ~10^3 nodes (I'll note that for the moment this bound is one I arrived at heuristically not rigirously).

The ultimate goals of this project is a library with the underlying logic for grid system (hex or euclidean), pathfinding, tree systems, AI state machines, or any other game system that can be represnted as a set of things with some set of connections between them.  

## State-of-Project 5/30 (sprint 1 report)
