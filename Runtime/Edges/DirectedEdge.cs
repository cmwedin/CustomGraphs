namespace SadSapphicGames.CustomGraphs {
    public class DirectedEdge<TGraphType> : GraphEdge<TGraphType> {


        public DirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode) : base(_sourceNode, _sinkNode) {
            //? no new code needed
        }
        public DirectedEdge(GraphEdge<TGraphType> _edge) : base(_edge) {
        }
        public override GraphEdge<TGraphType> Copy() {
            return new DirectedEdge<TGraphType>(this);
        }

        public override GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(node != GetSourceNode() && node != GetSinkNode()) throw new NotAttachedToEdgeException();
            else if(node == GetSourceNode()) return GetSinkNode();
            else return node;
        }
    }
}