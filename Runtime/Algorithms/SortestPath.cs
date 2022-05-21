using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs
{
    public static class ShortestPath<TGraphType> {
        // TODO these two smell
        private static float PathCost(List<DirectedEdge<TGraphType>> path) {
            float cost = 0;
            foreach(var edge in path) {cost += edge.Weight;}
            return cost;
        }
        private static float PathCost(List<UndirectedEdge<TGraphType>> path) {
            float cost = 0;
            foreach(var edge in path) {cost += edge.Weight;}
            return cost;
        }
        public static Dictionary<int,List<DirectedEdge<TGraphType>>> DAGShortestPath(DirectedGraph<TGraphType> graph, GraphNode<TGraphType> startingNode) {
            if(!TarjanSCCSolver<TGraphType>.CheckDAG(graph)) throw new NotDAGException();
            Dictionary<int, List<DirectedEdge<TGraphType>>> bestPaths = new Dictionary<int, List<DirectedEdge<TGraphType>>>();
            Dictionary<int, float?> bestPathWeight = new Dictionary<int, float?>();
            List<DirectedEdge<TGraphType>> currentPath = new List<DirectedEdge<TGraphType>>();
        
            //? start the search
            foreach (var node in graph.GetAllNodes()) {
                bestPathWeight.Add(node.ID,null);
            }
            DAGRecursion(startingNode, currentPath, bestPathWeight, bestPaths);
            return bestPaths;

        }

        private static void DAGRecursion(
            GraphNode<TGraphType> currentNode,
            List<DirectedEdge<TGraphType>> currentPath,
            Dictionary<int, float?> bestPathWeight,
            Dictionary<int, List<DirectedEdge<TGraphType>>> bestPaths
        ) {
            foreach(DirectedEdge<TGraphType> edge in currentNode.GetOutEdges()) {
                currentPath.Add(edge);
                var oppNode = edge.GetOppositeNode(currentNode);
                if(bestPathWeight[oppNode.ID] == null) {
                    //? this is our first time reaching this node
                    bestPathWeight[oppNode.ID] = PathCost(currentPath);
                    bestPaths[oppNode.ID] = currentPath;
                    //? this was our first time hear so start a new search from it
                    DAGRecursion(
                        oppNode,
                        currentPath,
                        bestPathWeight,
                        bestPaths
                    );
                } else if(bestPathWeight[oppNode.ID] > PathCost(currentPath)) {
                    //? this path is better then whatever our pervious best path for this node was
                    bestPathWeight[oppNode.ID] = PathCost(currentPath);
                    bestPaths[oppNode.ID] = currentPath;
                }
                currentPath.Remove(edge);
            }
        }

    }
}
