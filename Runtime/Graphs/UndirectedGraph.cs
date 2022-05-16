using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class UndirectedGraph<TGraphType> : Graph<TGraphType> { 
        public UndirectedGraph(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public UndirectedGraph(int V, List<int[]> E) : base(V, E) {
        }

        public override void AddEdge(int v1, int v2) {
            base.AddEdge(v1, v2); //? just checks that v1 and v2 are in the graph
            edges.Add(new UndirectedEdge<TGraphType>(Nodes[v1],Nodes[v2]));
        }
    }
}
