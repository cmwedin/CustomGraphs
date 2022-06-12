using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public static class TopologicalSort<TGraphType> {
        public static Dictionary<GraphNode<TGraphType>,int> Sort(DirectedGraph<TGraphType> graph) {
            if(!TarjanSCCSolver<TGraphType>.CheckDAG(graph)) {
                Debug.LogWarning("only a DAG can be Top-Sorted");
                return null;
            }
            Dictionary<int,int> nodeDegrees = new Dictionary<int, int>();
            Dictionary<GraphNode<TGraphType>,int> sortedNodes = new Dictionary<GraphNode<TGraphType>,int>();
            Queue<GraphNode<TGraphType>> sortQ = new Queue<GraphNode<TGraphType>>();
            
            foreach(var node in graph.GetAllNodes()) {
                nodeDegrees.Add(node.ID,node.GetInEdges().Count);
                if(nodeDegrees[node.ID] == 0) sortQ.Enqueue(node);
            }
            
            int i = 0;
            while(sortQ.TryDequeue(out GraphNode<TGraphType> nextNode)) {
                sortedNodes.Add(nextNode,i);
                foreach(var edge in nextNode.GetOutEdges()) {
                    var neighbor = edge.GetOppositeNode(nextNode);
                    nodeDegrees[neighbor.ID]--;
                    if(nodeDegrees[neighbor.ID] == 0) sortQ.Enqueue(neighbor);
                }
                i++;
            }
            return sortedNodes;
        }
    }
}