using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public static class TopologicalSort<TGraphType> {
        public static Dictionary<int,GraphNode<TGraphType>> Sort(Graph<TGraphType> graph) {
            if(!TarjanSCCSolver<TGraphType>.CheckDAG(graph)) {
                Debug.LogWarning("only a DAG can be Top-Sorted");
                return null;
            }
            Dictionary<int,int> nodeDegrees = new Dictionary<int, int>();
            Dictionary<int,GraphNode<TGraphType>> sortedNodes = new Dictionary<int, GraphNode<TGraphType>>();
            Queue<GraphNode<TGraphType>> sortQ = new Queue<GraphNode<TGraphType>>();
            
            foreach(var node in graph.Nodes.Values) {
                nodeDegrees.Add(node.ID,node.InEdges.Count);
                if(nodeDegrees[node.ID] == 0) sortQ.Enqueue(node);
            }
            
            int i = 0;
            while(sortQ.TryDequeue(out GraphNode<TGraphType> nextNode)) {
                sortedNodes.Add(i,nextNode);
                foreach(var edge in nextNode.OutEdges) {
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