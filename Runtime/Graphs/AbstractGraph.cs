using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs{
    public abstract class AbstractGraph<TGraphType> { //? the default type of graph is directed and unweighted
// ! Members -----
// * Value Types - Public
        public int Size { get => nodes.Keys.Count;}
// * Value Types - Private

// * Reference Types - Public

// * Reference Types -  Private
        protected Dictionary<int , GraphNode<TGraphType>> nodes = new Dictionary<int, GraphNode<TGraphType>>();
        protected Dictionary<string, AbstractEdge<TGraphType>> edges = new Dictionary<string, AbstractEdge<TGraphType>>();

// ! Methods -----
// * Member Accessors

        public GraphNode<TGraphType> GetNode(int ID) {
            if(!nodes.ContainsKey(ID)) {
                Debug.LogWarning($"node {ID} not found in graph");
                return null;
            }
            return nodes[ID];
        }
        public GraphNode<TGraphType> GetRandomNode() {
            var allNodes = GetAllNodes();
            var random = new System.Random();
            int index  = random.Next(Size);
            return allNodes[index]; 
        }
        public List<GraphNode<TGraphType>> GetAllNodes() {
            return nodes.Values.ToList();
        }
        public List<int> GetAllNodeIDs() {
            return nodes.Keys.ToList(); //? this should be a new object? 
        }
        public bool HasNode(GraphNode<TGraphType> node) {
            return GetAllNodes().Contains(node); 
        }
        public AbstractEdge<TGraphType> GetEdge(string ID) {
            if(!edges.ContainsKey(ID)) {
                Debug.LogWarning($"edge {ID} not found in graph");
                return null;
            }
            return edges[ID];
        }
        public List<AbstractEdge<TGraphType>> GetAllEdges() {
            return edges.Values.ToList();
        }
        public List<string> GetAllEdgeIDs() {
            return edges.Keys.ToList(); //? again, should be a new object with copied values
        }
        public List<AbstractEdge<TGraphType>> GetEdgeList(List<string> IDs) {
            List<AbstractEdge<TGraphType>> output = new List<AbstractEdge<TGraphType>>();
            foreach(var ID in IDs) {output.Add(edges[ID]);}
            return output;
        }
// * Constructors
        // ? Empty Graph Constructor
        public AbstractGraph() {
        }
        // ? adjacency list constructor
        public AbstractGraph(Dictionary<int,List<int>> adjList) { //? O(V+E) time
            foreach (int id in adjList.Keys) { 
                AddNode(id);
            }
            List<int[]> edgeList = new List<int[]>();
            
            foreach (int id in nodes.Keys) {
                foreach (int adjID in adjList[id]) {
                    edgeList.Add(new int[2] {id,adjID});
                }
            }
            InitializeEdges(edgeList);
            DebugMsg();
        }
        // ? list of edges and size constructor
        public AbstractGraph(int V, List<int[]> E) { //? O(V+E) time
            for (int id = 0; id < V; id++) {
                AddNode(new GraphNode<TGraphType>(id, this));
            }
            InitializeEdges(E);
        }
        // TODO adjacency matrix constructor
        public AbstractGraph(int[][] adjMatrix) {
            throw new NotImplementedException();
        }
        //? copy "constructor"
        public abstract AbstractGraph<TGraphType> Copy();

// * Modification Methods
    // * abstract edge methods
        public virtual bool TryAddEdge(int id1, int id2) { 
            if(!nodes.ContainsKey(id1)) {
                Debug.LogWarning($"A new node with id {id1} had to be created to add this edge");
                AddNode(id1);
            }
            if(!nodes.ContainsKey(id2)) {
                Debug.LogWarning($"A new node with id {id2} had to be created to add this edge");
                AddNode(id2);
            }
            return TryAddEdge(GetNode(id1), GetNode(id2));
        }
        public abstract bool TryAddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2);
        
        protected abstract bool TryAddEdge(AbstractEdge<TGraphType> edgeToAdd);
        protected abstract void InitializeEdges(List<int[]> edgeList);
        
        public void RemoveEdge(AbstractEdge<TGraphType> edge) {
            if(edge.ParentGraph != this) {
                Debug.LogWarning("you are trying to remove and edge from a graph that isn't its parent");
                return;
            }
            GetNode(edge.SourceNodeID).RemoveEdge(edge);
            GetNode(edge.SinkNodeID).RemoveEdge(edge);
            edges.Remove(edge.ID);
            edge = null;
            return;
        }
        protected void AddNode(int nodeID) {
            nodes.Add(nodeID, new GraphNode<TGraphType>(nodeID,this));
        }
        public void AddNode(GraphNode<TGraphType> nodeToAdd) {
            if(nodes.ContainsKey(nodeToAdd.ID)) throw new NonUniqueIDException(nodeToAdd.ID);
            if(nodeToAdd.ParentGraph != null) {
                Debug.LogWarning("This Node is already attached to a graph, it must be removed from its parent before it can be added to another graph");
                return;
            }
            nodeToAdd.ClearEdges();
            // ? old code before i decide it would be better to just clear out a nodes edges when adding it to a new graph
            // foreach (var edgeID in nodeToAdd.edgeIDs) {
            //         var edgeNodeIDs = edgeID.Split(",",2);
            //         if(nodes.ContainsKey(Int32.Parse(edgeNodeIDs[1]))) {
            //             AddEdge(Int32.Parse(edgeNodeIDs[0]),Int32.Parse(edgeNodeIDs[1]));
            //         } else {
            //             nodeToAdd.RemoveEdgeID(edgeID);
            //     } }
            nodeToAdd.SetParent(this);
            nodes.Add(nodeToAdd.ID,nodeToAdd);
        }
        public void RemoveNode(GraphNode<TGraphType> node) {
            if(!(node.ParentGraph == this)) return;
            foreach (var edgeID in node.EdgeIDs) {
                RemoveEdge(GetEdge(edgeID));
            }
            nodes.Remove(node.ID);
            node = null;
        }
// * Operator Overloads
        public static AbstractGraph<TGraphType> operator +(AbstractGraph<TGraphType> a,GraphNode<TGraphType> b) {
            AbstractGraph<TGraphType> output = a.Copy();
            GraphNode<TGraphType> bCopy = new GraphNode<TGraphType>(b); 
            output.AddNode(bCopy);
            return output;
        }
        public static AbstractGraph<TGraphType> operator -(AbstractGraph<TGraphType> a,GraphNode<TGraphType> b) {
            if(b.ParentGraph != a) {
                Debug.LogWarning("You cannot subtract a node from a graph that is not its parent, returning null");
                return null;
            }
            AbstractGraph<TGraphType> output = a.Copy();
            if(!a.HasNode(b)) return output;
            output.RemoveNode(output.GetNode(b.ID));
            return output;
        }
        public static AbstractGraph<TGraphType> operator +(AbstractGraph<TGraphType> a,AbstractEdge<TGraphType> b) {
            AbstractGraph<TGraphType> output = a.Copy();
            AbstractEdge<TGraphType> bCopy = b.Copy();
            output.TryAddEdge(bCopy);
            return output;
        }
        public static AbstractGraph<TGraphType> operator -(AbstractGraph<TGraphType> a,AbstractEdge<TGraphType> b) {
            if(b.ParentGraph != a) {
                Debug.LogWarning("You cannot subtract an edge from a graph that is not its parent, returning null");
                return null;
            }
            AbstractGraph<TGraphType> output = a.Copy();
            output.RemoveEdge(output.GetEdge(b.ID));
            return output;
        }
// * Searches
    // * Depth First Search
        public List<GraphNode<TGraphType>> DFS(int nodeID) {
            return DFS(GetNode(nodeID));
        }
        public List<GraphNode<TGraphType>> DFS(GraphNode<TGraphType> node) {
            List<GraphNode<TGraphType>> connectedNodes = new List<GraphNode<TGraphType>>{node};
            List<int> visitedIDs = new List<int>{node.ID};
            Stack<int> idsToVisit = new Stack<int>(node.NeighborIDs);
            while (idsToVisit.TryPop(out int nextID)) {        
                if(VisitNode(nextID,visitedIDs)) {
                    foreach (int id in GetNode(nextID).NeighborIDs) {
                        idsToVisit.Push(id);
                    }
                    connectedNodes.Add(GetNode(nextID));
                } else {
                    //? incase I need to do anything when reaching a visited node in the future
                } 
            }
            return connectedNodes;
        }
    // * Breath First Search
        public List<GraphNode<TGraphType>> BFS(int nodeID)
        {
            return BFS(GetNode(nodeID));
        }

        public List<GraphNode<TGraphType>> BFS(GraphNode<TGraphType> node)
        {
            List<GraphNode<TGraphType>> connectedNodes = new List<GraphNode<TGraphType>>{node};
            List<int> visitedIDs = new List<int>{node.ID};
            Queue<int> idsToVisit = new Queue<int>(node.NeighborIDs);
            while (idsToVisit.TryDequeue(out int nextID)) {        
                if(VisitNode(nextID,visitedIDs)) {
                    foreach (int id in GetNode(nextID).NeighborIDs) {
                        idsToVisit.Enqueue(id);
                    }
                    connectedNodes.Add(GetNode(nextID));
                } else {
                    //? incase I need to do anything when reaching a visited node in the future
                } 
            }
            return connectedNodes;
        }
        // * Get Connected component sets
        // ? a potentially useful observation is in an undirected graph connectedness forms an equivalence relation
        public List<List<GraphNode<TGraphType>>> GetConnectedComponents(){ //? this type name is rather cumbersome
            List<List<GraphNode<TGraphType>>> connectedComponents = new List<List<GraphNode<TGraphType>>>();
            Stack<int> idStack = new Stack<int>();
            foreach (int id in nodes.Keys) {
                idStack.Push(id);
            }
            while(idStack.TryPop(out int nextID)) {
                bool inComponent = false;
                if(connectedComponents == new List<List<GraphNode<TGraphType>>>()) { //? if this was the first node in the stack just search
                    connectedComponents.Add(DFS(nextID)); //?either search works
                    continue;
                }
                foreach (var _class in connectedComponents) { //? if this node is in one of the components we have already searched move on to the next
                    if(_class.Contains(GetNode(nextID))) 
                        {inComponent = true;}
                }
                if(!inComponent) connectedComponents.Add(DFS(nextID)); //? if it isn't in any of them add a new one by searching from it
            }
            return connectedComponents;
        }
        public bool HasPath(int node1ID, int node2ID){ //? this is identical to asking if two nodes are in the same equivalence class
            return HasPath(GetNode(node1ID), GetNode(node2ID));
        }
        public bool HasPath(GraphNode<TGraphType> node1, GraphNode<TGraphType> node2) {
            return BFS(node1).Contains(node2); 
            // ? this could be optimized by rewriting the search code to terminate when the destination node is reached 
            // ? but thats still O(v+e) time so i don't really care to until it becomes a problem
        }
        private bool VisitNode(int id, List<int> visitedIDs) { //? may be useful for future functionality
            if(visitedIDs.Contains(id)) return false;
            else {
                visitedIDs.Add(id);
                return true;
            } 
        }
        protected void DebugMsg() {
            Debug.Log($"this graph has nodes {string.Join("|", nodes.Keys.ToList())}");
            Debug.Log($"this graph has edges {string.Join("|",edges.Keys.ToList())}");
        }
    }
}
