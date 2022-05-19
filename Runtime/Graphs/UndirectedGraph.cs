using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class UndirectedGraph<TGraphType> : AbstractGraph<TGraphType> { 
        public UndirectedGraph(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public UndirectedGraph(int V, List<int[]> E) : base(V, E) {
        }
        public UndirectedGraph(AbstractGraph<TGraphType> _graph) : base(_graph) {
            if(!(_graph is UndirectedGraph<TGraphType>)) throw new IncompatibleEdgeException();
            foreach (var _edge in _graph.GetAllEdges()) {
                this.AddEdge(new UndirectedEdge<TGraphType>(_edge));
            }
        }

        public override void AddNewEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            base.AddNewEdge(v1, v2); //? just checks that v1 and v2 are in the graph
            var edge = new UndirectedEdge<TGraphType>(v1,v2); //? we do this first so we can access its ID when adding it to the dict 
            edges.Add(edge.ID, edge);
        }
        protected override void AddEdge(GraphEdge<TGraphType> edgeToAdd) {
            if(edgeToAdd is UndirectedEdge<TGraphType>) base.AddEdge(edgeToAdd);
            else throw new IncompatibleEdgeException();
        }
    }
}
