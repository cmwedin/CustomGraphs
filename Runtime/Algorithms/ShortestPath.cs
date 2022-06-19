using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SadSapphicGames.DataStructures;

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
            //? we need to make sure our return values are primitive types so the reference don't become empty leaving the stack frame
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
                    Debug.Log($"reached node {oppNode.ID} for first time"); 
                    Debug.Log("initializing best path");
                    bestPathIDs[oppNode.ID] = PathAsString(currentPath);
                    Debug.Log($"initializing best cost to {PathCost(currentPath)}");
                    bestPathCost[oppNode.ID] = PathCost(currentPath); 
                    Debug.Log("continuing recursion");
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

        public static Dictionary<GraphNode<TGraphType>, float> DijkstraShortestPath(
            GraphNode<TGraphType> startNode,
            out Dictionary<GraphNode<TGraphType>, string> bestPathIDs
        ) {
            var graph = startNode.ParentGraph;
            if(graph == null) {
                Debug.LogWarning("This node is not attached to a graph, returning null");
                bestPathIDs = null;
                return null;
            }
            
        // ? initialization
            // List<AbstractEdge<TGraphType>> currentPath = new List<AbstractEdge<TGraphType>>();
            List<AbstractEdge<TGraphType>> visitedEdges = new List<AbstractEdge<TGraphType>>();
            Dictionary<GraphNode<TGraphType>, float> bestPathCost = new Dictionary<GraphNode<TGraphType>, float>();
            var bestPaths = new Dictionary<GraphNode<TGraphType>, List<AbstractEdge<TGraphType>>>();
            bestPathIDs = new Dictionary<GraphNode<TGraphType>, string>();
            int D = Mathf.Clamp(Mathf.FloorToInt(graph.GetAllEdges().Count/graph.GetAllNodes().Count),2,int.MaxValue);
            Debug.Log($"optimal heap degree for this graph is {D}");
            D_aryHeap<GraphNode<TGraphType>> heap = new D_aryHeap<GraphNode<TGraphType>>(D);
            foreach(var node in graph.GetAllNodes()) {
                bestPathCost.Add(node,float.PositiveInfinity);
                bestPaths.Add(node, new List<AbstractEdge<TGraphType>>());
            }
            bestPathCost[startNode] = 0;
            heap.Push(startNode,0);
            
        // ? Algorithm body
            while (heap.TryPop(out var currentNode)) {
                Debug.Log($"evaluating node {currentNode.ID}");
                foreach (var edge in currentNode.GetOutEdges()) {
                    if(visitedEdges.Contains(edge)) continue;
                    if(edge.Weight < 0) {
                        Debug.LogWarning($"edge {edge.ID} has weight {edge.Weight}, Dijkstra's Algorithm cannot be run on graphs with negative edge weights. Returning null");
                        bestPathIDs = null;
                        return null;
                    } else { //? this else is for readability, not needed
                        Debug.Log($"evaluating edge {edge.ID}");
                        visitedEdges.Add(edge);
                        var oppositeNode = edge.GetOppositeNode(currentNode); 

                        if(bestPathCost[oppositeNode] == float.PositiveInfinity) { //? if this is our first time reaching this node
                            Debug.Log($"this is the first time reaching node {oppositeNode.ID}");
                            bestPaths[oppositeNode] = new List<AbstractEdge<TGraphType>>(bestPaths[currentNode]);
                            bestPaths[oppositeNode].Add(edge);
                            Debug.Log($"initializing best path to {PathAsString(bestPaths[oppositeNode])}");
                            bestPathIDs[oppositeNode] = PathAsString(bestPaths[oppositeNode]);
                            bestPathCost[oppositeNode] = PathCost(bestPaths[oppositeNode]);
                            heap.Push(oppositeNode,bestPathCost[oppositeNode]);
                        } else if(bestPathCost[currentNode] + edge.Weight < bestPathCost[oppositeNode]) { //? if this path to this node is better than the previous
                            Debug.Log($"better path to node {oppositeNode.ID} found with cost {bestPathCost[currentNode] + edge.Weight}, previous best path cost was {bestPathCost[oppositeNode]}");
                            bestPaths[oppositeNode] = new List<AbstractEdge<TGraphType>>(bestPaths[currentNode]);
                            bestPaths[oppositeNode].Add(edge);
                            bestPathIDs[oppositeNode] = PathAsString(bestPaths[oppositeNode]);
                            bestPathCost[oppositeNode] = PathCost(bestPaths[oppositeNode]);
                            heap.DecreaseKey(oppositeNode,bestPathCost[oppositeNode]);
                        } else { //? our previous path to this node was better than this one, this section is for readability
                            continue;
                        }
                    }
                }
            }
            return bestPathCost;
        }


        // public static Dictionary<int,float> DijkstraShortestPath(AbstractGraph<TGraphType> graph, int startingNodeID,out Dictionary<int,string> bestPaths) {
        //     var startingNode = graph.GetNode(startingNodeID);
        //     var queue = new PriorityQueue<TElement,TPriority>(); 
        //     Dictionary<int, float> output = null;
        //     return output;
        // }

    }
}