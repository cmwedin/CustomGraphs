using UnityEngine;

namespace SadSapphicGames.CustomGraphs
{
    public class UndirectedEdge<TGraphType> : AbstractEdge<TGraphType>
    {
        public UndirectedEdge(AbstractEdge<TGraphType> _edge) : base(_edge) {
        }

        // public UndirectedEdge(GraphNode<TGraphType> _sourceNode, GraphNode<TGraphType> _sinkNode, float weight = 1) : base(_sourceNode, _sinkNode, weight) {
        //     //? no new code needed
        // }

        public UndirectedEdge(int _sourceID, int _sinkID, float weight = 1) : base(_sourceID, _sinkID, weight) {
        }

        public override GraphNode<TGraphType> GetOppositeNode(GraphNode<TGraphType> node) {
            if(ParentGraph == null) {
                Debug.LogWarning("You must assign this edge to a graph before getting the nodes attached to it, returning null");
                return null;
            }
            if(node != GetSourceNode() && node != GetSinkNode()) throw new NotAttachedToEdgeException();
            else if(node == GetSourceNode()) return GetSinkNode();
            else return GetSourceNode();  
        }
    }
}