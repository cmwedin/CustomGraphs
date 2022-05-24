using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs
{
    public static class ShortestPath<TGraphType> {
        public static float PathCost(IEnumerable<AbstractEdge<TGraphType>> path) {
            float cost = 0;
            foreach(var edge in path) {cost += edge.Weight;}
            return cost;
        }
        public static string PathAsString(IEnumerable<AbstractEdge<TGraphType>> path) {
            var asStrings = new List<string>(); 
            foreach(var edge in path) {asStrings.Add(edge.ID);}
            return string.Join("|",asStrings);
        }
        public static Dictionary<int,float> DAGShortestPath(
            DirectedGraph<TGraphType> graph,
            int startingNodeID,
            //? we need to make sure our return values are primitive types so the reference dont become empty leaving the stack frame
            out Dictionary<int,string> bestPathIDsOut
        ) {
            var startingNode = graph.GetNode(startingNodeID);
            Debug.Log("Starting new DAG shortest paths eval");
            if(!TarjanSCCSolver<TGraphType>.CheckDAG(graph)) throw new NotDAGException();
            Dictionary<int,string> bestPathIDs = new Dictionary<int,string>(); 
            Dictionary<int, float> bestPathCost = new Dictionary<int, float>();
            List<DirectedEdge<TGraphType>> currentPath = new List<DirectedEdge<TGraphType>>();
        
            //? start the search
            foreach (var node in graph.GetAllNodes()) {
                Debug.Log($"Initializing node {node.ID} with cost -1");
                bestPathCost.Add(node.ID,-1);
            }
            bestPathIDsOut = DAGRecursion(startingNode, currentPath, bestPathCost, bestPathIDs);
            return bestPathCost;
        }

        private static Dictionary<int, string> DAGRecursion( 
            GraphNode<TGraphType> currentNode,
            List<DirectedEdge<TGraphType>> currentPath,
            Dictionary<int, float> bestPathCost,
            Dictionary<int, string> bestPathIDs
        ) {
            Debug.Log($"DAG shortest path recursion called, current path: {PathAsString(currentPath)}");
            foreach(DirectedEdge<TGraphType> edge in currentNode.GetOutEdges()) {
                currentPath.Add(edge);
                var oppNode = edge.GetOppositeNode(currentNode);
                if(bestPathCost[oppNode.ID] != -1) {
                    if(bestPathCost[oppNode.ID] > PathCost(currentPath)) {
                        Debug.Log($"Found better path for node {oppNode.ID}, setting cost to {PathCost(currentPath)}");
                        bestPathCost[oppNode.ID] = PathCost(currentPath);
                        bestPathIDs[oppNode.ID] = PathAsString(currentPath);
                    } else { continue;}
                } else {
                    Debug.Log($"reached node {oppNode.ID} for first time"); //! the shows up for node 1
                    Debug.Log("initializing best path");
                    bestPathIDs[oppNode.ID] = PathAsString(currentPath);
                    Debug.Log($"initializing best cost to {PathCost(currentPath)}");
                    bestPathCost[oppNode.ID] = PathCost(currentPath); //! presumably this breaks the if?
                    Debug.Log("continuing recursion");  //! this code forward isn't running
                    DAGRecursion(
                        oppNode,
                        currentPath,
                        bestPathCost,
                        bestPathIDs
                    );
                } 
                currentPath.Remove(edge);
            }
            return bestPathIDs;
        }

        // public static Dictionary<int,float> DijkstraShortestPath(AbstractGraph<TGraphType> graph, int startingNodeID,out Dictionary<int,string> bestPaths) {
        //     var startingNode = graph.GetNode(startingNodeID);
        //     var queue = new PriorityQueue<TElement,TPriority>(); 
        //     Dictionary<int, float> output = null;
        //     return output;
        // }

    }
}