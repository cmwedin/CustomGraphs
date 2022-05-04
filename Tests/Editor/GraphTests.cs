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
    public void TestDFS() {
        Graph<bool> trivialGraph = new Graph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>()}
        });
        Assert.AreEqual(expected:new List<int>{0},actual:trivialGraph.DFS(0));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GraphTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
