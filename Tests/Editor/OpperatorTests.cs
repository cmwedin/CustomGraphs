using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;
internal class Foo {
    public int i = 1;
    public Bar bar = new Bar();
}
internal class Bar {
    public int i = 2;
}

public class OperatorTests {
    [Test]
    public void CopyTests() {
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{1}}, 
            {1, new List<int>{}} 
        });
        UndirectedGraph<bool> graphB = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
            {2, new List<int>{}} ,
            {3, new List<int>{2}} 

        });
        var graphA2 = graphA.Copy();
        var graphB2 = graphB.Copy();
        Assert.IsTrue(graphA2 is DirectedGraph<bool>);
        Assert.IsFalse(graphA2 is UndirectedGraph<bool>);
        Assert.IsTrue(graphB2 is UndirectedGraph<bool>);
        Assert.IsFalse(graphB2 is DirectedGraph<bool>);
        Assert.IsFalse(graphA.GetNode(0) == graphA2.GetNode(0));
        Assert.IsTrue(graphA.HasPath(0,1));
        Assert.IsTrue(graphA2.HasPath(0,1));
        Assert.IsFalse(graphB.GetNode(2) == graphB2.GetNode(2));
        Assert.IsTrue(graphB2.HasPath(2,3));
    }
    [Test]
    public void plusNodeValTypeTest() {
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{}} 
        });
        DirectedGraph<bool> graphB = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {1, new List<int>{}} 
        });
        DirectedGraph<bool> result1 = (DirectedGraph<bool>)(graphA + graphB.GetNode(1));
        result1.GetNode(1).SetValue(true);
        DirectedGraph<bool> result2 = (DirectedGraph<bool>)(graphB + graphA.GetNode(0));
        result2.GetNode(1).SetValue(true);
        Assert.IsFalse(result1 == graphA);
        Assert.IsFalse(result1.GetNode(1) == graphB.GetNode(1));
        Assert.IsFalse(result2 == graphB);
        Assert.IsFalse(result2.GetNode(0) == graphA.GetNode(0));
        Assert.IsFalse(graphB.GetNode(1).Value);
        Assert.IsTrue(result1.GetNode(1).Value);
        Assert.IsTrue(result2.GetNode(1).Value);
    }
    [Test]
    public void plusNodeRefTypeTest() {
        DirectedGraph<Foo> graphA = new DirectedGraph<Foo>( new Dictionary<int, List<int>> {
            {0, new List<int>{}} 
        });
        graphA.GetNode(0).SetValue(new Foo());
        DirectedGraph<Foo> graphB = new DirectedGraph<Foo>( new Dictionary<int, List<int>> {
            {1, new List<int>{}} 
        });
        graphB.GetNode(1).SetValue(new Foo());
        DirectedGraph<Foo> result1 = (DirectedGraph<Foo>)(graphA + graphB.GetNode(1));
        result1.GetNode(1).Value.i = 3;
        DirectedGraph<Foo> result2 = (DirectedGraph<Foo>)(graphB + graphA.GetNode(0));
        result2.GetNode(0).Value.bar.i = 3;
        Assert.IsFalse(result1 == graphA);
        Assert.IsFalse(result1.GetNode(1) == graphB.GetNode(1));
        Assert.IsFalse(result2 == graphB);
        Assert.IsFalse(result2.GetNode(0) == graphA.GetNode(0));
        Assert.AreEqual(expected: 1, actual: graphB.GetNode(1).Value.i);
        Assert.AreEqual(expected: 3, actual: result1.GetNode(1).Value.i);
        Assert.AreEqual(expected: 2, actual: graphA.GetNode(0).Value.bar.i);
        Assert.AreEqual(expected: 3, actual: result2.GetNode(0).Value.bar.i);
    }
}
