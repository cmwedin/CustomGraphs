using System;
using System.Collections.Generic;
using System.Collections.Immutable; // ? turns out we might not need this
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

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
        public AbstractGraph<TGraphType> ParentGraph { get => parentGraph;}
        public List<string> edgeIDs { get { // //? since the list is of value types modifications to this have no effect on the actual edges
                List<string> output = new List<string>();
                foreach (var id in inEdgeIDs) {
                    output.Add(id);
                }
                foreach (var id in outEdgeIDs) {
                    if(!output.Contains(id))output.Add(id);
                }
                return output;
        }}

// * Reference Types - Private
        private TGraphType value; // ? This could hypothetically be a value type
        private AbstractGraph<TGraphType> parentGraph;
        private List<string> outEdgeIDs = new List<string>();
        private List<string> inEdgeIDs = new List<string>(); // ? undirected edges are both inEdges and 


// ! Methods -------
// * Member Accessors 
        public void SetValue(TGraphType _value) {
            value = _value;
        }
        public void SetParent(AbstractGraph<TGraphType> newParent) {
            if(parentGraph != null) {
                Debug.LogWarning("You must orphan this node first before setting a new parent");
                return;
            }
            parentGraph = newParent;
        }
        public ReadOnlyCollection<AbstractEdge<TGraphType>> GetOutEdges() {
            List<AbstractEdge<TGraphType>> output = ParentGraph.GetEdgeList(outEdgeIDs);
            return output.AsReadOnly();
        }

        public ReadOnlyCollection<AbstractEdge<TGraphType>> GetInEdges() {
            List<AbstractEdge<TGraphType>> output = ParentGraph.GetEdgeList(inEdgeIDs);
            return output.AsReadOnly();
        }
        public List<int> NeighborIDs { get { 
            List<int> output = new List<int>();
            foreach (var edge in GetOutEdges()) {
                output.Add(edge.GetOppositeNode(this).ID);
            }
            return output;
        } }

// * Constructors
        public GraphNode(int _id, AbstractGraph<TGraphType> _parentGraph = null) { //? edges are to be initialized in graph constructors
            this.id = _id;
            this.parentGraph = _parentGraph;
            this.value = default(TGraphType);
        }

        //? copy constructor
        public GraphNode(GraphNode<TGraphType> _node) {
            this.id = _node.ID;
            this.value = ObjectExtensions.Copy(_node.Value);
            this.inEdgeIDs = ObjectExtensions.Copy(_node.inEdgeIDs);
            this.outEdgeIDs = ObjectExtensions.Copy(_node.outEdgeIDs);
            this.parentGraph = null;
        }

// * Modification Methods
        public void AddEdge(AbstractEdge<TGraphType> _edge) {
            if(outEdgeIDs.Contains(_edge.ID) || inEdgeIDs.Contains(_edge.ID)) return;
            if(_edge.GetSinkNode() == this) { inEdgeIDs.Add(_edge.ID);} //? if this node is a sink add it to in edges
            if(_edge.GetOppositeNode(this) != this){outEdgeIDs.Add(_edge.ID);} //? if the other node is accessible add it to out edge (and undirected edge will be both) 
        }     
        internal void RemoveEdge(AbstractEdge<TGraphType> edge) {
            //? this should only be called from AbstractGraph.RemoveEdge(edge)
            //? which will make sure both nodes of an edge have it removed
            inEdgeIDs.Remove(edge.ID);
            outEdgeIDs.Remove(edge.ID);
        }
        internal void ClearEdges() { // ? this should only be used when moving a node from one graph to another;
            inEdgeIDs = new List<string>();
            outEdgeIDs = new List<string>();
        }
    }
}