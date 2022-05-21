using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class Tree<TGraphType> : UndirectedGraph<TGraphType>
    {
        public Tree(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
            //? we shouldn't have to do anything special when constructing a tree
            //? so we just check this actually is a tree after the base constructor does its job
            if(!Tree<TGraphType>.VerifyTree(this)) throw new NotATreeException();
        }

        public static bool VerifyTree(AbstractGraph<TGraphType> _graph) {
            if(_graph is DirectedGraph<TGraphType>) {
                // ? i feel in CS this is often overlooked do to the prevalence of rooted trees 
                //? but in mathematically graph theory a tree must form one connected component
                Debug.LogWarning("A tree is by definition undirected");
                return false;
            }
            List<GraphNode<TGraphType>> visitedNodes = null;
            foreach (var _node in _graph.GetAllNodes()) {
                if(visitedNodes.Contains(_node)) { continue;
                } else if(visitedNodes == null) {
                    //? this is the first node we tested
                    if(CycleSolver<TGraphType>.FindCycleFrom(_node, out visitedNodes)) {
                        //? we found a cycle 
                        return false;
                    } // ? visitedNodes should now contain every node in the tree
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