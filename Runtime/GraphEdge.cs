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
            sinkNode.AddEdge(this);
        }

        public GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(node != sourceNode && node != sinkNode) throw new NotAttachedToEdgeException();
            else if(node == sourceNode) return sinkNode;
            else return node; // ? in directed graphs we identify incoming edges by edges who's opposite nodes are the node itself   
        }
    }
}