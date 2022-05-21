using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs {
    public static class CycleSolver<TGraphType> {
        public static bool FindCycleFrom(
            GraphNode<TGraphType> startNode, 
            GraphNode<TGraphType> parentNode = null, 
            List<GraphEdge<TGraphType>> visitedEdges = null
        ) {
            parentNode ??= startNode;
            visitedEdges ??= new List<GraphEdge<TGraphType>>(); 
            foreach (var edge in startNode.GetOutEdges()) {
                if(visitedEdges.Contains(edge)) continue;
                visitedEdges.Add(edge);
                var oppositeNode = edge.GetOppositeNode(startNode);
                if(oppositeNode == parentNode) return true;
                if(FindCycleFrom(oppositeNode, parentNode, visitedEdges)) return true;
            }
            return false;
        } 
    }
}