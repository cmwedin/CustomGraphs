namespace SadSapphicGames.CustomGraphs
{
    public class UndirectedEdge<TGraphType> : GraphEdge<TGraphType>
    {
        public UndirectedEdge(GraphEdge<TGraphType> _edge) : base(_edge) {
        }

        public UndirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode, float weight = 1) : base(_sourceNode, _sinkNode, weight) {
            //? no new code needed
        }

        public UndirectedEdge(int _sourceID, int _sinkID, float weight = 1) : base(_sourceID, _sinkID, weight) {
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