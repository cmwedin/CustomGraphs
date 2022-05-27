using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs{
    public abstract class AbstractEdge<TGraphType> {
// * Value Types - Public
        public int SourceNodeID { get => sourceNodeID;}
        public int SinkNodeID { get => sinkNodeID;}
        public string ID { get => id;}
        public float Weight { get => weight; }

        // * Value Types - Private
        private string id;
        private int sourceNodeID;
        private int sinkNodeID;
        //? to avoid exponential proliferation of class types (adding a weighted version of all other graph classes) 
        //?"unweighted" graphs use 1
        private float weight;

// * Reference Types - Public
        public AbstractGraph<TGraphType> ParentGraph { get => parentGraph;}
        
// * Reference Types - Private
        private AbstractGraph<TGraphType> parentGraph; //? set to null when copying an edge
// * Member Accessors
        public static explicit operator string(AbstractEdge<TGraphType> edge) {
            return edge.ID;
        }
        public void SetParent(AbstractGraph<TGraphType> newParent) {
            if(parentGraph != null) {
                Debug.LogWarning("You must orphan this edge first before setting a new parent");
                return;
            }
            parentGraph = newParent;
        }
        public GraphNode<TGraphType> GetSourceNode() {
            return parentGraph.GetNode(sourceNodeID);
        }

        public GraphNode<TGraphType> GetSinkNode() {
            return parentGraph.GetNode(sinkNodeID);
        }

        public abstract GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node); //? this function is the main point of distinction between Directed and Undirected
// * Constructors
        // ? orphan edge constructor (intended for use in adding edges by reference)
        public AbstractEdge(int _sourceID, int _sinkID, float weight = 1) {
            sourceNodeID = _sourceID;
            sinkNodeID = _sinkID;
            id = $"{sourceNodeID},{sinkNodeID}";
            this.parentGraph = null;
            this.weight = weight; 
        } 
        // ? Standard constructor
        public AbstractEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode, float weight = 1)
        {
            sourceNodeID = _sourceNode.ID;
            sinkNodeID = _sinkNode.ID;
            id = $"{sourceNodeID},{sinkNodeID}";
            this.parentGraph = _sourceNode.ParentGraph; //TODO this is insufficient
            GetSourceNode().AddEdge(this);
            GetSinkNode().AddEdge(this);
            this.weight = weight;            
        }

        //? copy constructor
        public abstract AbstractEdge<TGraphType> Copy(); //? so we can access the copy constructor of abstract graphs
        public AbstractEdge(AbstractEdge<TGraphType> _edge) {
            //? we ignore the only reference type member of an edge and consider the new edge to be an orphan
            this.sinkNodeID = _edge.SinkNodeID;
            this.sourceNodeID = _edge.SourceNodeID;
            this.id = _edge.ID;
            this.weight = _edge.Weight;
            this.parentGraph = null; 
        }
// * Modification
        public bool TrySwapNodes() {
            var oldSourceEdgeIDs = GetSourceNode().EdgeIDs;
            oldSourceEdgeIDs.Remove(this.ID);
            var oldSinkEdgeIDs = GetSinkNode().EdgeIDs;
            oldSinkEdgeIDs.Remove(this.ID);
            var newSourceEdgeIDs = new List<string>();
            var newSinkEdgeIDs = new List<string>();
            var edgesToRemove = ParentGraph.GetEdgeList(oldSourceEdgeIDs).Concat(ParentGraph.GetEdgeList(oldSinkEdgeIDs)).ToList();
            foreach(var edge in edgesToRemove) ParentGraph.RemoveEdge(edge);
            //TODO new edge id lists equal the old with SourceNodeID replaced with SinkNodeID and v.v.
            (sourceNodeID, sinkNodeID) = (SinkNodeID, SourceNodeID);
            //TODO id = id reversed
            List<string> edgeIDsToAdd = newSourceEdgeIDs.Concat(newSinkEdgeIDs).ToList();
            foreach(var edgeID in edgeIDsToAdd) {
                string[] nodeIDs = edgeID.Split(",",2);
                //! Problem - this wont work for tree's as it thinks adding an edge will introduce a cycle
                //! which it usually would had we not just deleted edges from the tree
                //! a potentially solution would be to add a replace edge method?
                ParentGraph.TryAddEdge(Int32.Parse(nodeIDs[0]),Int32.Parse(nodeIDs[1])); 
            }
            return true   
        }
    }
}