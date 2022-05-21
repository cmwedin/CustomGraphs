using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs {
    public static class CycleSolver<TGraphType> {
        public static bool FindCycleFrom(
            // ? in
            GraphNode<TGraphType> currentNode, 
            GraphNode<TGraphType> parentNode = null, 
            List<GraphEdge<TGraphType>> visitedEdges = null
        ) {
            parentNode ??= currentNode;
            visitedEdges ??= new List<GraphEdge<TGraphType>>(); 
            foreach (var edge in currentNode.GetOutEdges()) {
                if(visitedEdges.Contains(edge)) continue;
                visitedEdges.Add(edge);
                var oppositeNode = edge.GetOppositeNode(currentNode);
                if(oppositeNode == parentNode) return true;
                if(FindCycleFrom(oppositeNode, parentNode, visitedEdges)) return true;
            }
            return false;
        } 
        public static bool FindCycleFrom(
            // ? in
            GraphNode<TGraphType> currentNode, 
            out List<GraphNode<TGraphType>> touchedNodes,
            GraphNode<TGraphType> parentNode = null, 
            List<GraphEdge<TGraphType>> visitedEdges = null,
            List<GraphNode<TGraphType>> visitedNodes = null
        ) {
            parentNode ??= currentNode;
            visitedNodes ??= new List<GraphNode<TGraphType>>();
            visitedEdges ??= new List<GraphEdge<TGraphType>>(); 
            visitedNodes.Add(currentNode);
            touchedNodes = visitedNodes;
            foreach (var edge in currentNode.GetOutEdges()) {
                if(visitedEdges.Contains(edge)) continue;
                visitedEdges.Add(edge);
                var oppositeNode = edge.GetOppositeNode(currentNode);
                if(oppositeNode == parentNode) return true;
                if(FindCycleFrom(oppositeNode, out touchedNodes, parentNode, visitedEdges, visitedNodes)) return true;
            }
            return false;

        }
    }
}