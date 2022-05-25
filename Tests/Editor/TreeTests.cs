using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class TreeTests
{
    UndirectedGraph<bool> undirectedTree = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1,2}},
        {1, new List<int>{3}},
        {2, new List<int>{4}},
        {3, new List<int>{}},
        {4, new List<int>{}}
    });
    Tree<bool> actualTree = new Tree<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1,2}},
        {1, new List<int>{3}},
        {2, new List<int>{4}},
        {3, new List<int>{}},
        {4, new List<int>{}}
    });
    [Test]
    public void TestVerifyTree() {
        Assert.IsTrue(Tree<bool>.VerifyTree(undirectedTree));
        Assert.IsFalse(Tree<bool>.VerifyTree(
            undirectedTree + new UndirectedEdge<bool>(3,4)));
    }

    public void AddEdgeTest() {
        Assert.IsFalse(actualTree.TryAddEdge(0,3));
    }
}
