using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs{
    public abstract class Graph<TGraphType> { //? the default type of graph is directed and unweighted
        protected Dictionary<int , GraphNode<TGraphType>> nodes = new Dictionary<int, GraphNode<TGraphType>>();
        public Dictionary<int, GraphNode<TGraphType>> Nodes { get => nodes;}

        public int Size { get => Nodes.Keys.Count;}
        protected List<GraphEdge<TGraphType>> edges = new List<GraphEdge<TGraphType>>();
        public List<GraphEdge<TGraphType>> Edges { get => edges;}


        // * Constructors
        public Graph(Dictionary<int,List<int>> adjacencyList) { //? O(V+E) time
            foreach (int id in adjacencyList.Keys) { 
                nodes.Add(id, new GraphNode<TGraphType>(id));
            }
            foreach (int id in Nodes.Keys) {
                foreach (int adjID in adjacencyList[id]) {
                    AddEdge(id,adjID);
                }
            }
        }
        public Graph(int V, List<int[]> E) { //? O(V+E) time
            for (int id = 0; id < V; id++) {
                nodes.Add(id, new GraphNode<TGraphType>(id));
            }
            foreach (var edge in E) {
                if(edge.Length != 2) throw new Exception("each edges array length must be exactly 2");
                this.AddEdge(edge[0],edge[1]);
            }
        }
        public virtual void AddEdge(int v1, int v2) {
            if(!Nodes.ContainsKey(v1)) throw new NotInGraphException(v1);
            if(!Nodes.ContainsKey(v2)) throw new NotInGraphException(v2);
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
        // ? a potentially usefull observation is in an undirected graph connectedness forms an equivalence relation
        public List<List<GraphNode<TGraphType>>> GetConnectedComponents(){ //? this type name is rather cumbersome
            List<List<GraphNode<TGraphType>>> connectedComponents = new List<List<GraphNode<TGraphType>>>();
            Stack<int> idStack = new Stack<int>();
            foreach (int id in Nodes.Keys) {
                idStack.Push(id);
            }
            while(idStack.TryPop(out int nextID)) {
                bool inClass = false;
                if(connectedComponents == new List<List<GraphNode<TGraphType>>>()) { //? if this was the first node in the stack just search
                    connectedComponents.Add(DFS(nextID)); //?either search works
                    continue;
                }
                foreach (var _class in connectedComponents) { //? if this node is in one of the equiv classes we have already searched move on to the next
                    if(_class.Contains(Nodes[nextID])) 
                        {inClass = true;}
                }
                if(!inClass) connectedComponents.Add(DFS(nextID)); //? if it isn't in any of them add a new one by searching from it
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

        private bool VisitNode(int id, List<int> visitedIDs) { //? may be usefull for future functionality
            if(visitedIDs.Contains(id)) return false;
            else {
                visitedIDs.Add(id);
                return true;
            } 
        }
    }
}
