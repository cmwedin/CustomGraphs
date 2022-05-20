namespace SadSapphicGames.CustomGraphs
{
    public class UndirectedEdge<TGraphType> : GraphEdge<TGraphType>
    {
        public UndirectedEdge(GraphEdge<TGraphType> _edge) : base(_edge) {
        }

        public UndirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode) : base(_sourceNode, _sinkNode) {
            //? no new code needed
        }

        public override GraphEdge<TGraphType> Copy() {
            return new UndirectedEdge<TGraphType>(this);
        }

        public override GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(node != GetSourceNode() && node != GetSinkNode()) throw new NotAttachedToEdgeException();
            else if(node == GetSourceNode()) return GetSinkNode();
            else return GetSourceNode();  
        }
    }
}