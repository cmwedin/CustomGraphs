namespace SadSapphicGames.CustomGraphs
{
    public class UndirectedEdge<TGraphType> : GraphEdge<TGraphType>
    {
        public UndirectedEdge(GraphEdge<TGraphType> _edge) : base(_edge) {
        }

        public UndirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode) : base(_sourceNode, _sinkNode) {
            //? no new code needed
        }

        public override GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            //? the base implementation assumes undirected graph
            return base.GetOppositeNode(node);
        }
    }
}