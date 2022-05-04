using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class GraphTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestTrivialGraph() {
        Graph<bool> trivialGraph = new Graph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>()}
        });
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.Nodes[0]},actual:trivialGraph.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.Nodes[0]},actual:trivialGraph.BFS(0));
    }
    [Test]
    public void TestTrivialCycle() {
        Graph<bool> trivialCycle = new Graph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}},
            {1, new List<int>{0}}
        });
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.Nodes[0],trivialCycle.Nodes[1]},actual:trivialCycle.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.Nodes[0],trivialCycle.Nodes[1]},actual:trivialCycle.BFS(0));
        
    }
    [Test]
    public void TestBFSvsDFS() {
        Graph<bool> graph = new Graph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{3}},
            {2, new List<int>{4}},
            {3, new List<int>{}},
            {4, new List<int>{}}
        });
        Assert.AreEqual(
            expected:new List<GraphNode<bool>>{
                graph.Nodes[0],
                graph.Nodes[2],
                graph.Nodes[4],
                graph.Nodes[1],
                graph.Nodes[3],},
            actual:graph.DFS(0));
        Assert.AreEqual(
            expected:new List<GraphNode<bool>>{
                graph.Nodes[0],
                graph.Nodes[1],
                graph.Nodes[2],
                graph.Nodes[3],
                graph.Nodes[4],},
            actual:graph.BFS(0));
        
    }
}
