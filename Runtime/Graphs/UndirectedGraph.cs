using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class UndirectedGraph<TGraphType> : AbstractGraph<TGraphType> { 
        public UndirectedGraph(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public UndirectedGraph(int V, List<int[]> E) : base(V, E) {
        }

        public override void AddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            base.AddEdge(v1, v2); //? just checks that v1 and v2 are in the graph
            edges.Add(new UndirectedEdge<TGraphType>(v1,v2));
        }
    }
}
