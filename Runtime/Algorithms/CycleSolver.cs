using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs {
    public static class CycleSolver<TGraphType> {
        public static bool FindCycleFrom(
            // ? in
            GraphNode<TGraphType> currentNode, 
            List<AbstractEdge<TGraphType>> visitedEdges = null,
            List<GraphNode<TGraphType>> visitedNodes = null
        ) {
            return FindCycleFrom(currentNode, out var empty, visitedEdges, visitedNodes);
            // visitedEdges ??= new List<GraphEdge<TGraphType>>(); 
            // visitedNodes ??= new List<GraphNode<TGraphType>>();
            // visitedNodes.Add(currentNode);
            // foreach (var edge in currentNode.GetOutEdges()) {
            //     if(visitedEdges.Contains(edge)) continue;
            //     visitedEdges.Add(edge);
            //     var oppositeNode = edge.GetOppositeNode(currentNode);
            //     if(visitedNodes.Contains(oppositeNode)) return true;
            //     if(FindCycleFrom(oppositeNode, visitedEdges, visitedNodes)) return true;
            // }
            // return false;
        } 
        public static bool FindCycleFrom(
            // ? in
            GraphNode<TGraphType> currentNode, 
            out List<GraphNode<TGraphType>> touchedNodes, 
            List<AbstractEdge<TGraphType>> visitedEdges = null,
            List<GraphNode<TGraphType>> visitedNodes = null
        ) {;
            visitedNodes ??= new List<GraphNode<TGraphType>>();
            visitedEdges ??= new List<AbstractEdge<TGraphType>>(); 
            visitedNodes.Add(currentNode);
            touchedNodes = visitedNodes;
            foreach (var edge in currentNode.GetOutEdges()) {
                if(visitedEdges.Contains(edge)) continue;
                visitedEdges.Add(edge);
                var oppositeNode = edge.GetOppositeNode(currentNode);
                if(visitedNodes.Contains(oppositeNode)) return true;
                if(FindCycleFrom(oppositeNode, out touchedNodes, visitedEdges, visitedNodes)) return true;
            }
            return false;

        }
    }
}