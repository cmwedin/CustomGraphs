using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ? 
namespace SadSapphicGames.CustomGraphs {
    public class RootedTree<TGraphType> : Tree<TGraphType> {
        public GraphNode<TGraphType> RootNode { get {
            // ! this is probably not efficient
            // ! should set a reference to this so we can get the root in constant time
            var node = GetRandomNode();
            while(node.GetInEdges().Count != 0) {
                node = node.GetInEdges()[0].GetOppositeNode(node); 
            }
            return node;
        }}
        public GraphNode<TGraphType> GetParentNode(GraphNode<TGraphType> node) {
            if (node.ParentGraph != this) { throw new DifferentGraphsException();}
            if (node.GetInEdges().Count == 0) {
                Debug.LogWarning("This is the root node, returning null for its parent");
                return null;
            } else {
                return node.GetInEdges()[0].GetOppositeNode(node);
            }
        }
        public List<GraphNode<TGraphType>> GetChildren(GraphNode<TGraphType> node) {
            if (node.ParentGraph != this) { throw new DifferentGraphsException();}
            var children = new List<GraphNode<TGraphType>>();
            foreach(var edge in node.GetOutEdges()) {children.Add(edge.GetOppositeNode(node));}
            return children;
        }
// * Constructors
        public RootedTree(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public RootedTree() : base() {
        }
// * Overrides
        public override bool TryAddEdge(int id1, int id2) {
            if(nodes.ContainsKey(id1)) {
                if(nodes.ContainsKey(id2)) {
                    Debug.LogWarning("Trees cannot have an edge added between two existing nodes without creating a cycle, which trees do not contain");
                    return false;
                } else {
                    return TryAddEdge(GetNode(id1), new GraphNode<TGraphType>(id2));
                }
            } else if(nodes.ContainsKey(id2)) {
                Debug.LogWarning($"you are trying to add a new in edge to node {id2}. A node in a rooted tree can only have one parent / in edge");
                return false;
            } else {
                //? neither node is a part of the tree
                Debug.LogWarning("at least one node of the edge must already be a part of the tree as a tree only contains one connected component");
                return false;
            }          
        }
        public override bool TryAddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            if(v1.ParentGraph == null && v2.ParentGraph == this) {
                Debug.LogWarning($"you are trying to add a new in edge to node {v2.ID}. A node in a rooted tree can only have one parent / in edge");
                return false;
            } else if(v1.ParentGraph == this && v2.ParentGraph == null) {
                AddNode(v2); //TODO don't like this - see tree class
                var _edge = new UndirectedEdge<TGraphType>(v1,v2);
                edges.Add(_edge.ID,_edge);
                return true;
            } else {
                Debug.LogWarning("An edge can only be added to a tree between a node it already contains and a new/orphan node, doing otherwise would break the tree condition");
                return false;
            }
        }
        protected override bool TryAddEdge(AbstractEdge<TGraphType> edge) {
            if(!(edge is UndirectedEdge<TGraphType>)) {
                Debug.LogWarning("A Tree is a type of undirected graph");
                return false;
            } else if(edges.ContainsKey(edge.ID)) {
                Debug.LogWarning("This edge has the same ID as one already in the graph, parallel edges are not currently supported");
                return false;
            } else if(edge.ParentGraph != null) {
                Debug.LogWarning("This edge is already attached to a graph, it must be removed from its parent before it can be added to another graph");
                Debug.LogWarning("if it cannot be removed consider the copy method or orphan edge constructor");
                return false;
            } else {
                if(!nodes.ContainsKey(edge.SourceNodeID) && nodes.ContainsKey(edge.SinkNodeID)) { 
                    Debug.LogWarning($"you are trying to add a new in edge to node {edge.SinkNodeID}. A node in a rooted tree can only have one parent / in edge");
                    return false;
                } else if(nodes.ContainsKey(edge.SourceNodeID) && !nodes.ContainsKey(edge.SinkNodeID)) { 
                    AddNode(edge.SinkNodeID); 
                    edge.SetParent(this);
                    GetNode(edge.SourceNodeID).AddEdge(edge);
                    GetNode(edge.SinkNodeID).AddEdge(edge);
                    edges.Add(edge.ID, edge);
                    return true;
                } else {
                    Debug.LogWarning("An edge can only be added to a tree between a node it already contains and a new/orphan node, doing otherwise would break the tree condition");
                    return false;
                }
            }
        }

        public List<GraphNode<float>> GetLayer(int bottomLayer) {
            throw new NotImplementedException();
        }
    }
}
