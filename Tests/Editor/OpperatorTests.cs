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

public class OpperatorTests {
    [Test]
    public void plusNodeValTypeTest() {
        DirectedGraph<bool> graphA = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {0, new List<int>{}} 
        });
        DirectedGraph<bool> graphB = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
            {1, new List<int>{}} 
        });
        DirectedGraph<bool> result1 = (DirectedGraph<bool>)(graphA + graphB.Nodes[1]);
        result1.Nodes[1].SetValue(true);
        DirectedGraph<bool> result2 = (DirectedGraph<bool>)(graphB + graphA.Nodes[0]);
        result2.Nodes[1].SetValue(true);
        Assert.IsFalse(result1 == graphA);
        Assert.IsFalse(result1.Nodes[1] == graphB.Nodes[1]);
        Assert.IsFalse(result2 == graphB);
        Assert.IsFalse(result2.Nodes[0] == graphA.Nodes[0]);
        Assert.IsFalse(graphB.Nodes[1].Value);
        Assert.IsTrue(result1.Nodes[1].Value);
        Assert.IsTrue(result2.Nodes[1].Value);
    }
    [Test]
    public void plusNodeRefTypeTest() {
        DirectedGraph<Foo> graphA = new DirectedGraph<Foo>( new Dictionary<int, List<int>> {
            {0, new List<int>{}} 
        });
        graphA.Nodes[0].SetValue(new Foo());
        DirectedGraph<Foo> graphB = new DirectedGraph<Foo>( new Dictionary<int, List<int>> {
            {1, new List<int>{}} 
        });
        graphB.Nodes[1].SetValue(new Foo());
        DirectedGraph<Foo> result1 = (DirectedGraph<Foo>)(graphA + graphB.Nodes[1]);
        result1.Nodes[1].Value.i = 3;
        DirectedGraph<Foo> result2 = (DirectedGraph<Foo>)(graphB + graphA.Nodes[0]);
        result2.Nodes[0].Value.bar.i = 3;
        Assert.IsFalse(result1 == graphA);
        Assert.IsFalse(result1.Nodes[1] == graphB.Nodes[1]);
        Assert.IsFalse(result2 == graphB);
        Assert.IsFalse(result2.Nodes[0] == graphA.Nodes[0]);
        Assert.AreEqual(expected: 1, actual: graphB.Nodes[1].Value.i);
        Assert.AreEqual(expected: 3, actual: result1.Nodes[1].Value.i);
        Assert.AreEqual(expected: 2, actual: graphA.Nodes[0].Value.bar.i);
        Assert.AreEqual(expected: 3, actual: result2.Nodes[0].Value.bar.i);
    }
}
