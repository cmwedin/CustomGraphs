using System;

namespace SadSapphicGames.CustomGraphs{
    public abstract class GraphEdge<TGraphType> {
        private GraphNode<TGraphType> sourceNode;
        private GraphNode<TGraphType> sinkNode;
        //? to avoid exponential proliferation of class types (adding a weighted version of all other graph classes) 
        //?"unweighted" graphs just ignore this variable
        private float weight; 
        public GraphNode<TGraphType> SourceNode { get => sourceNode; }
        public GraphNode<TGraphType> SinkNode { get => sinkNode; }

        public GraphEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode, float weight = 1)
        {
            sourceNode = _sourceNode;
            sinkNode = _sinkNode;
            sourceNode.AddEdge(this);
            sinkNode.AddEdge(this);
            this.weight = weight;
        }

        public virtual GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(node != SourceNode && node != SinkNode) throw new NotAttachedToEdgeException();
            else if(node == SourceNode) return SinkNode;
            else return SourceNode;  
        }
    }
}