using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.DataStructures;
public class Foo {
    public string bar;

    public Foo(string _bar) {
        bar = _bar;
    }
}
public class HeapTest
{
    [Test]
    public void PushPopTest() {
        D_aryHeap<Foo> heap = new D_aryHeap<Foo>(2);
        heap.Push(new Foo("foo 1"), 30);
        heap.Push(new Foo("foo 2"), 20);
        heap.Push(new Foo("foo 3"), 10);
        heap.Push(new Foo("foo 4"), 10);

        Foo outFoo;
        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 3");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 4");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 2");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 1");

        Assert.IsFalse(heap.TryPop(out outFoo));
    }
    [Test]
    public void ChangeKeyTest() { 
        D_aryHeap<int> heap = new D_aryHeap<int>(2, new Dictionary<int, float>{
            {1,10},
            {2,20},
            {3,30}
        });

        heap.IncreaseKey(1,50);
        heap.DecreaseKey(3,10);
        int outInt;
        Assert.IsTrue(heap.TryPop(out outInt));
        Assert.AreEqual(expected:3, actual:outInt); 
        Assert.IsTrue(heap.TryPop(out outInt));
        Assert.AreEqual(expected:2, actual:outInt); 
        Assert.IsTrue(heap.TryPop(out outInt));
        Assert.AreEqual(expected:1, actual:outInt); 
    }
}
