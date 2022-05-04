using System.Collections;
using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs {
    public class GraphNode<TGraphType> {
        private int id;
        private List<int> neighborIDs; 
        public List<int> NeighborIDs { get => neighborIDs; set => neighborIDs = value; }
        public int ID { get => id; set => id = value; }


        // * Constructor
        public GraphNode(int _id, List<int>_neighborIDs) {
            this.ID = _id;
            this.NeighborIDs = _neighborIDs;
        }

    }
}