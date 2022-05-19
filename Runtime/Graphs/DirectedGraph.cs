using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class DirectedGraph<TGraphType> : AbstractGraph<TGraphType>
    {

        public DirectedGraph() : base() {
        }
        public DirectedGraph(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public DirectedGraph(int V, List<int[]> E) : base(V, E) {
        }


        public  override AbstractGraph<TGraphType> Copy() {
            DirectedGraph<TGraphType> copyGraph = new DirectedGraph<TGraphType>();
            foreach (var _node in this.GetAllNodes()) {
                copyGraph.AddNode(new GraphNode<TGraphType>(_node));
            }      
            foreach (var _edge in this.GetAllEdges()) {
                copyGraph.AddEdge(new DirectedEdge<TGraphType>(_edge));
            }
            return copyGraph;
        }
  
        public override void AddNewEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            base.AddNewEdge(v1, v2); //? just checks that v1 and v2 are in the graph
            var edge = new DirectedEdge<TGraphType>(v1,v2); //? we do this first so we can access its ID when adding it to the dict 
            // Debug.Log($"adding edge {edge.ID}");
            edges.Add(edge.ID,edge);
        }

        protected override void AddEdge(GraphEdge<TGraphType> edgeToAdd) {
            if(edgeToAdd is DirectedEdge<TGraphType>) base.AddEdge(edgeToAdd);
            else throw new IncompatibleEdgeException();
        }
    }
}
