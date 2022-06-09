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
    public void ReplaceEdgeTest() {
        Tree<bool> tree = new Tree<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{3}},
            {2, new List<int>{4}},
            {3, new List<int>{}},
            {4, new List<int>{}}
        });

        Assert.IsTrue(tree.TryReplaceEdge(tree.GetEdge("2,4"),new UndirectedEdge<bool>(1,4)));
        Assert.IsFalse(tree.TryReplaceEdge(tree.GetEdge("1,4"),new UndirectedEdge<bool>(1,5)));
        // Assert.IsFalse(tree.TryReplaceEdge(tree.GetEdge("1,4"), new UndirectedEdge<bool>(2,3)));
        Assert.Throws<System.Exception>(delegate{tree.TryReplaceEdge(tree.GetEdge("1,4"), new UndirectedEdge<bool>(2,3));});
    }
        [Test]
    public void SwapNodesTest() {
        Tree<bool> tree = new Tree<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1,2}},
            {1, new List<int>{3}},
            {2, new List<int>{4}},
            {3, new List<int>{}},
            {4, new List<int>{}}
        });

        Assert.IsTrue(tree.GetEdge("2,4").TrySwapNodes());
        tree.DebugMsg();

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

        Assert.IsTrue(rootedTree.TryAddEdge(3,5));

        Assert.AreEqual(actual:rootedTree.RootNode, expected: rootedTree.GetNode(0));
        Assert.AreEqual(
            actual:rootedTree.GetLayer(0),
            expected: new List<GraphNode<bool>> {
                rootedTree.GetNode(0)
            }
        );
        Assert.AreEqual(
            actual:rootedTree.GetLayer(1),
            expected: new List<GraphNode<bool>> {
                rootedTree.GetNode(1),
                rootedTree.GetNode(2)
            }
        );
        Assert.AreEqual(
            actual:rootedTree.GetLayer(2),
            expected: new List<GraphNode<bool>> {
                rootedTree.GetNode(3),
                rootedTree.GetNode(4)
            }
        );
    }
}
