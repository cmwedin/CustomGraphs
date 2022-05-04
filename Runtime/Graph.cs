using System;
using System.Collections;
using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs{
    public class Graph<TGraphType> {
        private Dictionary<int , GraphNode<TGraphType>> _nodes = new Dictionary<int, GraphNode<TGraphType>>();

        public Graph(Dictionary<int,List<int>> adjacencyList) {
            foreach (int id in adjacencyList.Keys) {
                Nodes.Add(id, new GraphNode<TGraphType>(id, adjacencyList[id]));
            }
        }

        public Dictionary<int, GraphNode<TGraphType>> Nodes { get => _nodes;}

        
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
        // ? named based on my observation that graph connectivity forms an equivalence relation (a set theory concept for the unfamiliar)
        public List<List<GraphNode<TGraphType>>> GetEquivalenceClasses(){ //? this type name is rather cumbersome
            List<List<GraphNode<TGraphType>>> equivalenceClasses = new List<List<GraphNode<TGraphType>>>();
            Stack<int> idStack = new Stack<int>();
            foreach (int id in Nodes.Keys) {
                idStack.Push(id);
            }
            while(idStack.TryPop(out int nextID)) {
                bool inClass = false;
                if(equivalenceClasses == new List<List<GraphNode<TGraphType>>>()) { //? if this was the first node in the stack just search
                    equivalenceClasses.Add(DFS(nextID)); //?either search works
                    continue;
                }
                foreach (var _class in equivalenceClasses) { //? if this node is in one of the equiv classes we have already searched move on to the next
                    if(_class.Contains(Nodes[nextID])) 
                        {inClass = true;}
                }
                if(!inClass) equivalenceClasses.Add(DFS(nextID)); //? if it isnt in any of them add a new one by searching from it
            }
            return equivalenceClasses;
        }
        public bool HasPath(int node1ID, int node2ID){ //? this is identical to asking if two nodes are in the same equivalence class
            return HasPath(Nodes[node1ID],Nodes[node2ID]);
        }

        public bool HasPath(GraphNode<TGraphType> graphNode1, GraphNode<TGraphType> graphNode2)
        {
            throw new NotImplementedException();
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
