using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs
{
    public static class ShortestPath<TGraphType> {
        // TODO this smells
            //? why this has to smell:
            //? I can cast a directed or undirected edge to an abstract edge
            //? but a list of one type cannot be cast to a list of another type as easily 
            //? these functions provide a work around for that   
        //TODO all this repeated code makes me feel like there is a better way to do this
        //TODO but this should work for now
        private static float PathCost(List<DirectedEdge<TGraphType>> path) {
            List<AbstractEdge<TGraphType>> asAbstract = (List<AbstractEdge<TGraphType>>)path.Cast<AbstractEdge<TGraphType>>();
            return PathCost(asAbstract);
        }
        private static float PathCost(List<UndirectedEdge<TGraphType>> path) {
            List<AbstractEdge<TGraphType>> asAbstract = (List<AbstractEdge<TGraphType>>)path.Cast<AbstractEdge<TGraphType>>();
            return PathCost(asAbstract);
        }
        public static float PathCost(List<AbstractEdge<TGraphType>> path) {
            float cost = 0;
            foreach(var edge in path) {cost += edge.Weight;}
            return cost;
        }
        private static List<string> PathAsString(List<DirectedEdge<TGraphType>> path) {
            // List<AbstractEdge<TGraphType>> asAbstract = (List<AbstractEdge<TGraphType>>)path.Cast<AbstractEdge<TGraphType>>();
            // return PathAsString(asAbstract);
            List<string> asString = (List<string>)path.Cast<string>();
            return asString;
        }
        private static List<string> PathAsString(List<UndirectedEdge<TGraphType>> path) {
            // List<AbstractEdge<TGraphType>> asAbstract = (List<AbstractEdge<TGraphType>>)path.Cast<AbstractEdge<TGraphType>>();
            // return PathAsString(asAbstract);
            List<string> asString = (List<string>)path.Cast<string>();
            return asString;
        }
        // public static List<string> PathAsString(List<AbstractEdge<TGraphType>> path) {
        //     var asString = new List<string>(); 
        //     foreach(var edge in path) {asString.Add((string)edge);}
        //     return asString;
        // }
        public static Dictionary<int,List<DirectedEdge<TGraphType>>> DAGShortestPath(DirectedGraph<TGraphType> graph, GraphNode<TGraphType> startingNode) {
            Debug.Log("Starting new DAG shortest paths eval");
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
            Debug.Log($"DAG shortest path recursion called, current path: {string.Join("|",currentPath)}");
            foreach(DirectedEdge<TGraphType> edge in currentNode.GetOutEdges()) {
                currentPath.Add(edge);
                var oppNode = edge.GetOppositeNode(currentNode);
                if(bestPathWeight[oppNode.ID] == null) {
                    //? this is our first time reaching this node
                    bestPathWeight[oppNode.ID] = PathCost(currentPath);
                    bestPaths[oppNode.ID] = currentPath;
                    //? this was our first time hear so continue searching from it
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
