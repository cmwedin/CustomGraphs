using System.Collections;
using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs {
    public class GraphNode<TGraphType> {
        private int id;
        private TGraphType value;
        private List<int> neighborIDs; 
        public List<int> NeighborIDs { get => neighborIDs; set => neighborIDs = value; }
        public int ID { get => id; set => id = value; }
        public TGraphType Value { get => value;}


        // * Constructor
        public GraphNode(int _id, List<int>_neighborIDs) {
            this.ID = _id;
            this.NeighborIDs = _neighborIDs;
            this.value = default(TGraphType);
        }

    }
}