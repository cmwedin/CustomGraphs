namespace SadSapphicGames.CustomGraphs {
    public class DirectedEdge<TGraphType> : GraphEdge<TGraphType> {
        public DirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode) : base(_sourceNode, _sinkNode) {
            //? no new code needed
        }

        public override GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node)
        {
            var output =  base.GetOppositeNode(node);
            if(node == GetSinkNode()) return node;
            else return output;
        }
    }
}