using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ? 
namespace SadSapphicGames.CustomGraphs {
    public class RootedTree<TGraphType> : Tree<TGraphType> {
        public GraphNode<TGraphType> FindRootNode()
        {
            // ! this is probably not efficient
            // ! linear worst case (tree is a linked list)
            // ! should set a reference to this so we can get the root in constant time but would need to update that when operations that change the root happen
            var node = GetRandomNode();
            while (node.GetInEdges().Count != 0)
            {
                node = node.GetInEdges()[0].GetOppositeNode(node);
            }
            return node;
        }
        public GraphNode<TGraphType> GetParentNodeOf(GraphNode<TGraphType> node) {
            if (node.ParentGraph != this) { throw new DifferentGraphsException();}
            if (node.GetInEdges().Count == 0) {
                Debug.LogWarning("This is the root node, returning null for its parent");
                return null;
            } else {
                return node.GetInEdges()[0].GetOppositeNode(node);
            }
        }
        public List<GraphNode<TGraphType>> GetLayer(int k) {
            List<GraphNode<TGraphType>> prevLayer = new List<GraphNode<TGraphType>>{ FindRootNode() };
            IEnumerable<GraphNode<TGraphType>> nextLayer;

            int currentDepth = 0;
            while (currentDepth < k) {
                nextLayer = new List<GraphNode<TGraphType>>{};
                foreach (var node in prevLayer) {
                    // Debug.Log($"Adding children of node {node.ID} to next layer");
                    nextLayer = nextLayer.Concat(GetChildren(node));
                }
                // Debug.Log($"Next layer has {nextLayer.Count()} nodes");
                prevLayer = new List<GraphNode<TGraphType>>(nextLayer);
                currentDepth++;
            } 
            return prevLayer;
        }
        public List<GraphNode<TGraphType>> GetChildren(GraphNode<TGraphType> node) {
            if (node.ParentGraph != this) { throw new DifferentGraphsException();}
            var children = new List<GraphNode<TGraphType>>();
            foreach(var edge in node.GetOutEdges()) {
                if(node != edge.GetSinkNode()) {
                    children.Add(edge.GetOppositeNode(node));
                }
            }
            return children;
        }
// * Constructors
        public RootedTree(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
        }

        public RootedTree() : base() {
        }
// * Overrides
        // public override bool TryAddEdge(int id1, int id2) {
        //     if(nodes.ContainsKey(id1)) {
        //         if(nodes.ContainsKey(id2)) {
        //             Debug.LogWarning("Trees cannot have an edge added between two existing nodes without creating a cycle, which trees do not contain");
        //             return false;
        //         } else {
        //             return TryAddEdge(GetNode(id1), new GraphNode<TGraphType>(id2));
        //         }
        //     } else if(nodes.ContainsKey(id2)) {
        //         Debug.LogWarning($"you are trying to add a new in edge to node {id2}. A node in a rooted tree can only have one parent / in edge");
        //         return false;
        //     } else {
        //         //? neither node is a part of the tree
        //         Debug.LogWarning("at least one node of the edge must already be a part of the tree as a tree only contains one connected component");
        //         return false;
        //     }          
        // }
        // public override bool TryAddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
        //     if(v1.ParentGraph == null && v2.ParentGraph == this) {
        //         Debug.LogWarning($"you are trying to add a new in edge to node {v2.ID}. A node in a rooted tree can only have one parent / in edge");
        //         return false;
        //     } else if(v1.ParentGraph == this && v2.ParentGraph == null) {
        //         TryAddNode(v2); //TODO don't like this - see tree class
        //         var _edge = new UndirectedEdge<TGraphType>(v1.ID,v2.ID);
        //         edges.Add(_edge.ID,_edge);
        //         return true;
        //     } else {
        //         Debug.LogWarning("An edge can only be added to a tree between a node it already contains and a new/orphan node, doing otherwise would break the tree condition");
        //         return false;
        //     }
        // }
        public override bool TryAddEdge(AbstractEdge<TGraphType> edge) {
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
                    var node = new GraphNode<TGraphType>(edge.SinkNodeID); 
                    node.SetParent(this);
                    nodes.Add(node.ID, node);
                    
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
    }
}
