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
        public DirectedGraph(AbstractGraph<TGraphType> _graph) : base(_graph) {

        }

        protected override void InitializeEdges(List<int[]> edgeList) {
            foreach(var _edgeID in edgeList) {
                if ((_edgeID.Length != 2) || !nodes.ContainsKey(_edgeID[0]) || !nodes.ContainsKey(_edgeID[1])) throw new System.Exception("invalid initial edge list");
                var edge = new DirectedEdge<TGraphType>(_edgeID[0],_edgeID[1]);
                edge.SetParent(this);
                GetNode(edge.SourceNodeID).AddEdge(edge);
                GetNode(edge.SinkNodeID).AddEdge(edge);
                edges.Add(edge.ID,edge);
            }
        }  
  
        // public override bool TryAddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
        //     if(v1.ParentGraph == null) {
        //         Debug.LogWarning($"Node with ID {v1.ID} had to be added to graph before adding an edge from it");
        //         this.TryAddNode(v1);}
        //     if(v2.ParentGraph == null) {
        //         Debug.LogWarning($"Node with ID {v2.ID} had to be added to graph before adding an edge to it");
        //         this.TryAddNode(v2);}
        //     if(v1.ParentGraph != v2.ParentGraph) {
        //         Debug.LogWarning("Tried to add an edge between two node with different parents");
        //         return false;
        //     } else if (v1.ParentGraph != this) {
        //         Debug.LogWarning("Trying to add an edge between a different graphs node");
        //         return false;
        //     } //? both node parents are the same and are this
        //     var edge = new DirectedEdge<TGraphType>(v1.ID,v2.ID); //? we do this first so we can access its ID when adding it to the dict 
        //     // Debug.Log($"adding edge {edge.ID}");
        //     edges.Add(edge.ID,edge);
        //     return true;
        // }

        public override bool TryAddEdge(AbstractEdge<TGraphType> edge) {
            if(!(edge is DirectedEdge<TGraphType>)) {
                Debug.LogWarning("A directed graph can only have directed Edges added");
                return false;
            } 
            if(edges.ContainsKey(edge.ID)) {
                Debug.LogWarning("This edge has the same ID as one already in the graph, parallel edges are not currently supported");
                return false;
            }
            if(edge.ParentGraph != null) {
                Debug.LogWarning("This edge is already attached to a graph, it must be removed from its parent before it can be added to another graph");
                Debug.LogWarning("if it cannot be removed consider the copy method or orphan edge constructor");
                return false;
            }
            if(!nodes.ContainsKey(edge.SourceNodeID)) { 
                if(!TryAddNode(new GraphNode<TGraphType>(edge.SourceNodeID))) {
                    Debug.LogWarning($"Graph does not contain node {edge.SourceNodeID} and it could not be added to the graph");
                    return false;
                }
            }
            if(!nodes.ContainsKey(edge.SinkNodeID)) { 
                if(!TryAddNode(new GraphNode<TGraphType>(edge.SinkNodeID))) {
                    Debug.LogWarning($"Graph does not contain node {edge.SinkNodeID} and it could not be added to the graph");
                    return false;
                }
            }
            edge.SetParent(this);
            GetNode(edge.SourceNodeID).AddEdge(edge);
            GetNode(edge.SinkNodeID).AddEdge(edge);
            edges.Add(edge.ID, edge);
            return true;
        }
    }
}
