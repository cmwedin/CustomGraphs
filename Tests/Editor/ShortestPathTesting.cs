using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class ShortestPathTests {
    // A Test behaves as an ordinary method
    [Test]
    public void DAGShortestPathTest() {
        
        DirectedGraph<bool> DAGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{2,4}},
            {2, new List<int>{3}},
            {3, new List<int>{4}},
            {4, new List<int>{}} 

        });
        var shortestPathsCosts = ShortestPath<bool>.DAGShortestPath(DAGraph, 0,out var shortestPaths);

        Assert.AreEqual(expected: 3, actual: shortestPathsCosts[3]);
        Assert.AreEqual(expected: 2, actual: shortestPathsCosts[4]);
        Assert.AreEqual(
            expected: "0,1|1,2|2,3",
            actual: shortestPaths[3]
        );
        Assert.AreEqual(
            expected: "0,1|1,4",
            actual: shortestPaths[4]
        );
    }
    [Test]
    public void DAGShortestPathIllegalArgumentTest () {
        DirectedGraph<bool> trivialCycle = new DirectedGraph<bool>( new Dictionary<int, List<int>>{
            {0, new List<int>{1}},
            {1, new List<int>{0}}
        });
        Assert.Throws<SadSapphicGames.CustomGraphs.NotDAGException>(delegate{ ShortestPath<bool>.DAGShortestPath(trivialCycle,0,out var empty);});
    }
    [Test]
    public void DAGEqualCostPaths() {
        DirectedGraph<bool> graph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0,new List<int>{1,2}},
            {1,new List<int>{3}},
            {2,new List<int>{3}},
            {3,new List<int>{}},
        });
        var shortestPathsCosts = ShortestPath<bool>.DAGShortestPath(graph,0,out var shortestPaths);
        Assert.AreEqual(expected:2, actual:shortestPathsCosts[3]);
        Assert.AreEqual(
            actual: shortestPaths[3],  
            expected: "0,1|1,3");
    }
    [Test]
    public void DijkstraTest() {
        DirectedGraph<bool> directedGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{2,3}},
            {2, new List<int>{3}},
            {3,new List<int>{4,0}},
            {4,new List<int>{1}}
        });
        UndirectedGraph<bool> undirectedGraph = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{2,3}},
            {2, new List<int>{3}},
            {3,new List<int>{4,0}},
            {4,new List<int>{1}}
        });

        // ? Directed Graph Test
        var bestPathCost = ShortestPath<bool>.DijkstraShortestPath(directedGraph.GetNode(0), out var bestPathIDs);
        Assert.AreEqual(expected:0, actual: bestPathCost[directedGraph.GetNode(0)]);
        Assert.AreEqual(expected:1, actual: bestPathCost[directedGraph.GetNode(1)]);
        Assert.AreEqual(expected:1, actual: bestPathCost[directedGraph.GetNode(2)]);
        Assert.AreEqual(expected:2, actual: bestPathCost[directedGraph.GetNode(3)]);
        Assert.AreEqual(expected:3, actual: bestPathCost[directedGraph.GetNode(4)]);
        Assert.AreEqual(
            expected:"",
            actual:bestPathIDs[directedGraph.GetNode(0)]
        );
        Assert.AreEqual(
            expected:"0,1",
            actual:bestPathIDs[directedGraph.GetNode(1)]
        );
        Assert.AreEqual(
            expected:"0,2",
            actual:bestPathIDs[directedGraph.GetNode(2)]
        );
        Assert.AreEqual(
            expected:"0,1|1,3",
            actual:bestPathIDs[directedGraph.GetNode(3)]
        );
        Assert.AreEqual(
            expected:"0,1|1,3|3,4",
            actual:bestPathIDs[directedGraph.GetNode(4)]
        );

        //? UndirectedGraphTest
        bestPathCost = ShortestPath<bool>.DijkstraShortestPath(undirectedGraph.GetNode(0), out bestPathIDs);
        Assert.AreEqual(expected:0, actual: bestPathCost[undirectedGraph.GetNode(0)]);
        Assert.AreEqual(expected:1, actual: bestPathCost[undirectedGraph.GetNode(1)]);
        Assert.AreEqual(expected:1, actual: bestPathCost[undirectedGraph.GetNode(2)]);
        Assert.AreEqual(expected:1, actual: bestPathCost[undirectedGraph.GetNode(3)]);
        Assert.AreEqual(expected:2, actual: bestPathCost[undirectedGraph.GetNode(4)]);
        Assert.AreEqual(
            expected:"",
            actual:bestPathIDs[undirectedGraph.GetNode(0)]
        );
        Assert.AreEqual(
            expected:"0,1",
            actual:bestPathIDs[undirectedGraph.GetNode(1)]
        );
        Assert.AreEqual(
            expected:"0,2",
            actual:bestPathIDs[undirectedGraph.GetNode(2)]
        );
        Assert.AreEqual(
            expected:"3,0",
            actual:bestPathIDs[undirectedGraph.GetNode(3)]
        );
        Assert.AreEqual(
            expected:"0,1|4,1",
            actual:bestPathIDs[undirectedGraph.GetNode(4)]
        );
    }
}
