using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class UndirectedGraph<TGraphType> : AbstractGraph<TGraphType> { 
        public UndirectedGraph(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public UndirectedGraph(int V, List<int[]> E) : base(V, E) {
        }

        public UndirectedGraph() : base () {
        }

        public override AbstractGraph<TGraphType> Copy() {
            UndirectedGraph<TGraphType> copyGraph = new UndirectedGraph<TGraphType>();
            foreach (var _node in this.GetAllNodes()) {
                copyGraph.AddNode(new GraphNode<TGraphType>(_node));
            }      
            foreach (var _edge in this.GetAllEdges()) {
                copyGraph.AddEdge(new UndirectedEdge<TGraphType>(_edge));
            }
            return copyGraph;
        }

        public override void AddNewEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            base.AddNewEdge(v1, v2); //? just checks that v1 and v2 are in the graph
            var edge = new UndirectedEdge<TGraphType>(v1,v2); //? we do this first so we can access its ID when adding it to the dict 
            edges.Add(edge.ID, edge);
        }
        protected override void AddEdge(AbstractEdge<TGraphType> edgeToAdd) {
            if(edgeToAdd is UndirectedEdge<TGraphType>) base.AddEdge(edgeToAdd);
            else throw new IncompatibleEdgeException();
        }
    }
}
