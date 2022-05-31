# Custom Graphs
this is a unity package I have been developing to have a robust graph theory toolset across my projects. It is currently in its first open source alpha version 0.0.2. 
The current features of the pacakage are classes for representing directed / undirected graphs - and the special cases of trees and rooted trees. Classes for representing the components of a graph such as edges and nodes. Static classes for solving Tarjan's Strongly Connected Component on directed graphs, and for preforming Khan's Topological Sort or finding shortest paths on Directed Acyclic Graphs (called DAG's going forward). And lastly a D-ary heap data structure to test a use case of the rooted tree class. 

Documentation for the package is a work in progress however it will be my primary develoment on this project focus over the next two weeks.

## Project Goals
Before I go in depth into the current state of the project and my plans for it going forward, I want to state what the goals of this project is and what use cases it is designed for. 

As stated above - this project is intended to be used as a graph theory toolset for game development in Unity. Its focus is on being robust and extendable so that it can be as broadly useful as possible. As such, it prioitizes robustness of systems rather than raw efficiency in its features (for example creating an entire class for edges rather than implicitly in the nodes class as a list of connected nodes). It is not intended to be a highly specialized and efficient implementation of these algorithims, but rather "efficient enough" for use on the scale of game systems - say, sparse graphs with ~10^3 nodes (I'll note that for the moment this bound is one I arrived at heuristically not rigirously).

The ultimate goals of this project is a library with the underlying logic for grid system (hex or euclidean), pathfinding, tree systems, AI state machines, or any other game system that can be represnted as a set of things with some set of connections between them.  

## State-of-Project 5/30 (sprint 1 report)

The initial goals of this spirint where the following:
- Create the classes needed to represent graphs of an arbitrary type (e.g. with without weights, directed or undirected) as well as notable special cases of graphs such as trees, rooted or otherwise  
- Implement some key algorithims graph theory algorithims that will be needed going forward. In particular shortest path algorithims

The first goal was a full success. The types of graphs in general are directed vs undirected, and weighted versus unweigthed. For simplicity sack we consider an unweighted graph to be a weighted graph in which the weight is always one. DirectedGraph and UndirectedGraph are the two highest level non-abstract graph - although they both inherit from AbstractGraph for shared functionality. I have also implemented a subclass of undirected graph for Tree (which are definied as a fully connected graph with no cycles), and a subclass of them for rooted trees. 

In development of a method to verify if a given graph was a tree, I decided it would be usefull to have opperators to create a copy of a graph with a given node removed. This in turn caused me to revalute my use of reference type versus value type member properties of child components of a graph(its nodes and edges). A significant portion of the first half of this sprint was spent refactoring how the low level classes of nodes and edge where structure to minimize the use of reference type variable. Currently they both contain a reference to their parent graph, which can be null to indicate the object is an orphan and free to be added to other graphs (more details about this will be contained in the documention to come over the next sprint), and all the other member properties of them are value types (or a reference to a list of value types). They use methods of their parent graph to translate those int/strings into references to other components of the graph.

While said opperators where implemented, a different method was settled upon for verifying trees, so aside from the refactor developing them entailed, which has proven to be an excelent decision, they have yet to return on investment. Partly as a result of the delay implenting these opperators, progress on the second front of this sprint was limited. Beyond the Tarjan SCC solver algorithim and Khan topologic sort algorithim, which where already implemented by the start of this sprint, I have also implemented and agorithim to detect cycles in undirected graphs (used to verify trees) and to find the single source shortest paths in a directed acyclic graph. Algorithims I planned to implement this sprint that have been postponed include Dijkstra's shortest path algorithim and Floyd Warshall all pairs shortest path. I also at one point considered implementing belman ford this sprint as well; however, I decided that it has too niche of a use-case over Floyd-Warshall (large graphs with negative cycles specifiacally) to implement this. 

While I had to postpone the implementation of Dijkstra's algorithim I did make some progress to it in the form of developing a d-ary data structure as a test use-case of the rooted tree class. This stucure is needed for efficient implementation of Dijkstra and will also prove usefull in implementing the planned Prim's minimum spanning tree algorithim.

## Next Steps

My focus for the project for the sprint over the coming two weeks will be documentation and code review. I want to zoom out and take a look at the code base as a whole without being focused in the weeds of implemnting details. This review will be the sprints first priority. Improvements I identify that can be implemented in a reasonably short time frame (say a few hours of work or less) will be implemented as part of the review; however, more involved task will be left for nest sprint to focus on the primary goal of this srint. 

Once this review is complete and minor improvments implemnted - the next goal will be creating through documentation of the existing code. I want to do this after the review so I am aware of any major refactors I anticipate implemnting while writing it. This sprint's deadline is 6/13.   
