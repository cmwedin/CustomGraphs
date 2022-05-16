using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SadSapphicGames.CustomGraphs {
    public class GraphNode<TGraphType> {
        
        private int id;
        public int ID { get => id;}

        private TGraphType value;
        public TGraphType Value { get => value;}

        private List<GraphEdge<TGraphType>> outEdges = new List<GraphEdge<TGraphType>>();
        public List<GraphEdge<TGraphType>> OutEdges { get => outEdges;}
        private List<GraphEdge<TGraphType>> inEdges = new List<GraphEdge<TGraphType>>(); // ? undirected edges are always out edges
        public List<GraphEdge<TGraphType>> InEdges { get => inEdges;}
        public List<int> NeighborIDs { get {
            List<int> output = new List<int>();
            foreach (var edge in OutEdges) {
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
            if(OutEdges.Contains(_edge) || InEdges.Contains(_edge)) return;
            if(_edge.GetOppositeNode(this) == this) InEdges.Add(_edge); //? this will also throw an exception when trying to add edges to nodes they aren't attached to
            else {outEdges.Add(_edge);} //? this else is redundant but adds to readability
        } 

    }
}