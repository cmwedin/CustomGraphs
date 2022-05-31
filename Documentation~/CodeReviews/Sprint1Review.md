# Sprint 1 Code Review

sprint focus: creation of graph classes and some algorithm implementations. Review includes all code completed before the start of full time development

- [Sprint 1 Code Review](#sprint-1-code-review)
  - [Low Level Components](#low-level-components)
    - [GraphNode](#graphnode)
      - [Fields Review](#fields-review)
      - [Methods Review](#methods-review)
    - [AbstractEdge](#abstractedge)
    - [DirectedEdge : AbstractEdge](#directededge--abstractedge)
    - [UndirectedEdge : AbstractEdge](#undirectededge--abstractedge)
  - [High Level Components](#high-level-components)
    - [AbstractGraph](#abstractgraph)
    - [DirectedGraph : AbstractGraph](#directedgraph--abstractgraph)
    - [UndirectedGraph : AbstractGraph](#undirectedgraph--abstractgraph)
    - [Tree : UndirectedGraph](#tree--undirectedgraph)
    - [RootedTree : Tree](#rootedtree--tree)
  - [Data structures](#data-structures)
    - [D-ary Heap](#d-ary-heap)
  - [Algorithms](#algorithms)
    - [TarjanSCCSolver](#tarjansccsolver)
    - [CycleSolver](#cyclesolver)
    - [Topological Sort](#topological-sort)
    - [ShortestPath](#shortestpath)
  - [Exceptions](#exceptions)
  - [Tests](#tests)
    - [GraphTests](#graphtests)
    - [TreeTests](#treetests)
    - [OpperatorTests](#opperatortests)
    - [HeapTests](#heaptests)
    - [CylcesTests](#cylcestests)
    - [ShortestPathTests](#shortestpathtests)

## Low Level Components

these are the components that form the building blocks graph. They have their use of reference type variable limited to enable simple copying of them and primarily reference each other through methods of their parent graph. All low level components contain a reference to their parent which is set to null to indicate when a low level component is an orphan (orphan components are used when adding components to a graph by reference.

### GraphNode
Unlike edges the base class fore nodes GraphNode is not abstract. This is because the vast majority of graphs use the same data they need stored in the node. This is not to say that there are no instances where creating inherited classes for GraphNodes will never be needed - graph nodes that have a limit to the number of outgoing/incoming edge spring to mind as potentially useful - however I have not encountered any challenge that I felt warranted such a class yet.

#### Fields Review

The public fields of a Node are as follows:
- public int ID {get => id}
- public AbstractGraph ParentGraph {get => parentGraph}
- public List<string> EdgeIDs { get => something that should be its own method 

And the private field:
- private int id
- private AbstractGraph ParentGraph;
- private List<string> outEdgeIDs
- private List<string> InEdgeIDs

At this point I can already make some points on improvements to be made to the code base. 

First regarding the public property for ParentGraph - it should be noted that this is a property for a reference type. As such any external class will still be able to access the public method and field. This is not necessarily undesirable; however, it would be prudent reflect on what this data is used for and wether an actual reference to the parent graph is needed to achieve that. It could be possible the public property for the parent could be replace by a unique id generated at construction.   

Secondly EdgeIDs - intended to allow for other accessing a list of all edges a node regardless of wether they are in or out, or repeating edges that are both, should be made a method. It already essentially is, and this will make it more clear that there is no underlying value for this property, but rather that the property evaluates code every time it is invoked, potentially a performance consideration.

#### Methods Review

GraphNode's have two constructors
- public GraphNode(int _id, AbstractGraph _parentGraph [= null], TGraphType _value [= default] )
- public GraphNode(GraphNode _node)
  
The first method is the standard method a constructing a node. We allow the parent graph to be null to represent a so called "orphan node" that is free to be added to a new graph by reference. I'm going to make the suggestion that this parameter me removed for the following reasons. Firstly, it is ambiguous and could cause confusion about what the appropriate method of constructing a node and adding it to a new graph. Should you use the node constructor and pass in the graph you want it added two? what is the difference between doing this and using the AddNode method? Further more in antagonistic use of the library this would be a potential source of errors. Is it possible to instantiate a node that reference's a graph as a parent that does no actually contain the node? This constructor does not limit its use to within the assembly for the library. 

The following change should be implemented: upon instantiation all nodes should have null set for their parent. They should then be added to a graph by reference using AddNode(node), provided the type of graph allows that.

The second constructor is the copy constructor and straight forwardly copies the values of all the fields of the argument onto a new node instance. setting the parent to null. This is fine and I have no recommended changes.

Nodes contain the following methods for Accessing its Fields from other classes.
- public ReadOnlyCollection<AbstractEdge> GetOutEdges()
- public ReadOnlyCollection<AbstractEdge> GetInEdges()

It should have the method GetEdgeIDs, which is currently the EdgeIDs property discussed in the fields section. Some remarks on these is that first of all - they do not implement error control for node who do not yet have a parent. This should obviously be remedied, especially with the change suggested above. 

Further, why are we returning these as ReadOnlyCollections?
### AbstractEdge

### DirectedEdge : AbstractEdge

### UndirectedEdge : AbstractEdge

## High Level Components

The high level components of this library are the graphs themselves. A graph is defined by a list of edges and a list of nodes (Stored internally as dictionaries with integer keys for node and string keys for edges (the two nodes they connect separated by a comma).

### AbstractGraph

### DirectedGraph : AbstractGraph

### UndirectedGraph : AbstractGraph

### Tree : UndirectedGraph

### RootedTree : Tree

## Data structures

These are data structures that don't inherit from AbstractGraph but use them in there underlying implementation nonetheless. Note that of many of these it would be more pragmatic to implement them in a dependency free manner however they have proved useful as a method of testing the library while also creating classes to enable future functionality

### D-ary Heap

## Algorithms

These are static classes to solve graph theory problems such as "what is the shortest path between these two nodes" or "does this graph contain cycles or any strongly connected components

### TarjanSCCSolver

### CycleSolver

### Topological Sort

### ShortestPath

## Exceptions

During earlier development I was overly eager to throw exceptions during error/input handling. As such I anticipate many of the following exceptions will be candidates for removal

## Tests

These tests are for the moment fairly non-adversarial intended to verify the code is preforming as intended during standard use rather than test specifically designed to attempt to cause failure.

### GraphTests

### TreeTests

### OpperatorTests

### HeapTests

### CylcesTests

### ShortestPathTests
