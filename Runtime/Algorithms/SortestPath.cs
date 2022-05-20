using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs
{
    public static class ShortestPath<TGraphType> {
        public static Dictionary<int,List<DirectedEdge<TGraphType>>> DAGShortestPath(DirectedGraph<TGraphType> graph, GraphNode<TGraphType> startingNode) {
            Dictionary<int, List<DirectedEdge<TGraphType>>> output = new Dictionary<int, List<DirectedEdge<TGraphType>>>();
            var sortedNodes = TopologicalSort<TGraphType>.Sort(graph);
            if(sortedNodes == null) throw new System.Exception("graph is not acyclic"); //? should make a NotADAG exception
            return output;
        }
    }
}
