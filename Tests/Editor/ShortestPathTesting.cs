using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class ShortestPathTests {
    DirectedGraph<bool> DAGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1}}, 
        {1, new List<int>{2,4}},
        {2, new List<int>{3}},
        {3, new List<int>{4}},
        {4, new List<int>{}} 

    });
    // A Test behaves as an ordinary method
    [Test]
    public void DAGShortestPathTest() {
        var shortestPathsCosts = ShortestPath<bool>.DAGShortestPath(DAGraph, DAGraph.GetNode(0),out var shortestPaths);

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
}
