using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class TreeTests
{
    [Test]
    public void TestVerifyTree() {
        UndirectedGraph<bool> undirectedTree = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{3}},
            {2, new List<int>{4}},
            {3, new List<int>{}},
            {4, new List<int>{}}
        });
        Assert.IsTrue(Tree<bool>.VerifyTree(undirectedTree));
        Assert.IsFalse(Tree<bool>.VerifyTree(
            undirectedTree + new UndirectedEdge<bool>(3,4)));
    }
    [Test]
    public void AddEdgeTest() {
        Tree<bool> actualTree = new Tree<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{3}},
            {2, new List<int>{4}},
            {3, new List<int>{}},
            {4, new List<int>{}}
        });
        Assert.IsFalse(actualTree.TryAddEdge(0,3));
        Assert.IsFalse(actualTree.TryAddEdge(10,20));

        Assert.IsTrue(actualTree.TryAddEdge(3,5));
        Assert.IsTrue(actualTree.TryAddEdge(6,4));
    }
    [Test]
    public void RootedTreeTest() {
        RootedTree<bool> rootedTree = new RootedTree<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{3}},
            {2, new List<int>{4}},
            {3, new List<int>{}},
            {4, new List<int>{}}
        });
        Assert.IsFalse(rootedTree.TryAddEdge(0,3));
        Assert.IsFalse(rootedTree.TryAddEdge(10,20));
        Assert.IsFalse(rootedTree.TryAddEdge(6,4));

        Assert.AreEqual(actual:rootedTree.RootNode, expected: rootedTree.GetNode(0));
        Assert.IsTrue(rootedTree.TryAddEdge(3,5));
    }
}
