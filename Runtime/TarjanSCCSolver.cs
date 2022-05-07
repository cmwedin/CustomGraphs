using System;
using System.Collections;
using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs{
    public class TarjanSCCSolver<TGraphType> {
        private Graph<TGraphType> graph;
        private bool[] onStack, visited;
        private int[] low, scc;
        private Stack<GraphNode<TGraphType>> nodeStack;

        private bool solved = false;
        List<List<GraphNode<TGraphType>>> solution = new List<List<GraphNode<TGraphType>>>();

        // * Constructor
        public TarjanSCCSolver(Graph<TGraphType> _graph) {
            if(_graph == null) throw new System.Exception("TarjanSolver's graph cannot be null");
            if(graph.Size == 0) throw new EmptyGraphException();
            this.graph = _graph;
        }

        public List<List<GraphNode<TGraphType>>> GetSolution() {
            if(solved) return solution;
            Solve();
            return solution;
        }

        private void Solve() {
            if(solved) return;
            //ids = new int[graph.Size];
            low = new int[graph.Size];
            scc = new int[graph.Size];
            Array.Fill<int>(scc, -1);

            visited = new bool[graph.Size];
            onStack = new bool[graph.Size];
            nodeStack = new Stack<GraphNode<TGraphType>>();

            for (int id = 0; id < graph.Size; id++) {
                if(visited[id] == false) TarjanDFS(graph.Nodes[id]); //? the start of tarjan can be random so we just use whatever node in the graph happens to have id 0
            }
        }

        private void TarjanDFS(GraphNode<TGraphType> node) {
            nodeStack.Push(node);
            onStack[node.ID] = true;
            visited[node.ID] = true;
            low[node.ID] = node.ID;

            //? for all the edges on the start node, recursively call the function and update the value of low-link value of that node 
            foreach (var edge in node.Edges) {
                GraphNode<TGraphType> neighbor = edge.GetOppositeNode(node);
                if(visited[neighbor.ID] == false) {
                    TarjanDFS(neighbor); } //? call this function for my neighbor
                if(onStack[neighbor.ID]) low[node.ID] = Math.Min(low[node.ID],low[neighbor.ID]); //? if my neighbor found a lower link than me, update mine to theirs
            }

            if(low[node.ID] == node.ID) { //? if this node is the start of a strongly connected component
                while(nodeStack.TryPop(out GraphNode<TGraphType> topNode) && topNode.ID != node.ID) { //? remove all nodes from the stack till we reach its initial call
                    onStack[topNode.ID] = false;
                    low[topNode.ID] = node.ID; //?
                }
            }
        }
    }
}
