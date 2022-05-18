using System;
using System.Collections.Generic;
using System.Collections.Immutable; // ? turns out we might not need this
using System.Collections.ObjectModel;
using System.Linq;


namespace SadSapphicGames.CustomGraphs
{
    public class GraphNode<TGraphType> {
// ! Members -------
// * Value Types - Public
        public int ID { get => id;}        
// * Value Types - Private
        private int id;

// * Reference Types - Public
        public TGraphType Value { get => value;}
        public AbstractGraph<TGraphType> ParentGraph { get => parentGraph; set => parentGraph = value; }

// * Reference Types - Private
        private TGraphType value; // ? This could hypothetically be a value type
        private AbstractGraph<TGraphType> parentGraph;
        private List<GraphEdge<TGraphType>> outEdges = new List<GraphEdge<TGraphType>>();
        private List<GraphEdge<TGraphType>> inEdges = new List<GraphEdge<TGraphType>>(); // ? undirected edges are both inEdges and 


// ! Methods -------
// * Member Accessors 
        public void SetValue(TGraphType _value) {
            value = _value;
        }
        public ReadOnlyCollection<GraphEdge<TGraphType>> GetOutEdges() {
            return outEdges.AsReadOnly();
        }

        public ReadOnlyCollection<GraphEdge<TGraphType>> GetInEdges() {
            return inEdges.AsReadOnly();
        }
        public List<int> NeighborIDs { get { 
            List<int> output = new List<int>();
            foreach (var edge in GetOutEdges()) {
                output.Add(edge.GetOppositeNode(this).ID);
            }
            return output;
        } }

// * Constructors
        public GraphNode(int _id, AbstractGraph<TGraphType> _parentGraph) { //? edges are to be initialized in graph constructors
            this.id = _id;
            this.parentGraph = _parentGraph;
            this.value = default(TGraphType);
        }

        public void AddEdge(GraphEdge<TGraphType> _edge) {
            if(_edge.SinkNode == this) {InEdges.Add(_edge);} //? if this node is a sink add it to in edges
            if(GetOutEdges().Contains(_edge) || GetInEdges().Contains(_edge)) return;
            if(_edge.GetOppositeNode(this) != this){outEdges.Add(_edge);} //? if the other node is accessible add it to out edge (and undirected edge will be both) 
        }     

        internal void RemoveEdge(GraphEdge<TGraphType> edge) {
            //? this should only be called from AbstractGraph.RemoveEdge(edge)
            //? which will make sure both nodes of an edge have it removed
            inEdges.Remove(edge);
            outEdges.Remove(edge);
        }
    }
}