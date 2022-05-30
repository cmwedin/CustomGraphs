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
        heap.Push(new Foo("foo 4"), 15);

        Foo outFoo;
        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 3");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 4");

        Foo refFoo = new Foo("foo 5");
        heap.Push(refFoo,23);
        heap.IncreaseKey(refFoo,35);
        heap.DecreaseKey(refFoo,19);
        
        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 5");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 2");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 1");

        Assert.IsFalse(heap.TryPop(out outFoo));
    }
}
