using System.Collections;
using System.Collections.Generic;

internal class GraphNode<TGraphType> {
    private int ID;
    private List<int> neighborIDs; 

    // * Constructor
    public GraphNode(int _id, List<int>_neighborIDs) {
        this.ID = _id;
        this.neighborIDs = _neighborIDs;
    }
}