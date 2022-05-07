using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SadSapphicGames.CustomGraphs {
    public class GraphNode<TGraphType> {
        
        private int id;
        public int ID { get => id;}

        private TGraphType value;
        public TGraphType Value { get => value;}

        private List<GraphEdge<TGraphType>> edges = new List<GraphEdge<TGraphType>>();
        public List<GraphEdge<TGraphType>> Edges { get => edges;}
        //private List<int> neighborIDs; 
        public List<int> NeighborIDs { get {
            List<int> output = new List<int>();
            foreach (var edge in Edges) {
                output.Add(edge.GetOppositeNode(this).ID); //? this should always be a sink node right now, edges are only added to sources
            }
            return output;
        }}


        // * Constructors
        public GraphNode(int _id) { //? edges are to be initialized in graph constructors
            this.id = _id;
            this.value = default(TGraphType);
        }

        public void AddEdge(GraphEdge<TGraphType> _edge) {
            if(Edges.Contains(_edge)) return;
            if(ID != _edge.SourceNode.ID && ID !=_edge.SinkNode.ID) throw new NotAttachedToEdgeException();
            edges.Add(_edge);
        } 

    }
}