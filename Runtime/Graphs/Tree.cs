using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs {
    public class Tree<TGraphType> : UndirectedGraph<TGraphType>
    {
        public Tree(Dictionary<int, List<int>> adjacencyList) : base(adjacencyList) {
            
        }
    }
}