# Sprint 1 Code Review

sprint focus: creation of graph classes and some algorithim implementations. Review includes all code completed before the start of full time development

## Low Level Components

these are the components that form the building blocks graph. They have their use of reference type variable limited to enable simple copying of them and primarily reference each other through methods of their parent graph. All low level components contain a reference to their parent which is set to null to indicate when a low level component is an orphan (orphan components are used when adding components to a graph by reference.

### GraphNode

### AbstractEdge

### DirectedEdge : AbstractEdge

### UndirectedEdge : AbstractEdge

## High Level Components

Ther high level components of this library are the graphs themselves. A graph is defined by a list of edges and a list of nodes (Stored internally as dictionaries with interger keys for node and string keys for edges (the two nodes they connect seperated by a comma).

### AbstractGraph

### DirectedGraph : AbstractGraph

### UndirectedGraph : AbstractGraph

### Tree : UndirectedGraph

### RootedTree : Tree

## Data structures

These are data structures that dont inherit from AbstractGraph but use them in there underlying implentation nontheless. Note that of many of these it would be more pragmatic to implement them in a depndency free maner however they have proved usefull as a method of testing the library while also creating classes to enable future functionality

### D-ary Heap

## Alogrithims

These are static classes to solve graph theory problems such as "what is the shortest path between these two nodes" or "does this graph contain cycles or any strongly connected components

### TarjanSCCSolver

### CycleSolver

### Topological Sort

### ShortestPath

## Exceptions

During earlier development I was overly eager to throw exceptions during error/input handling. As such I anticipate many of the following exceptions will be candadites for removal

## Tests

These test for the moment fairly non-adversarial intended to verify the code is preforming as intentded furing standard use rather than test specifically designed to attempt to cause failure.

### GraphTests

### TreeTests

### OpperatorTests

### HeapTests

### CylcesTests

### ShortestPathTests
