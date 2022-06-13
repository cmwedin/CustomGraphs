using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class GraphTests
{
    // * Graphs to do tests with
    DirectedGraph<bool> trivialCycle = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1}},
        {1, new List<int>{0}}
    });
    DirectedGraph<bool> childNodes = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1,2}},
        {1, new List<int>{3}},
        {2, new List<int>{4}},
        {3, new List<int>{}},
        {4, new List<int>{}}
    });
    DirectedGraph<bool> islands = new DirectedGraph<bool> ( new Dictionary<int, List<int>> {
        {0, new List<int>{1}},
        {1, new List<int>{0}},
        {2, new List<int>{3}},
        {3, new List<int>{2}}
    });


    [Test]
    public void TestGetNode() {
        DirectedGraph<bool> trivialGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>()}
        });
        Assert.AreEqual(expected: 0, actual: trivialGraph.GetNode(0).ID);
    }
    
    [Test] 
    public void TestGetEdgeIDs() {
        DirectedGraph<float> graph = new DirectedGraph<float> ( new Dictionary<int, List<int>> {
            {0, new List<int>{1}},
            {1, new List<int>{}}
        });

        Assert.AreEqual(expected: 1, actual:graph.GetNode(0).GetEdgeIDs().Count);
    }
    [Test]
    public void TestGetDirectedOutEdges() {
        DirectedGraph<float> graph = new DirectedGraph<float> ( new Dictionary<int, List<int>> {
            {0, new List<int>{1}},
            {1, new List<int>{}}
        });

        Assert.AreEqual(expected: 1, actual:graph.GetNode(0).GetOutEdges().Count);
    }
    [Test]
    public void TestGetUndirectedOutEdges() {
        UndirectedGraph<float> graph = new UndirectedGraph<float> ( new Dictionary<int, List<int>> {
            {0, new List<int>{1}},
            {1, new List<int>{}}
        });

        Assert.AreEqual(expected: 1, actual:graph.GetNode(0).GetOutEdges().Count);
    }

    [Test]
    public void ReplaceEdgeTest(){
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{2}},
            {2, new List<int>{0}} 
        });
        UndirectedGraph<bool> graphB = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
            {3, new List<int>{4}},
            {4, new List<int>{5}},
            {5, new List<int>{}} 
        });
        Assert.IsFalse(graphA.TryReplaceEdge(graphA.GetEdge("0,1"),new UndirectedEdge<bool>(0,2)));
        Assert.IsFalse(graphA.TryReplaceEdge(graphA.GetEdge("0,1"),graphA.GetEdge("1,2")));

        
        Assert.IsTrue(graphA.TryReplaceEdge(graphA.GetEdge("0,1"),new DirectedEdge<bool>(0,2)));
        Assert.IsFalse(graphA.HasPath(0,1));     

    }
    [Test]
    public void SwapEdgeNodesTest(){
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{2}},
            {2, new List<int>{0}} 
        });
        UndirectedGraph<bool> graphB = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
            {3, new List<int>{4}},
            {4, new List<int>{5}},
            {5, new List<int>{}} 
        });
        Assert.IsTrue(graphA.GetEdge("0,1").TrySwapNodes());
        // Assert.IsFalse(graphA.HasPath(0,1));
        graphA.DebugMsg();

        Assert.IsTrue(graphB.GetEdge("3,4").TrySwapNodes());
       graphB.DebugMsg();     

    }
    [Test]
    public void IncorrectEdgeTest() {
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{2}},
            {2, new List<int>{0}} 
        });
        UndirectedGraph<bool> graphB = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
            {3, new List<int>{4}},
            {4, new List<int>{5}},
            {5, new List<int>{}} 
        });

        Assert.IsFalse(graphA.TryAddEdge(new UndirectedEdge<bool>(0,2)));
        Assert.IsFalse(graphB.TryAddEdge(new DirectedEdge<bool>(3,5)));      

    }
    [Test]
    public void TryAddNodeTest() {
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{2}},
            {2, new List<int>{0}} 
        });
        DirectedGraph<bool> trivialGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {4, new List<int>()}
        });

        Assert.IsTrue(graphA.TryAddNode(new GraphNode<bool>(3)));
        Assert.IsFalse(graphA.TryAddNode(new GraphNode<bool>(1)));
        Assert.IsFalse(graphA.TryAddNode(trivialGraph.GetNode(4)));
    }
    [Test]
    public void TryRemoveNodeTest() {
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{2}},
            {2, new List<int>{0}} 
        });
        DirectedGraph<bool> trivialGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {4, new List<int>()}
        });

        Assert.IsTrue(graphA.TryRemoveNode(graphA.GetNode(0)));
        Assert.IsFalse(graphA.TryAddNode(trivialGraph.GetNode(4)));

    }
    [Test]
    public void TestTrivialGraph() {
        DirectedGraph<bool> trivialGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>()}
        });
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.GetNode(0)},actual:trivialGraph.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.GetNode(0)},actual:trivialGraph.BFS(0));
    }
    [Test]
    public void TestTrivialCycle() {

        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.GetNode(0),trivialCycle.GetNode(1)},actual:trivialCycle.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.GetNode(0),trivialCycle.GetNode(1)},actual:trivialCycle.BFS(0));

        Assert.IsTrue(CycleSolver<bool>.FindCycleFrom(trivialCycle.GetNode(0)));  
    }
    [Test]
    public void TestBFSvsDFS() {

        Assert.AreEqual(
            expected:new List<GraphNode<bool>>{
                childNodes.GetNode(0),
                childNodes.GetNode(2),
                childNodes.GetNode(4),
                childNodes.GetNode(1),
                childNodes.GetNode(3)},
            actual:childNodes.DFS(0));
        Assert.AreEqual(
            expected:new List<GraphNode<bool>>{
                childNodes.GetNode(0),
                childNodes.GetNode(1),
                childNodes.GetNode(2),
                childNodes.GetNode(3),
                childNodes.GetNode(4)},
            actual:childNodes.BFS(0));
        
    }
    [Test]
    public void TestHasPath() {
        Assert.AreEqual(
            expected:false,
            actual:islands.HasPath(1,2));
        Assert.AreEqual(
            expected:true,
            actual:islands.HasPath(1,0));
        Assert.AreEqual(
            expected:true,
            actual:islands.HasPath(2,3));
        Assert.AreEqual(
            expected:false,
            actual:islands.HasPath(0,3));
    }

    [Test]
    public void SortTest() {
        DirectedGraph<bool> unsorted = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{}},
            {1, new List<int>{0}},
            {2, new List<int>{1}}
        });
        var sortedNodes = TopologicalSort<bool>.Sort(unsorted);
        Assert.AreEqual(expected: 0, actual: sortedNodes[unsorted.GetNode(2)]);
    }
}
