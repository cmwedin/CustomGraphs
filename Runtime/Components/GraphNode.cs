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



        // * Reference Types - Private
        private TGraphType value; // ? This could hypothetically be a value type
        private AbstractGraph<TGraphType> parentGraph = null;
        private List<string> outEdgeIDs = new List<string>();
        private List<string> inEdgeIDs = new List<string>(); // ? undirected edges are both inEdges and 


// ! Methods -------
// * Field Accessors 
        public ReadOnlyCollection<AbstractEdge<TGraphType>> GetOutEdges() {
            if(parentGraph == null) {
                Debug.LogWarning("This node does not have a parent to get its out edges from, returning null");
                return null;
            }
            List<AbstractEdge<TGraphType>> output = ParentGraph.GetEdgeList(outEdgeIDs);
            return output.AsReadOnly();
        }

        public ReadOnlyCollection<AbstractEdge<TGraphType>> GetInEdges() {
            if(parentGraph == null) {
                Debug.LogWarning("This node does not have a parent to get its in edges from, returning null");
                return null;
            }
            List<AbstractEdge<TGraphType>> output = ParentGraph.GetEdgeList(inEdgeIDs);
            return output.AsReadOnly();
        }
        public List<string> GetEdgeIDs() { // //? since the list is of value types modifications to this have no effect on the actual edges
            List<string> output = new List<string>();
            foreach (var id in inEdgeIDs)
            {
                output.Add(id);
            }
            foreach (var id in outEdgeIDs)
            {
                if (!output.Contains(id)) output.Add(id);
            }
            return output;
        }
        public List<int> GetNeighborIDs() {
            List<int> output = new List<int>();
            foreach (var edge in GetOutEdges())
            {
                output.Add(edge.GetOppositeNode(this).ID);
            }
            return output;
        }

// * Constructors
        public GraphNode(
            int _id,
            TGraphType value = default(TGraphType)
        ) {
            this.id = _id;
            this.value = value;
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
        public void SetValue(TGraphType _value) {
            value = _value;
        }
        internal void AddEdge(AbstractEdge<TGraphType> _edge) {
            if(parentGraph == null) {
                Debug.LogWarning("This node must be added to a graph before it can have edges added");
                return;
            }
            if(outEdgeIDs.Contains(_edge.ID) || inEdgeIDs.Contains(_edge.ID)) {
                Debug.LogWarning($"node already contains and edge with id {_edge.ID}");
                return;
            }
            if(_edge.SinkNodeID == this.ID) { inEdgeIDs.Add(_edge.ID);} //? if this node is a sink add it to in edges
            if(_edge.GetOppositeNode(this) != this){outEdgeIDs.Add(_edge.ID);} //? if the other node is accessible add it to out edge (and undirected edge will be both) 
        }     
        internal void RemoveEdge(AbstractEdge<TGraphType> edge) {
            //? this should only be called from AbstractGraph.RemoveEdge(edge)
            //? which will make sure both nodes of an edge have it removed
            if(parentGraph == null) {
                Debug.LogWarning("This node must be added to a graph before it can have edges removed");
                return;
            }
            inEdgeIDs.Remove(edge.ID);
            outEdgeIDs.Remove(edge.ID);
        }
        internal void ClearEdges() { // ? this should only be used when moving a node from one graph to another;
            if(parentGraph != null) {
                Debug.LogWarning("This node must be orphaned before having its edges cleared");
                return;
            }
            inEdgeIDs = new List<string>();
            outEdgeIDs = new List<string>();
        }
        internal void SetParent(AbstractGraph<TGraphType> _parent) {
            if(parentGraph != null) {
                Debug.LogWarning("You must orphan this node first before setting a new parent");
                return;
            }
            parentGraph = _parent;
        }
    }
}