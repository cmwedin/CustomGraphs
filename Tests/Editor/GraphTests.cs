using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class GraphTests
{
    // * Graphs to do tests with
    Graph<bool> trivialGraph = new Graph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>()}
    });
    Graph<bool> trivialCycle = new Graph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1}},
        {1, new List<int>{0}}
    });
    Graph<bool> childNodes = new Graph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1,2}},
        {1, new List<int>{3}},
        {2, new List<int>{4}},
        {3, new List<int>{}},
        {4, new List<int>{}}
    });
    Graph<bool> islands = new Graph<bool> ( new Dictionary<int, List<int>> {
        {0, new List<int>{1}},
        {1, new List<int>{0}},
        {2, new List<int>{3}},
        {3, new List<int>{2}}
    });
    Graph<bool> bottleNeck = new Graph<bool> ( new Dictionary<int, List<int>> {
        {0, new List<int>{3}},
        {1, new List<int>{3}},
        {2, new List<int>{3}},
        {3, new List<int>{4,5}},
        {4, new List<int>{5}},
        {5, new List<int>{3}}
    });
    // A Test behaves as an ordinary method
    [Test]
    public void TestTrivialGraph() {

        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.Nodes[0]},actual:trivialGraph.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialGraph.Nodes[0]},actual:trivialGraph.BFS(0));
    }
    [Test]
    public void TestTrivialCycle() {

        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.Nodes[0],trivialCycle.Nodes[1]},actual:trivialCycle.DFS(0));
        Assert.AreEqual(expected:new List<GraphNode<bool>>{trivialCycle.Nodes[0],trivialCycle.Nodes[1]},actual:trivialCycle.BFS(0));
        
    }
    [Test]
    public void TestBFSvsDFS() {

        Assert.AreEqual(
            expected:new List<GraphNode<bool>>{
                childNodes.Nodes[0],
                childNodes.Nodes[2],
                childNodes.Nodes[4],
                childNodes.Nodes[1],
                childNodes.Nodes[3]},
            actual:childNodes.DFS(0));
        Assert.AreEqual(
            expected:new List<GraphNode<bool>>{
                childNodes.Nodes[0],
                childNodes.Nodes[1],
                childNodes.Nodes[2],
                childNodes.Nodes[3],
                childNodes.Nodes[4]},
            actual:childNodes.BFS(0));
        
    }
    [Test]
    public void TestConnectedComponents() {
        Assert.AreEqual(
            expected:new List<List<GraphNode<bool>>>{
                new List<GraphNode<bool>> {islands.Nodes[3],islands.Nodes[2]},
                new List<GraphNode<bool>> {islands.Nodes[1],islands.Nodes[0]}
            },
            actual:islands.GetConnectedComponents());
    }
}
