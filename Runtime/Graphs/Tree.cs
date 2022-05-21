using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CustomGraphs {
    public class Tree<TGraphType> : UndirectedGraph<TGraphType>
    {
        public Tree(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
            
        }

        public static bool VerifyTree(AbstractGraph<TGraphType> _graph) {
            if(_graph is DirectedGraph<TGraphType>) {
                // ? i feel in CS this is often overlooked do to the prevalence of rooted trees 
                //? but in mathematically graph theory a tree must form one connected component
                Debug.LogWarning("A tree is by definition undirected");
                return false;
            }
            
            return true;
        }
    }
}