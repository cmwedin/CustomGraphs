using System.Collections;
using System.Collections.Generic;

public class Graph<TGraphType> {
    private Dictionary<int , GraphNode<TGraphType>> Nodes;

    public Graph(Dictionary<int,List<int>> adjacencyList) {
        foreach (int id in adjacencyList.Keys) {
            Nodes.Add(id, new GraphNode<TGraphType>(id, adjacencyList[id]));
        }
    }
}
