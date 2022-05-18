using System;

namespace SadSapphicGames.CustomGraphs{
    public abstract class GraphEdge<TGraphType> {
// * Value Types - Public
        public int SourceNodeID { get => sourceNodeID;}
        public int SinkNodeID { get => sinkNodeID;}
        public string ID { get => $"{SourceNodeID},{SinkNodeID}";}
        public float Weight { get => weight; }

        // * Value Types - Private
        private int sourceNodeID;
        private int sinkNodeID;
        //? to avoid exponential proliferation of class types (adding a weighted version of all other graph classes) 
        //?"unweighted" graphs just ignore this variable
        private float weight;

// * Reference Types - Public
        
// * Reference Types - Private
        private AbstractGraph<TGraphType> parentGraph; //? set to null when copying an edge

// * Constructors
        public GraphEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode, float weight = 1)
        {
            sourceNodeID = _sourceNode.ID;
            sinkNodeID = _sinkNode.ID;
            this.parentGraph = _sourceNode.ParentGraph;
            GetSourceNode().AddEdge(this);
            GetSinkNode().AddEdge(this);
            this.weight = weight;
            
        }

        //? copy constructor
        public GraphEdge(GraphEdge<TGraphType> _edge) {
        }
        public GraphNode<TGraphType> GetSourceNode() {
            return parentGraph.Nodes[sourceNodeID];
        }

        public GraphNode<TGraphType> GetSinkNode() {
            return parentGraph.Nodes[sinkNodeID];
        }

        public virtual GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(node != GetSourceNode() && node != GetSinkNode()) throw new NotAttachedToEdgeException();
            else if(node == GetSourceNode()) return GetSinkNode();
            else return GetSourceNode();  
        }
    }
}