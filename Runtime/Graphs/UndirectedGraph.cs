using System;
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
                copyGraph.TryAddEdge(new UndirectedEdge<TGraphType>(_edge));
            }
            return copyGraph;
        }
        protected override void InitializeEdges(List<int[]> edgeList) {
            foreach(var _edgeID in edgeList) {
                if ((_edgeID.Length != 2) || !nodes.ContainsKey(_edgeID[0]) || !nodes.ContainsKey(_edgeID[1])) throw new System.Exception("invalid initial edge list");
                var edge = new UndirectedEdge<TGraphType>(GetNode(_edgeID[0]),GetNode(_edgeID[1]));
                edges.Add(edge.ID,edge);
            }
        }

        public override bool TryAddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            if(v1.ParentGraph == null) {this.AddNode(v1);}
            if(v2.ParentGraph == null) {this.AddNode(v2);}
            if(v1.ParentGraph != v2.ParentGraph) {
                Debug.LogWarning("Tried to add an edge between two node with different parents");
                return false;
            } else if (v1.ParentGraph != this) {
                Debug.LogWarning("Trying to add an edge between a different graphs node");
                return false;
            } //? both node parents are the same and are this
            var edge = new UndirectedEdge<TGraphType>(v1,v2); //? we do this first so we can access its ID when adding it to the dict 
            // Debug.Log($"adding edge {edge.ID}");
            edges.Add(edge.ID,edge);
            return true;
        }



        protected override bool TryAddEdge(AbstractEdge<TGraphType> edgeToAdd) {
            if(!(edgeToAdd is UndirectedEdge<TGraphType>)) {
                Debug.LogWarning("An undirected graph can only have undirected Edges added");
                return false;
            } 
            if(edges.ContainsKey(edgeToAdd.ID)) {
                Debug.LogWarning("This edge has the same ID as one already in the graph, parallel edges are not currently supported");
                return false;
            }
            if(edgeToAdd.ParentGraph != null) {
                Debug.LogWarning("This edge is already attached to a graph, it must be removed from its parent before it can be added to another graph");
                Debug.LogWarning("if it cannot be removed consider the copy method or orphan edge constructor");
                return false;
            }
            if(!nodes.ContainsKey(edgeToAdd.SourceNodeID)) { AddNode(edgeToAdd.SourceNodeID); }
            if(!nodes.ContainsKey(edgeToAdd.SinkNodeID)) { AddNode(edgeToAdd.SinkNodeID); }
            edgeToAdd.SetParent(this);
            GetNode(edgeToAdd.SourceNodeID).AddEdge(edgeToAdd);
            GetNode(edgeToAdd.SinkNodeID).AddEdge(edgeToAdd);
            edges.Add(edgeToAdd.ID, edgeToAdd);
            return true;
        }
    }
}
