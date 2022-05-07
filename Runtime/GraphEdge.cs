namespace SadSapphicGames.CustomGraphs{
    public class GraphEdge<TGraphType> {
        private GraphNode<TGraphType> sourceNode;
        private GraphNode<TGraphType> sinkNode;
        public GraphNode<TGraphType> SourceNode { get => sourceNode; }
        public GraphNode<TGraphType> SinkNode { get => sinkNode; }

        public GraphEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode) {
            sourceNode = _sourceNode;
            sinkNode = _sinkNode;

            sourceNode.AddEdge(this);
            //? sinkNode.AddEdge(this); in an undirected edge this could be uncommented
        }

        public GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(node != sourceNode && node != sinkNode) throw new NotAttachedToEdgeException();
            else if(node == sourceNode) return sinkNode;
            else return sourceNode; // ? if neither of the previous conditions are true we know node is the sink node
        }
    }
}