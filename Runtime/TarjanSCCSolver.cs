using System;
using System.Collections;
using System.Collections.Generic;

namespace SadSapphicGames.CustomGraphs{
    public static class TarjanSCCSolver<TGraphType> {
        // private Graph<TGraphType> graph;
        //? these could also be arrays but im using dicts to decouple the behavior of graph nodes from int id's incase i want to change that in the future
        //? however the space complexity of this solution could be rather large, testing will be needed
        private static Dictionary<GraphNode<TGraphType>,bool> onStack; //? bools to track if each node is on the stacks or visited
        private static Dictionary<GraphNode<TGraphType>, int> tarjanIDs; //? order of each node in the algorithm, also wether the node is unvisited (-1)
        private static int[] lowlink; //? lowest valued id reachable from the node
        private static Stack<GraphNode<TGraphType>> nodeStack; //? stack of nodes in the current scc

        // private bool solved = false;
        private static Dictionary<GraphNode<TGraphType>,int> finalLowLinks = new Dictionary<GraphNode<TGraphType>, int>();
        // private Dictionary<int,List<GraphNode<TGraphType>>> solution = new Dictionary<int, List<GraphNode<TGraphType>>>();

       // * Constructor
        // public TarjanSCCSolver(Graph<TGraphType> _graph) {
        //     if(_graph == null) throw new System.Exception("TarjanSolver's graph cannot be null");
        //     if(_graph.Size == 0) throw new EmptyGraphException();
        //     this.graph = _graph;
        // }

        // public static Dictionary<int,List<GraphNode<TGraphType>>> GetSolution() {
        //     if(solved) return solution;
        //     Solve();
        //     return solution;
        // }
        // public int GetCycleNumber() {
        //     if (!solved) {Solve();}
        //     return solution.Values.Count;
        // } 
        public static bool CheckDAG(Graph<TGraphType> graph) {
            var sccList = Solve(graph);
            return sccList.Count == 0;
        }

        public static List<List<GraphNode<TGraphType>>> Solve(Graph<TGraphType> graph) {
            lowlink = new int[graph.Size];
            onStack = new Dictionary<GraphNode<TGraphType>, bool>();
            tarjanIDs = new Dictionary<GraphNode<TGraphType>, int>();
            List<List<GraphNode<TGraphType>>> sccList = new List<List<GraphNode<TGraphType>>>();

            foreach (var node in graph.Nodes.Values) { //? this does take O(V) time however at sufficiently large arguments  O(V)+O(V+E) ~ O(V+E)
                tarjanIDs.Add(node, -1);
                onStack.Add(node, false);
            }

            nodeStack = new Stack<GraphNode<TGraphType>>();
            int iterationDepth = -1; // TODO might be worthwhile to make this a prop that throws an excep if its value is greater than graph.Size
            for (int id = 0; id < graph.Size; id++) {
                if(tarjanIDs[graph.Nodes[id]] == -1) TarjanDFS(sccList, graph.Nodes[id],iterationDepth,out iterationDepth); 
                //? the start of tarjan can be random so we just use whatever node in the graph happens to have id 0
                //? if i make node id's a generic type in the future I could pick a random element of graph.Nodes.Values
            }
            return sccList;
        }

        private static void TarjanDFS(
            List<List<GraphNode<TGraphType>>> sccList,
            GraphNode<TGraphType> node, //? what node are we starting at?
            int iterationDepth, //? how deep in a recursive call stack are we, -1 for the initial call
            out int outDepth //? so we can tell how deep we went on returning to in initial for loop in Solve()
        ) {
            iterationDepth++;
            // if(iterationDepth > graph.Size) throw new Exception("iteration depth to large, terminating algorithm");
            nodeStack.Push(node);
            onStack[node] = true;
            tarjanIDs[node] = iterationDepth;
            lowlink[iterationDepth] = iterationDepth;


            //? for all the edges on the start node, recursively call the function and update the value of low-link value of that node 
            foreach (var edge in node.OutEdges) {
                GraphNode<TGraphType> neighbor = edge.GetOppositeNode(node);
                if(tarjanIDs[neighbor] == -1) {
                    TarjanDFS(sccList, neighbor, iterationDepth, out iterationDepth); } //? call this function for my neighbor
                if(onStack[neighbor]) lowlink[tarjanIDs[node]] = Math.Min(lowlink[tarjanIDs[node]],lowlink[tarjanIDs[neighbor]]); //? if my neighbor found a lower link than me, update mine to theirs
            }

            if(lowlink[tarjanIDs[node]] == tarjanIDs[node]) { //? if this node is the start of a strongly connected component
                List<GraphNode<TGraphType>> scc = new List<GraphNode<TGraphType>>();
                while(nodeStack.TryPop(out GraphNode<TGraphType> topNode)) { //? remove all nodes from the stack till we reach its initial call
                    onStack[topNode] = false;
                    scc.Add(topNode);
                    finalLowLinks[topNode] = tarjanIDs[node];
                    if(tarjanIDs[topNode] == tarjanIDs[node]) break;
                } 
                if(scc.Count > 1) sccList.Add(scc); //? if this node was on the top of the stack we've reached a dead end not a SCC
            }
            outDepth = iterationDepth;
        }
    }
}
