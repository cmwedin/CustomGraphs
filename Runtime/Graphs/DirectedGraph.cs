using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class DirectedGraph<TGraphType> : AbstractGraph<TGraphType>
    {
        public DirectedGraph(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public DirectedGraph(int V, List<int[]> E) : base(V, E) {
        }

        public override void AddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            base.AddEdge(v1, v2); //? just checks that v1 and v2 are in the graph
            edges.Add(new DirectedEdge<TGraphType>(v1,v2));
        }
    }
}
