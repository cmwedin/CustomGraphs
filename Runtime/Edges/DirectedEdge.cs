namespace SadSapphicGames.CustomGraphs {
    public class DirectedEdge<TGraphType> : GraphEdge<TGraphType> {
        public DirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode, float weight = 1) : base(_sourceNode, _sinkNode, weight) {
            //? no new code needed
        }
        public DirectedEdge(GraphEdge<TGraphType> _edge) : base(_edge) {
        }

        public DirectedEdge(int _sourceID, int _sinkID, float weight = 1) : base(_sourceID, _sinkID, weight) {
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