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
                        connectedNodes.Add(Nodes[nextID]);
                    }
                } else {
                    //? incase I need to do anything when reaching a visited node in the future
                } 
            }
            return connectedNodes;
        }

        public List<GraphNode<TGraphType>> BFS(int nodeID)
        {
            return BFS(Nodes[nodeID]);
        }

        private List<GraphNode<TGraphType>> BFS(GraphNode<TGraphType> node)
        {
            List<GraphNode<TGraphType>> connectedNodes = new List<GraphNode<TGraphType>>{node};
            List<int> visitedIDs = new List<int>{node.ID};
            Queue<int> idsToVisit = new Queue<int>(node.NeighborIDs);
            while (idsToVisit.TryDequeue(out int nextID)) {        
                if(VisitNode(nextID,visitedIDs)) {
                    foreach (int id in Nodes[nextID].NeighborIDs) {
                        idsToVisit.Enqueue(id);
                        connectedNodes.Add(Nodes[nextID]);
                    }
                } else {
                    //? incase I need to do anything when reaching a visited node in the future
                } 
            }
            return connectedNodes;
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
