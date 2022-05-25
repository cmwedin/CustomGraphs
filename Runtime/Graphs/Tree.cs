using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class Tree<TGraphType> : UndirectedGraph<TGraphType>
    {
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
                //? we already know id doesnt contain id1
                return TryAddEdge(new GraphNode<TGraphType>(id1), GetNode(id2));
            } else {
                //? neither node is a part of the tree
                Debug.LogWarning("at least one node of the edge must already be a part of the tree as a tree only contains one connected component");
                return false;
            }          
        }
        public override bool TryAddEdge(GraphNode<TGraphType> v1, GraphNode<TGraphType> v2) {
            if(v1.ParentGraph == null && v2.ParentGraph == this) {
                AddNode(v1); //TODO I dont like being able to do this, adding a node without adding an edge as well would break the tree condition
                var _edge = new UndirectedEdge<TGraphType>(v1,v2);
                edges.Add(_edge.ID,_edge);
                return true;
            } else if(v1.ParentGraph == this && v2.ParentGraph == null) {
                AddNode(v2); //TODO " "
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
                    AddNode(edge.SourceNodeID); 
                    edge.SetParent(this);
                    GetNode(edge.SourceNodeID).AddEdge(edge);
                    GetNode(edge.SinkNodeID).AddEdge(edge);
                    edges.Add(edge.ID, edge);
                    return true;
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
// * Constructors
        public Tree(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
            //? we shouldn't have to do anything special when constructing a tree
            //? so we just check this actually is a tree after the base constructor does its job
            if(!Tree<TGraphType>.VerifyTree(this)) throw new NotATreeException();
        }

// * Static Methods
        public static bool VerifyTree(AbstractGraph<TGraphType> _graph) {
            if(_graph is DirectedGraph<TGraphType>) {
                // ? i feel in CS this is often overlooked do to the prevalence of rooted trees 
                //? but in mathematically graph theory a tree must form one connected component
                Debug.LogWarning("A tree is by definition undirected");
                return false;
            }
            List<GraphNode<TGraphType>> visitedNodes = null;
            foreach (var _node in _graph.GetAllNodes()) {
                if(visitedNodes == null) {
                    //? this is the first node we tested
                    if(CycleSolver<TGraphType>.FindCycleFrom(_node, out visitedNodes)) {
                        //? we found a cycle 
                        return false;
                    } // ? visitedNodes should now contain every node in the tree
                } else if(visitedNodes.Contains(_node)) { continue;
                } else {
                    //? this graph has a node that is not reachable from the first node
                    //? since we didn't find a cycle in the its connected component we visited all of it 
                    //? therefore the graph is not connected and not a tree
                    return false;
                }               
            }
            return true;
        }
    }
}