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
    public void publicTestPushPopTest() {
        D_aryHeap<Foo> heap = new D_aryHeap<Foo>(2);
        heap.Push(new Foo("foo 1"), 3);
        heap.Push(new Foo("foo 2"), 2);
        heap.Push(new Foo("foo 3"), 1);

        Foo outFoo;
        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 3");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 2");

        Assert.IsTrue(heap.TryPop(out outFoo));
        Assert.AreEqual(actual:outFoo.bar, expected: "foo 1");
    }
}
