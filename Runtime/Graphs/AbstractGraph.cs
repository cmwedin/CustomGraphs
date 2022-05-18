using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs{
    public abstract class AbstractGraph<TGraphType> { //? the default type of graph is directed and unweighted
        protected Dictionary<int , GraphNode<TGraphType>> nodes = new Dictionary<int, GraphNode<TGraphType>>();
        public Dictionary<int, GraphNode<TGraphType>> Nodes { get => nodes;}

        public int Size { get => Nodes.Keys.Count;}
        protected Dictionary<string, GraphEdge<TGraphType>> edges = new Dictionary<string, GraphEdge<TGraphType>>();


        // * Constructors
        public AbstractGraph(Dictionary<int,List<int>> adjacencyList) { //? O(V+E) time
            foreach (int id in adjacencyList.Keys) { 
                AddNode(new GraphNode<TGraphType>(id, this));
            }
            foreach (int id in Nodes.Keys) {
                foreach (int adjID in adjacencyList[id]) {
                    AddEdge(id,adjID);
                }
            }
        }

        public AbstractGraph(int V, List<int[]> E) { //? O(V+E) time
            for (int id = 0; id < V; id++) {
                AddNode(new GraphNode<TGraphType>(id, this));
            }
            foreach (var edge in E) {
                if(edge.Length != 2) throw new Exception("each edges array length must be exactly 2");
                this.AddEdge(edge[0],edge[1]);
            }
        }

        // * Operator Overloads
        // ? the approach I am using here is space inefficient and creates unneeded copies of nodes in memory
            //? if we add a non-new node to a graph the Copy method will created new copies of its edges in memory
                //? these edges will in turn create new copies of their source and sink node (that haven't already been copied)
                //? this in turn causes copying a non-new node to create a copy of its entire connected component
            //? if the node is new we still create a new copy of it as there if it is or not
                //? however in this case we only create one redundant node as a new node won't have any attached edges.
        // ? this should be resolved in garbage collection as none of the redundant info created in memory should be used anywhere after we clear the copy nodes edges.
            //? but its possible I'm misunderstanding how garbage collection works and the redundant objects referencing each other will prevent them from being marked for collection
        //TODO regardless this seems like something that should be resolved in the future, probably bad practice representing nodes in such a way that causes this
        public static AbstractGraph<TGraphType> operator +(AbstractGraph<TGraphType> a,GraphNode<TGraphType> b) {
            AbstractGraph<TGraphType> output = ObjectExtensions.Copy(a);
            GraphNode<TGraphType> bCopy = ObjectExtensions.Copy(b); 
            output.AddNode(bCopy);
            return output;
        }
        public static AbstractGraph<TGraphType> operator -(AbstractGraph<TGraphType> a,GraphNode<TGraphType> b) {
            AbstractGraph<TGraphType> output = ObjectExtensions.Copy(a);
            return output;
        }
        public static AbstractGraph<TGraphType> operator +(AbstractGraph<TGraphType> a,GraphEdge<TGraphType> b) {
            AbstractGraph<TGraphType> output = ObjectExtensions.Copy(a);
            return output;
        }
        public static AbstractGraph<TGraphType> operator -(AbstractGraph<TGraphType> a,GraphEdge<TGraphType> b) {
            AbstractGraph<TGraphType> output = ObjectExtensions.Copy(a);
            return output;
        }
        


        private void AddNode(GraphNode<TGraphType> node) {
            if(Nodes.ContainsKey(node.ID)) throw new NonUniqueIDException(node.ID);
            nodes.Add(node.ID,node);
        }
        public void RemoveNode(GraphNode<TGraphType> node) {
            if(!HasNode(node)) return;
            List<GraphEdge<TGraphType>> edgesToRemove = new List<GraphEdge<TGraphType>>();
            foreach (var edge in node.GetInEdges()) {
                if(!edgesToRemove.Contains(edge)) edgesToRemove.Add(edge);
            }
            foreach (var edge in node.GetOutEdges()) {
                if(!edgesToRemove.Contains(edge)) edgesToRemove.Add(edge);
            }
            foreach (var edge in edgesToRemove) {
                RemoveEdge(edge);
            }
            Nodes.Remove(node.ID);
        }       
        public bool HasNode(GraphNode<TGraphType> node) {
            return Nodes.ContainsValue(node); 
        }
        public void AddEdge(int id1, int id2) {
            AddEdge(Nodes[id1],Nodes[id2]);
        }

        public virtual void AddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            if(!this.HasNode(v1)) throw new NotInGraphException(v1.ID);
            if(!this.HasNode(v2)) throw new NotInGraphException(v2.ID);
            //? subclasses override this and add the edge based on wether or not it should be undirected
        }
        public GraphEdge<TGraphType> GetEdge(string ID) {
            return edges[ID];
        }
        public List<GraphEdge<TGraphType>> GetEdgeList(List<string> IDs) {
            List<GraphEdge<TGraphType>> output = new List<GraphEdge<TGraphType>>();
            foreach(var ID in IDs) {output.Add(edges[ID]);}
            return output;
        }

        public void RemoveEdge(GraphEdge<TGraphType> edge) {
            if(!edges.ContainsValue(edge)) return;
            edges.Remove(edge.ID);
            edge.GetSourceNode().RemoveEdge(edge);
            edge.GetSinkNode().RemoveEdge(edge);
            return;
        }
 
        
        // * Depth First Search
        public List<GraphNode<TGraphType>> DFS(int nodeID) {
            return DFS(Nodes[nodeID]);
        }
        public List<GraphNode<TGraphType>> DFS(GraphNode<TGraphType> node) {
            List<GraphNode<TGraphType>> connectedNodes = new List<GraphNode<TGraphType>>{node};
            List<int> visitedIDs = new List<int>{node.ID};
            Stack<int> idsToVisit = new Stack<int>(node.NeighborIDs);
            while (idsToVisit.TryPop(out int nextID)) {        
                if(VisitNode(nextID,visitedIDs)) {
                    foreach (int id in Nodes[nextID].NeighborIDs) {
                        idsToVisit.Push(id);
                    }
                    connectedNodes.Add(Nodes[nextID]);
                } else {
                    //? incase I need to do anything when reaching a visited node in the future
                } 
            }
            return connectedNodes;
        }

        // * Breath First Search
        public List<GraphNode<TGraphType>> BFS(int nodeID)
        {
            return BFS(Nodes[nodeID]);
        }

        public List<GraphNode<TGraphType>> BFS(GraphNode<TGraphType> node)
        {
            List<GraphNode<TGraphType>> connectedNodes = new List<GraphNode<TGraphType>>{node};
            List<int> visitedIDs = new List<int>{node.ID};
            Queue<int> idsToVisit = new Queue<int>(node.NeighborIDs);
            while (idsToVisit.TryDequeue(out int nextID)) {        
                if(VisitNode(nextID,visitedIDs)) {
                    foreach (int id in Nodes[nextID].NeighborIDs) {
                        idsToVisit.Enqueue(id);
                    }
                    connectedNodes.Add(Nodes[nextID]);
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
            foreach (int id in Nodes.Keys) {
                idStack.Push(id);
            }
            while(idStack.TryPop(out int nextID)) {
                bool inComponent = false;
                if(connectedComponents == new List<List<GraphNode<TGraphType>>>()) { //? if this was the first node in the stack just search
                    connectedComponents.Add(DFS(nextID)); //?either search works
                    continue;
                }
                foreach (var _class in connectedComponents) { //? if this node is in one of the components we have already searched move on to the next
                    if(_class.Contains(Nodes[nextID])) 
                        {inComponent = true;}
                }
                if(!inComponent) connectedComponents.Add(DFS(nextID)); //? if it isn't in any of them add a new one by searching from it
            }
            return connectedComponents;
        }
        public bool HasPath(int node1ID, int node2ID){ //? this is identical to asking if two nodes are in the same equivalence class
            return HasPath(Nodes[node1ID],Nodes[node2ID]);
        }
        public bool HasPath(GraphNode<TGraphType> node1, GraphNode<TGraphType> node2) {
            return DFS(node1).Contains(node2); 
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
    }
}
