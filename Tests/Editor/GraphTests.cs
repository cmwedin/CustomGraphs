using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class GraphTests
{
    // * Graphs to do tests with
    DirectedGraph<bool> trivialGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>()}
    });
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
    DirectedGraph<bool> scc = new DirectedGraph<bool> ( new Dictionary<int, List<int>> { 
        {0, new List<int>{1}},
        {1, new List<int>{2}},
        {2, new List<int>{0}}, //? end of scc 1
        {3, new List<int>{0,2,4}},
        {4, new List<int>{5}},
        {5, new List<int>{0,3}}, //? end of scc 2
        {6, new List<int>{4,7}},
        {7, new List<int>{6,5}}, //? end of scc 3
    });
    DirectedGraph<bool> bottleNeck = new DirectedGraph<bool> ( new Dictionary<int, List<int>> {
        {0, new List<int>{3}},
        {1, new List<int>{3}},
        {2, new List<int>{3}},
        {3, new List<int>{4,5}},
        {4, new List<int>{5}},
        {5, new List<int>{3}}
    });
    DirectedGraph<bool> unsorted = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{}},
        {1, new List<int>{0}},
        {2, new List<int>{1}}
    });
    // A Test behaves as an ordinary method
    [Test]
    public void TestTrivialGraph() {

        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.GetNode(0)},actual:trivialGraph.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.GetNode(0)},actual:trivialGraph.BFS(0));
    }
    [Test]
    public void TestTrivialCycle() {

        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.GetNode(0),trivialCycle.GetNode(1)},actual:trivialCycle.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.GetNode(0),trivialCycle.GetNode(1)},actual:trivialCycle.BFS(0));
        
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
    public void TestConnectedComponents() {
        Assert.AreEqual(
            expected:new List<List<GraphNode<bool>>>{
                new List<GraphNode<bool>> {islands.GetNode(3),islands.GetNode(2)},
                new List<GraphNode<bool>> {islands.GetNode(1),islands.GetNode(0)}
            },
            actual:islands.GetConnectedComponents());
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
    public void TarjanTest() {
        var tarjanSolution = TarjanSCCSolver<bool>.Solve(scc);
        Assert.AreEqual(
            expected: 3,
            // actual: solution.Values.Count
            actual: tarjanSolution.Count
        );
        Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(2),scc.GetNode(1),scc.GetNode(0)},
            actual: tarjanSolution[0]
        );
        Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(5),scc.GetNode(4),scc.GetNode(3)},
            actual: tarjanSolution[1]
        );
        Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(7),scc.GetNode(6)},
            actual: tarjanSolution[2]
        );
    }

    [Test]
    public void DAGTest() {
        Assert.IsTrue(TarjanSCCSolver<bool>.CheckDAG(childNodes));
        Assert.IsFalse(TarjanSCCSolver<bool>.CheckDAG(scc));
    }
    [Test]
    public void SortTest() {
        var sortedNodes = TopologicalSort<bool>.Sort(unsorted);
        Assert.AreEqual(expected: unsorted.GetNode(2), actual: sortedNodes[0]);
    }
}
