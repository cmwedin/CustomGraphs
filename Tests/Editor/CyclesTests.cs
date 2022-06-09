using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SadSapphicGames.CustomGraphs;

public class CyclesTests
{
    DirectedGraph<bool> trivialGraph = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>()}
    });
    DirectedGraph<bool> trivialCycle = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1}},
        {1, new List<int>{0}}
    });
    DirectedGraph<bool> scc = new DirectedGraph<bool> ( new Dictionary<int, List<int>> { 
        {0, new List<int>{1}},
        {1, new List<int>{2}},
        {2, new List<int>{0}}, //? end of scc 1
        {3, new List<int>{0,2,4}},
        {4, new List<int>{5}},
        {5, new List<int>{0,3}}, //? end of scc 2
        {6, new List<int>{4,7}},
        {7, new List<int>{6,5}}, //? end of scc 3
    });
    DirectedGraph<bool> acyclic = new DirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1,2}},
        {1, new List<int>{3}},
        {2, new List<int>{4}},
        {3, new List<int>{}},
        {4, new List<int>{}}
    });

    UndirectedGraph<bool> undirectedCycle = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1,2}},
        {1, new List<int>{2}},
        {2, new List<int>{}}
    } );
    UndirectedGraph<bool> DirectedCycle = new UndirectedGraph<bool>( new Dictionary<int, List<int>> {
        {0, new List<int>{1}},
        {1, new List<int>{2}},
        {2, new List<int>{0}},
        {3, new List<int>{0}}
    } );
    [Test]
    public void DAGTest() {
        Assert.IsTrue(TarjanSCCSolver<bool>.CheckDAG(acyclic));
        Assert.IsFalse(TarjanSCCSolver<bool>.CheckDAG(scc));
    }
    [Test]
    public void TarjanTest() {
        var tarjanSolution = TarjanSCCSolver<bool>.Solve(scc);
        Assert.AreEqual(
            expected: 3,
            // actual: solution.Values.Count
            actual: tarjanSolution.Count
        );
        Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(2),scc.GetNode(1),scc.GetNode(0)},
            actual: tarjanSolution[0]
        );
        Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(5),scc.GetNode(4),scc.GetNode(3)},
            actual: tarjanSolution[1]
        );
        Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(7),scc.GetNode(6)},
            actual: tarjanSolution[2]
        );
    }
    [Test]
    public void CycleSolverTest() {
        Assert.IsTrue(CycleSolver<bool>.FindCycleFrom(scc.GetNode(1)));
        Assert.IsTrue(CycleSolver<bool>.FindCycleFrom(scc.GetNode(4)));
        Assert.IsTrue(CycleSolver<bool>.FindCycleFrom(scc.GetNode(6)));
        Assert.IsTrue(CycleSolver<bool>.FindCycleFrom(undirectedCycle.GetNode(0)));
        Assert.IsTrue(CycleSolver<bool>.FindCycleFrom(DirectedCycle.GetNode(3)));
        Assert.IsFalse(CycleSolver<bool>.FindCycleFrom(acyclic.GetNode(0)));
    }
    [Test]
    public void CycleCallbackTest() {
            CycleSolver<bool>.FindCycleFrom(scc.GetNode(0), out var callback);
            Assert.AreEqual(
            expected: new List<GraphNode<bool>> {scc.GetNode(0),scc.GetNode(1),scc.GetNode(2)},
            actual: callback
        );
    }
}
