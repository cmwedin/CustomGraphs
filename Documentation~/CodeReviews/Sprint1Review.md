# Sprint 1 Code Review

sprint focus: creation of graph classes and some algorithm implementations. Review includes all code completed before the start of full time development. When referencing the type names of graph classes i will be neglecting the <TGraphType> generic type parameter for readability.

- [Sprint 1 Code Review](#sprint-1-code-review)
  - [Low Level Components](#low-level-components)
    - [GraphNode](#graphnode)
      - [Fields Review](#fields-review)
      - [Methods Review](#methods-review)
    - [AbstractEdge](#abstractedge)
      - [Fields Review](#fields-review-1)
      - [Methods Review](#methods-review-1)
    - [DirectedEdge : AbstractEdge](#directededge--abstractedge)
    - [UndirectedEdge : AbstractEdge](#undirectededge--abstractedge)
  - [High Level Components](#high-level-components)
    - [AbstractGraph](#abstractgraph)
      - [Fields Review](#fields-review-2)
      - [Methods Review](#methods-review-2)
    - [DirectedGraph : AbstractGraph](#directedgraph--abstractgraph)
    - [UndirectedGraph : AbstractGraph](#undirectedgraph--abstractgraph)
    - [Tree : UndirectedGraph](#tree--undirectedgraph)
    - [RootedTree : Tree](#rootedtree--tree)
  - [Data structures](#data-structures)
    - [D-ary Heap](#d-ary-heap)
      - [Fields](#fields)
      - [Methods](#methods)
  - [Algorithms](#algorithms)
    - [TarjanSCCSolver](#tarjansccsolver)
      - [Fields](#fields-1)
      - [Methods](#methods-1)
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

These are the components that form the building blocks graph. They have their use of reference type variables limited to enable simple copying of them and primarily reference each other through methods of their parent graph. All low-level components contain a reference to their parent which is set to null to indicate when a low-level component is an orphan (orphan components are used when adding components to a graph by reference.)

### GraphNode
Unlike edges, the base class for nodes GraphNode is not abstract. This is because the vast majority of graphs have to same functionality they need of their nodes. This is not to say that there are no instances where creating inherited classes for GraphNodes will be needed - graph nodes that have a limit to the number of outgoing/incoming edge spring to mind as potentially useful - however I have not felt the need to implement such a feature yet.

#### Fields Review

The public fields of a Node are as follows:
- public int ID {get => id}
- public AbstractGraph ParentGraph {get => parentGraph}
- public List<string> EdgeIDs { get => something that should be its own method }
- public List<string> NeighborIDs { get => something that should be its own method }

And the private fields:
- private int id
- private AbstractGraph ParentGraph;
- private List<string> outEdgeIDs
- private List<string> InEdgeIDs

First regarding the public property for ParentGraph - it should be noted that this is a property for a reference type. As such any external class will still be able to access the public method and field. This is not necessarily undesirable; however, it would be prudent reflect on what this data is used for and whether an actual reference to the parent graph is needed to achieve that. It could be possible the public property for the parent could be replace by a unique id generated at construction.   

Secondly, the EdgeIDs property - intended to allow for other accessing a list of all edges a node regardless of whether they are in or out, or repeating edges that are both - should be made a method. It already essentially is, and this will make it more clear that there is no underlying value for this property, but rather that the property evaluates code every time it is invoked, potentially a performance consideration.

The same is true of NeighborIDs however this code is older and looking the instances it is referenced I feel comfortable recommending this function be removed.

#### Methods Review

GraphNode's have two constructors
- public GraphNode(int _id, [AbstractGraph _parentGraph = null], [TGraphType _value = default] )
- public GraphNode(GraphNode _node)
  
The first method is the standard method of constructing a node. We allow the parent graph to be null to represent a so called "orphan node" that is free to be added to a new graph by reference. I'm going to make the suggestion that this parameter me removed for the following reasons. Firstly, it is ambiguous and could cause confusion about what the appropriate method of constructing a node and adding it to a new graph is. Should you use the node constructor and pass in the graph you want it added to? What is the difference between doing this and using the AddNode method? Furthermore in antagonistic use of the library this would be a potential source of errors. Is it possible to instantiate a node that reference's a graph as a parent that does no actually contain the node? This constructor does not limit its use to within the assembly for the library. 

The following change should be implemented: upon instantiation all nodes should have null set for their parent. They should then be added to a graph by reference using AddNode(node), provided the type of graph allows that.

The second constructor is the copy constructor and straight forwardly copies the values of all the fields of the argument onto a new node instance. setting the parent to null. This is fine and I have no recommended changes.

Nodes contain the following methods for Accessing its Fields from other classes.
- public ReadOnlyCollection<AbstractEdge> GetOutEdges()
- public ReadOnlyCollection<AbstractEdge> GetInEdges()

It should also have the method GetEdgeIDs, which is currently the EdgeIDs property discussed in the fields section. Some remarks on these is that, first of all, they do not implement error control for nodes whom do not yet have a parent. This should obviously be remedied, especially with the change suggested above. Secondly, why is this the only place we are using ReadOnlyCollections for this type of function? This behavior should be standardized for all functions like this if it is the desired behavior. 

Lastly are the methods nodes have for modification of themselves. 
- internal void AddEdge(AbstractEdge _edge)
- internal void RemoveEdge(AbstractEdge _edge)
- internal void ClearEdges()
- internal void SetParent(AbstractGraph _parent)
- public void SetValue(TGraphType _value)

I will note here that as a rule this these functions do not modify the parent graph themselves and thus should only be used internally within the CustomGraphs assembly, invoked from the appropriate method in the parent graph. This is a potential source of error in antagonistic use and something that should be tested for; however, provided I am able to restrict the use of them outside of the assembly it is a "code smell" I'm willing to accept for now. That said, I want to put a pin in this for future refactoring. A more serious issue is that again some of these functions no not handle the case where they are called on a node that does not have a parent. This is easily remedied. Beyond that however these methods look fine.

One last note for this class is that the reason we only allow adding an edge to a node by reference is that whether an edge gets added to outEdges depends on the type of edge it is. Undirected Edges are always added to outEdges, however Directed Edges are only add to their source nodes outEdges.

### AbstractEdge
This is the base level class for all edges. It is abstract because all edges must specify if they are directed or undirected - there is no presumed default. 

#### Fields Review
AbstractEdge has the following public value type fields:
- public int SourceNodeID {get => sourceNodeID}
- public int SinkNodeID {get => sinkNodeID}
- public string ID {get => id}
- public float Weight {get => weight}

They all also have straight forward private fields underlying each property. They also have the public reference type field
- public AbstractGraph ParentGraph {get => parentGraph}

Which again has a private field underlying the property. The same criticism of the ParentGraph property made regarding graph nodes apply head well. It may be prudent to replace the public property with a unique ID. The rest of the properties are all fine.

#### Methods Review
While AbstractEdges can not be instantiated they have the following constructors for inheritance.
- public AbstractEdge(int _sourceID, int _sinkID, [float weight = 1])
- public AbstractEdge(GraphNode _sourceNode, GraphNode _sinkNode, [float weight = 1])
- public AbstractEdge(AbstractEdge _edge)

It would potentially prudent to combine the first two constructors into one for the sake of clarity and consistency. If nodes establish the standard that upon instantiating a low lever component its parent is null until it its added to a graph we should follow that standard for abstract edges as well. The primary reason for the constructor that uses references to existing nodes is to assign the parent graph of the those nodes to be the parent graph of the edge as well. Aside from the issues with consistency this is fine, but i still think those issues warrant its removal. Aside from assigning a non-null value to parentGraph this constructor is equivalent to the constructor that takes integer ID's as Edges do not actually directly reference their Source and Sink nodes anywhere.

The final constructor is the copy constructor. I will note there is some redundancy between this constructor and the abstract method 
-public AbstractEdge Copy(AbstractEdge _edge)

the reason for this was a problem when copying an edge whose type i didn't necessarily know (implement an AbstractGraph copy constructor rather than copy constructors for both Directed and Undirected graphs). The solution was to implement Copy as an abstract method of Edge. In retrospect there was an alternative solution using Activator.CreateInstance(_edge.GetType(), {constructor parameters}). I will discus this more in the review of the AbstractGraph Copy constructor.

They contain the following Methods for accessing data from their parent graph
- public GraphNode GetSourceNode()
- public GraphNode GetSinkNode()

which are  fine aside from the fact they again neglect error control for null parents

They also contain the abstract method 
- public abstract GraphNode GetOppositeNode(_node)

Which is the primary point of distinction between Directed and Undirected Edges, which will be discussed more in their respective reviews.

The methods for modifying an edge are:

- public void SetParent(AbstractGraph)
- public void TrySwapNodes()

Currently, SetParent prevents you from changing the parent of an edge unless the parent is currently null and warns you to orphan in edge first. This is acceptable as error control against graphs contain components that don't have it set as their parent.

The TrySwapNodes method has a somewhat misleading name as it currently never actually returns false / fails to swap; however it is possible these will come up in the future. I would rather use the "try" naming semantic for all modifications to graphs in case future subclasses of graphs need to prevent certain modifications. Also it should already have the null parent fail case, which it does not. THis needs to be added.

### DirectedEdge : AbstractEdge

DirectedEdge's don't contain any fields not inherited from AbstractEdge. They also don't implement any new methods. All the constructors simply inherit the Abstract constructors with no additional code. They do implement the following abstract methods from AbstractEdge
- public override AbstractEdge Copy()
- public GraphNode GetOppositeNode()

copy simply calls the appropriate copy construct with this as an argument. GetOppositeNode in this case will always return the SinkNode. This will allow the sink node to identify an incoming edge will also knowing it cant actually travel down it (as the opposite node from it is itself).

This behavior works to distinguish directed an undirected edge however it is somewhat unclear. As the same class of object is returned in the "fail" case as the method successfully completing it might be difficult to notice what happened and that the node this method is returning isn't actual the node opposite its argument. 

The method should be refactored so that it returns null when asking for the node opposite the sink node of a directed edge. Perhaps it should also be renamed GetConnectedNode to make it even more clear it will only return the opposite node if it is accessible. 
### UndirectedEdge : AbstractEdge

Again UndirectedEdge predominantly just inherit the fields and methods of AbstractEdges, with the only new code being the implementations of its abstract methods.

The implementation of Copy() is the same as in DirectedEdge - aside from invoking the copy constructor of UndirectedEdge instead. the implementation of GetOppositeNode(GraphNode node) is much simpler here than  DirectedEdge. Here it does exactly what the name would imply and returns the sourceNode is the sinkNode is passed in and the sourceNode for the sinkNode.

For both of these classes however we do need to add null parent error control.

## High Level Components

The high level components of this library are the graphs themselves. A graph is defined by a list of edges and a list of nodes (Stored internally as dictionaries with integer keys for node and string keys for edges (the two nodes they connect separated by a comma).

### AbstractGraph
This is the parent class of all graphs. Any logic that all graphs share should go here. Functions related to adding edges should be abstract to ensure inherited classes implement the appropriate type of edges.

#### Fields Review

this high level component contains one public property

- public int Size {get => nodes.Keys.Count}

and two private fields
- protected Dictionary<int, GraphNode> n
odes
- protected Dictionary<string, AbstractEdge> edges

these are all fine. As discussed in the low level component review it might be prudent for graphs to have a unique id, automatically generated upon instantiation.

#### Methods Review

There are currently three constructors for graphs.

- public AbstractGraph()
- public AbstractGraph(Dictionary<int List<int>> adjList)
- public AbstractGraph(int V, List<int[]> E)

The first constructor is the empty graph constructor, the second constructs from an adjacency list, the third from a number of nodes and list of edges (nodes are presumed to have id's ranging from 0 to V-1). There is also a planed constructor from an adjacency matrix however that is currently not implemented. AbstractGraph's do not have a true copy constructor implemented but instead an abstract method
- public AbstractGraph Copy()

The reason for this was the need to know what kind of edges to instantiate when creating a copy of a graph, however since we have a reference to the object we want to copy it would be possible to replace this with an actual constructor that uses Activator.CreateInstance to create instances of edges based on the type of edges in the graph passed in as an argument to the constructor. This would allow use to implement all copying logic at the highest level of abstraction for Graphs.

All constructors also make us of the method

- protected abstract void InitializeEdges(list<int[]> edgeList)

as while initializing the edge of a graph they might not satisfy the restrictions some subclasses use in TryAddEdge(), which presumes the graph is fully constructed. This function is abstract as the child classes must initialize the appropriate type of edges. 

Other abstract functions relating to edges are
- public abstract bool TryAddEdge(GraphNode sourceNode, GraphNode sinkNode)
- public abstract bool TryAddEdge(AbstractEdge edgeToAdd)

They also have the virtual method 
-public virtual bool TryAddEdge(int id1, int id2)

however this method just calls TryAddEdge(graphNode sourceNode, graphNode sinkNode) with the appropriate node after verifying they are in the graph.

One standard in these methods that may be worth reconsidering is that if you attempt to add an edge to a node that doesn't exist it will be created and added to the graph. This is perhaps unintuitive and it would be better to return false in those circumstances. An argument for way it might be better to leave this as is would be that in Tree's adding a node by itself shouldn't be allowed, it would break the tree condition; however adding a new edge to a new node should be. It also might be prudent to remove the method to add an edge by ID and establish the standard that all components are added to graphs by reference.

Other methods for modifying the graph include 
- public virtual bool TryReplaceEdge(AbstractEdge oldEdge, AbstractEdge newEdge)
- public void RemoveEdge(AbstractEdge edge)
- public void AddNode(int nodeID)
- public void AddNode(GraphNode node)
- public void RemoveNode(GraphNode node)

The most involved method of these is TryReplaceEdge - which was added as a tree should not allow an edge to be removed without a new one being added to re-satisfy the tree condition.

The first change i recommend for these methods is to unify them under the TryBlankBlank naming semantic. This will make it more clear that if the operation would break the rules of a graph subclass it wont be allowed. They should all also return a bool to identify if the operation was allowed or not. Beyond that it would make sense to have methods to remove nodes/edges by id however this is a low priority.

Abstract graphs also implement the flowing operator overloads.

- public static AbstractGraph operator +(AbstractGraph a, GraphNode b)
- public static AbstractGraph operator -(AbstractGraph a, GraphNode b)
- public static AbstractGraph operator +(AbstractGraph a, AbstractEdge b)
- public static AbstractGraph operator -(AbstractGraph a, AbstractEdge b)

the behavior of these operators is straightforwardly what you would expect. The return of the operators is a new graph instance so modifications to it have no effect on the original graph a. A potential issue with these operators is with Tree's certain operations would be forbidden. We could alleviate this by up-casting the copy of the Tree a to a undirected graph which will be more permissive with its allowed operations. These operators have also yet to prove themselves useful so if needed they would likely be easy to remove.

Next are the field access methods for nodes
- public GraphNode GetNode(int id)
- public GraphNode GetRandomNode()
- public List<GraphNode> GetAllNodes()
- public List<int> GetAllNodeIDs()
- public bool HasNode(GraphNode node)
  
and for edges

- public AbstractEdge GetEdge(int sourceID, int sinkID)
- public AbstractEdge GetEdge(string ID)
- public List<AbstractEdge> GetAllEdges()
- public List<string> GetAllEdgesIDs()
- public List<AbstractEdge> GetEdgeList()
 
Again these functions all straightforwardly do what you would expect. Most of them are only a line or two long. My only suggestion regarding them would be to unify the types of methods available for edges and nodes by adding GetNodeList, HasEdge, and GetRandomEdge methods.

Lastly we have a few older methods implemented before the start of fulltime development that have been of limited use. 

- public List<GraphNode> DFS(int startID)
- public List<GraphNode> DFS(int startNode)
- public List<GraphNode> BFS(int startID)
- public List<GraphNode> BFS(int startNode)

the functions that take integer id's as parameters are QoL functions that simply call the actual search method using the node with the given ID. Other old functions are 

- public GetConnectedComponents()
- public bool HasPath(int node1ID, int node2ID)
- public bool HasPath(GraphNode node1, GraphNode node2)
- public VisitNode(int id, List<int> visitedIDs)

the search functions have been of limited use al typically there is algorithm specific work that needs to be done at each stage of the search making this generic version unsuitable for general use. It is useful in the HasPath functions however. The VisitNode function hasn't been used frequently but is a nice QoL function. Overall these methods are unlikely to see extensive use going forward, but I still recommend keeping them for their occasional convenience in testing.  

GetConnectedComponents on the other hand has never been used outside of testing and is of questionable efficiency, so i recommend it be removed.
  
### DirectedGraph : AbstractGraph

DirectedGraph does not add any additional fields to AbstractGraph. It also does not add any additional code to its constructors. The only distinctions between it and its parent class are its implementation of the abstract methods, which are again

- public override AbstractGraph Copy()
- protected override void InitializeEdges(List<int[]> edgeList)
- public override bool TryAddEdge(GraphNode v1, GraphNode v2)
- public override bool TryAddEdge(AbstractEdge edge)

As stated above it is likely possible to implement Copy as a base class level constructor using Activator.CreateInstance() to deduce the type of edge to instantiate from the argument / original graph. This is a low priority change as the code for the copy constructor has already been written so the work that could have been avoided by using a base level constructor has already been done.

Another possible change is to make the argument for InitializeEdges a Vector2Int list to ensure it is the appropriate size. There are a few reasons I would argue not to do this. Firstly - Vector2Int is a unity class and while this library is intended for use in unity if it is possible to avoid directly Unity class such that it could be used outside unity as well I see no reason not too. Currently the only unity feature we are using is unity debug warning messages, which could just be replace by system debug messages in a non-unity port. Secondly, Vector2Int are a reference type and i would rather use a value type for a something like this.

The two TryAddEdge methods are both fine. They have appropriate error controlling and do what they are supposed to; however, there are knock on changes that will be caused by some changes to edges. In particular for the method that takes to nodes as an argument. Since the ability to assign a parent graph in the constructors of low level components will be removed, the parent must be assigned with the TryAddEdge method (previously the edge would deduce is parent from the parent of the nodes it connects in this constructor). In fact - for clarity sake it might be prudent to remove this method all together. While convenient it might be better to establish that components are always added by a reference to a component with a null parent.

### UndirectedGraph : AbstractGraph

I have no comments to make for this class that i didn't already make on UndirectedGraph, the only distinction between these two classes is what kinds of edges they instantiate.

### Tree : UndirectedGraph

Again Trees do not add any fields to their parent class. They do add additional logic to certain methods however

The rule that all trees use to determine if an operation should be allowed is the "Tree Condition." There are multiple equivalent ways to state this condition however for out purposes we state it as a tree is a graph that is both
- fully connected (the reason we make tree a subtype of undirected graph)
- acyclic

Tree's add a new method to verify this
- public static bool VerifyTree(AbstractGraph graph)

which uses the CycleSolver's find cycle method to find if there is a cycle accessible from any node on the graph, then makes sure every other node in the graph is accessible from that node as well. We only need to check for a cycle once since if there is a cycle disconnected to the first node we test it is by definition not a tree.

To ensure the tree condition is always satisfied there are differences in the when the TryAddEdge methods will allow you to add an edge. I will not list out the method signatures for a third time, but the distinctions are that a tree will not let you add an edge that:
- Connects two node already in the graph (this would be a cycle)
- adds two new nodes to a graph (this would form a disconnected components)
  
or more succinctly for an edge to be added to a tree one node of the edge must be included in the graph and one must be a new node.

when replacing an edge on the other hand the requirements are more nuanced. removing an edge from a tree always splits it into two connected components. For the new edge added to re-satisfy the tree condition one node must be from one of these components, the the other from the other. Currently the class uses a stop gap solution of always letting you add the edge then throwing an exception if VerifyTree returns false on the new graph. This needs to be changed in the future as this behavior is inconsistent with "try" methods returning false if you attempt to do a forbidden operation.

Currently trees do not prevent adding new nodes even though this breaks the tree condition as the new node would be disconnected from the rest of the tree. This will be rectified in the broader refactor to make the methods for adding nodes consistent with the methods for adding edges. 

### RootedTree : Tree
A RootedTree is a tree that also satisfies the property that every node exactly one inEdge except for one, the root, which has zero. 
A RootedTree does have one additional field not included in its parent. The property
- public GraphNode RootNode { get  {...}}
  
which calculate the root by picking a random node and moving up the graph until it reaches a node with no parent. This is not a particularly efficient method of calculating this as it takes linear time in the worst case (when the tree is equivalent to a linked list). Solutions to this would be an algorithm that finds the root more efficiently or an to store a reference to the root node and only run this function when that reference needs to be updates (this is how the heap class handles this). At the very least it should be made a method to make it clear that invoking it is a potentially significant time complexity consideration

As for the methods of RootedTree again none of the Constructors have any additional code. The TryAddEdge methods now require the specifically the source node be the node already in the graph and the sink node be the new node. 


Methods new to this class are
- public GraphNode GetParentNode(GraphNode node)
- public List<GraphNode> GetChildren(GraphNode node)
- public List<GraphNode> GetLayer(int k)
  
The GetParent and GetChildren methods straightforwardly return the appropriate nodes connected to the arguments. GetLayer returns a list of all nodes k edges away from the root. The time complexity of this is O(# of node below layer k)

## Data structures

These are data structures that don't inherit from AbstractGraph but use them in there underlying implementation nonetheless. Note that of many of these it would be more pragmatic to implement them in a dependency free manner however they have proved useful as a method of testing the library while also creating classes to enable future functionality.


### D-ary Heap
This implementation of a D-ary heap isn't space efficient as one could be implemented with a simple array; however it has proved useful as a use case to test the functionality of the RootedTree class. This structure is needed for efficient implementation of Dijkstra's shortest path algorithm as well as Prim's MST algorithm.  
#### Fields
the heap class contains the following private fields
- private GraphNode rootNode
- private int childCapacity
- private RootedTree heapTree
- private Dictionary<THeapType, int> objectIDs
- private Dictionary<int, THeapType> reverseObjIDs
- private int IDcounter

And the following public properties
- public int Size {get => heapTree.Size}
- public bool isEmpty {get => Size == 0}
  
The biggest concern for these fields is the use of two different dictionaries to keep track of the ID of each objects node in the heap tree. This is needed because we need to preform these look ups both ways. To get the id for each objects node in the graph, and when we pop a node to get the object we should return. Using two dictionaries to do this is the more time efficient but less space efficient solution to this. 

the rootNode field also needs to be updated whenever the root is changed in the tree. This is done to avoid running the code to find the root of a RootedTree every time we need to reference to root. If RootedTree is modified to update its root automatically when needed this field could be removed and the heapTree.RootNode field could be used instead. 

#### Methods

DaryHeap's currently have one constructor
- public D_aryHeap(int D)
  
It would be prudent to design a way to prevent the child capacity of the heap from being modified after its instantiation. It would also be useful to implement constructors that initialize the heap with some entries. This could be done using a dictionary of THeapType objects and their keys as an additional parameter.

The public methods for interacting with the heap are
- public THeapType Peek()
- public void Push()
- public bool TryPop(out THeapType outObject)
- public void IncreaseKey(THeapType obj, float newValue)
- public void DecreaseKey(THeapType obj, float newValue)

there is also a planned but currently unimplemented  method TryPopThenPush. This would be beneficial because it eliminates the need to sort the heap both when popping the top node and when pushing a new node. Instead we could replace to root with the new node then resort the heap a single time.

Only minor changes are necessary here. First of all, the Peek method should control for the scenario where the heap is empty. I would also recommend waiting to instantiate the heapTree until at least one node has been added to it. When the methods for adding nodes are refactored the situation where the tree is empty will be considered when determining if TryAddNode will allow the operation to be done; however it might be more clear if the tree is instantiated when its first node is added. An argument against this is it might cause null reference errors before a object is added to the heap.

private/internal methods for modifying the heap are
- private void DeleteRoot()
- private void DeleteElement(THeapType obj)
- private void SiftUp(THeapType obj)
- private void SiftDown(THeapType obj)

A problem with these is that DeleteElement doesn't preform all the needed work to be done in that case that the removed element was the root. Specifically it does not update the root after removing a node (this is only needed when the root itself is removed). This is because the root is intended to be removed through the DeleteRoot method; however, DeleteElement cannot prevent itself from being called with the root as its argument as DeleteRoot still uses it for actually removing the node itself, and just preforms the additional work to set the new root. 

A potential solution would be to remove the DeleteRoot method and if the argument of DeleteElement is the current root simply store is smallest child to set as the new root after the old is removed. The sift methods are both fine.

Lastly we have a few methods for accessing certain nodes in heap.
- private GraphNode GetHeapNode(THeapType obj)
- private GraphNode GetBottomNode()
- private GraphNode GetGreatestChild(GraphNode node)
- private GraphNode GetSmallestChild(GraphNode node)

The only of these worth commenting it on is GetBottomNode as the others are either straight forward look ups or list sorting. GetBottomNode uses some math and the fact a heap is an "almost complete" D-ary tree to improve its efficiency by only searching the FloorToInt(log_d(N)-1) layer. This is obviously more efficient then searching the whole tree, however if the deduction it is based on is wrong it would cause errors (I don't believe this is the case though) 

## Algorithms

These are static classes to solve graph theory problems such as "what is the shortest path between these two nodes" or "does this graph contain cycles or any strongly connected components

### TarjanSCCSolver
This was originally a full class however after its initial implementation the decision was made to make algorithms static classes as needing to instantiate the class to use the algorithm is unintuitive. It is possible this post-implementation change may cause errors however. 
#### Fields
The TarjanSCCSolver class has the following private global fields

- private static Dictionary<GraphNode, bool> onStack
- private static Dictionary<GraphNode, int> TarjanID's
- private static int[] lowlink
- private static Dictionary<GraphNode, int> finalLowLinks

because this is a static class we need to be sure to set these back to their initial values when we start solving a new graph. In fact, it may be prudent to make these fields local to the algorithms solve method.

#### Methods

TarjanSCCSolver has two public methods available for general use and one private method for internal use.

- public static bool CheckDAG(DirectedGraph graph)
- public static List<List<GraphNode>> Solve(DirectedGraph graph)
- private static void TarjanDFS(...)
  
The parameters for the DFS function are 
- List<List<GraphNode>> sccList
- GraphNode node
- int iterationDepth
- out int outDepth

CheckDAG just calls the solve function then checks it its return, the list of all strongly connected components, is empty. The majority of the work for calculating the strongly connected components in the graph occurs in the DFS function, with the Solve function initializing values passing them into the recursive search function. As this is a well known algorithm the are little changes that need to be made to these methods.
### CycleSolver
This static class is intended to find cycles in undirected graphs for which Tarjan is unsuited, or when it is not needed to find all cycles in a graph but simply if any exist. This class contains two static methods (actually one method with one overload)

- public static bool FindCycleFrom(GraphNode CurrentNode, [List<AbstractEdge> visitedEdges = null], [List<GraphNode> visitedNodes = null])
- public static bool FindCycleFrom(GraphNode CurrentNode, out List<GraphNode> touchedNodes [List<AbstractEdge> visitedEdges = null], [List<GraphNode> visitedNodes = null])

With the first simply wrapping the second for situations where only the cycles existence is needed, not what nodes where touched while finding the cycle. The second function is recursive, but it should not be possible for the function to enter an infinite loop. As this is a straightforward method I have no changes to recommend to it. This class is mainly a place to put future functionality.   
### Topological Sort

### ShortestPath

## Exceptions

During earlier development I was overly eager to throw exceptions during error/input handling. As such I anticipate many of the following exceptions will be candidates for removal. As the isn't much to say about these exceptions I will simply say wether I think they should be removed or not.

- NotInGraphException: Remove
- NotAttachedToEdgeException: Keep
- IncompatibleEdgeException: Keep
- EmptyGraphException: Remove
- DifferentGraphException: Keep
- NotATreeException: Keep
- NonUniqueIDException: Remove
- OrphanException: Keep
- NotADAGException: Keep
  
In general when the code prevents something from happening it should do so by displaying a warning not throwing an exception. Exceptions should be saved for situations where it appears something has already gone wrong previously (e.g. an Orphan node in a situation where the node should already have a parent. A Tree that doesn't satisfy the tree condition) or situations where there is likely user error that needs to be resolved (passing a node not attached to a tree object into its GetChildren method).  Try... functions should very rarely if ever throw exceptions.  

## Tests

These tests are for the moment fairly non-adversarial intended to verify the code is preforming as intended during standard use rather than test specifically designed to attempt to cause failure.

### GraphTests

### TreeTests

### OpperatorTests

### HeapTests

### CylcesTests

### ShortestPathTests
