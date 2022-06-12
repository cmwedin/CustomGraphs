using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SadSapphicGames.CustomGraphs;
using System;
using System.Linq;

namespace SadSapphicGames.DataStructures{
    // ! IMPORTANT NOTE : 
    // ! this is not intended to be an efficient implementation of a heap 
    // ! (well it is hopefully time efficient but a space efficient implementation would just use an array)
    // ! but rather use case to identify any additional behavior that might be needed for a extensibility of the tree class
    // ! and provide a d-ary heap for myself to use in dijkstra and prim MST that could be replaced with a more efficient one in the future. 
    // ? since this isn't intended for general use and I mostly need it for pathfinding I'm only going to implement a min heap 
    public class D_aryHeap<THeapType> {
        private GraphNode<float> rootNode;
        private readonly int childCapacity; // ? the D in D-ary
        private RootedTree<float> heapTree = new RootedTree<float>();
        private Dictionary<THeapType,int> objectIDs = new Dictionary<THeapType, int>();
        private Dictionary<int,THeapType> reverseObjID = new Dictionary<int, THeapType>();
        private int IDcounter = 0;
// * Properties
        public int Size { get => heapTree.Size; }
        public bool isEmpty { get => Size == 0;}

// * Constructors
        public D_aryHeap(int D) {
            childCapacity = D;
        }
        public D_aryHeap(int D, Dictionary<THeapType, float> initialValues) {
            childCapacity = D;
            foreach (var obj in initialValues.Keys) {
                Push(obj,initialValues[obj]);
            }
        }
        
        public THeapType Peek() {
            if(isEmpty) {
                Debug.LogWarning("Peeked empty heap, returning default value");
                return default(THeapType);
            }
            return reverseObjID[rootNode.ID];
        }
        public void Push(THeapType inObject, float key) {
            if(objectIDs.ContainsKey(inObject)) {
                Debug.LogWarning($"Heap already contains object");
            }
            objectIDs.Add(inObject,IDcounter);
            reverseObjID.Add(IDcounter,inObject);
            IDcounter++;
            if(isEmpty) {
                Debug.Log("adding first node to the heap");
                heapTree.TryAddNode(new GraphNode<float>(objectIDs[inObject],key));
                rootNode = heapTree.FindRootNode();
                return;
            } else {
                if(!heapTree.TryAddEdge(new UndirectedEdge<float>(GetBottomNode().ID, objectIDs[inObject]))){
                    throw new SystemException("failed to add edge to next node");
                }; 
                heapTree.GetNode(objectIDs[inObject]).SetValue(key);
            }
            SiftUp(inObject);
            if(key < rootNode.Value) {
                //? update root
                rootNode = heapTree.FindRootNode();
            }
            Debug.Log("current heap tree info");
            heapTree.DebugMsg();
            // throw new NotImplementedException();
        }
        public bool TryPop(out THeapType outObject) {
            if(isEmpty) {
                outObject = default(THeapType);
                return false;
            }
            outObject = Peek(); //placeholder 
            DeleteElement(outObject);
            Debug.Log("current heap tree info");
            heapTree.DebugMsg();
            return true;
        }

        // public bool TryPopThenPush(THeapType inObject, float key, out THeapType outObject) {
        //     //TODO
        //     throw new NotImplementedException();
        //     if(isEmpty) {
        //         outObject = default(THeapType);
        //         Push(inObject, key);
        //         return false;
        //     }
        //     outObject = Peek();
        //     ReplaceRoot(inObject,key);
        //     return true;
        // }

        // private void ReplaceRoot(THeapType inObject, float key) {
        //     throw new NotImplementedException();
        //     objectIDs.Add(inObject,IDcounter);
        //     reverseObjID.Add(IDcounter,inObject);
        //     IDcounter++;
        //     //TODO if I want to add PopThenPush I just need to finish this function
        //     //TODO seems like it might be more specialized than a class that admittedly isn't intended for broad use might warrant however
        //     //TODO the reasoning for implementing something like this is that it eliminates the need to sort the heap twice
        //     //TODO once when popping the root
        //     //TODO and once when pushing the new object
        // }

        public void IncreaseKey(THeapType obj, float newValue) {
            var objNode = GetHeapNode(obj);
            if(objNode.Value >= newValue) {
                Debug.LogWarning("this object already has a greater key in the heap");
                return;
            }
            objNode.SetValue(newValue);
            SiftDown(obj);
        }
        public void DecreaseKey(THeapType obj, float newValue) {
            var objNode = GetHeapNode(obj);
            if(objNode.Value <= newValue) {
                Debug.LogWarning("this object already has a lower key in the heap");
                return;
            }
            objNode.SetValue(newValue);
            SiftUp(obj);
            if(objNode.Value < rootNode.Value) {rootNode = heapTree.FindRootNode(); }
        }
        // private void DeleteRoot() {
        //     GraphNode<float> newRoot = GetSmallestChild(rootNode);
        //     DeleteElement(reverseObjID[rootNode.ID]);
        //     rootNode = newRoot;
        // }
        private void DeleteElement(THeapType obj) {
            GraphNode<float> newRoot = null;
            var objNode = GetHeapNode(obj);
            if(objNode == rootNode) {
                newRoot = GetSmallestChild(rootNode);
            }
            // if(objNode == rootNode) {throw new Exception("The root node must be deleted through DeleteRoot() not DeleteElement(THeapType obj)");}
            while(heapTree.GetChildren(objNode).Count != 0) {
                var smallestChild = GetSmallestChild(objNode);
                heapTree.GetEdge($"{objNode.ID},{smallestChild.ID}").TrySwapNodes();
            }
            heapTree.TryRemoveNode(objNode);
            if(newRoot != null) rootNode = newRoot;     
        }
        private void SiftUp(THeapType obj) {
            var objNode = GetHeapNode(obj);
            // if(objNode == null) throw new SystemException("this shouldn't happen");
            if( heapTree.GetParentNodeOf(objNode) == null) {
                Debug.LogWarning("object is already the root of the heap");
                return;
            } else {
                while (heapTree.GetParentNodeOf(objNode) != null && objNode.Value < heapTree.GetParentNodeOf(objNode).Value) {
                    objNode.GetInEdges()[0].TrySwapNodes();
                }
            }
            // throw new NotImplementedException();
        }
        private void SiftDown(THeapType obj) {
            var objNode = GetHeapNode(obj);
            if( heapTree.GetChildren(objNode).Count == 0) {
                Debug.LogWarning("object is already the bottom-most node");
                return;
            } else while (objNode.Value > GetSmallestChild(objNode).Value) {
                heapTree.GetEdge($"{objNode.ID},{GetSmallestChild(objNode).ID}").TrySwapNodes();
            }
            // throw new NotImplementedException();
        }

        private GraphNode<float> GetHeapNode(THeapType obj) {
            if(isEmpty) {
                Debug.LogWarning("There are no nodes in the heap, returning null");
                return null;
            }
            return heapTree.GetNode(objectIDs[obj]);
        }
        private GraphNode<float> GetBottomNode() {
            if(isEmpty) {
                Debug.LogWarning("There are no nodes in the heap, returning null");
                return null;
            }
            // ? we know the graph has size N nodes
            // ? and every node but the bottom node should have d children
            // ? if N > d + 1 the bottom is a child of the root
                // ? here note that layer k has d^k elements 
                // ? hence we can generalize that if N > Sum(from k=0 to K, d^k) the bottom node is bellow layer k
            //? more precisely we can conclude that if Sum(from k=0 to K+1, d^k) > N > Sum(from k=0 to K, d^k)
                // ? or more clearly d^k+1 > N or k > log_d(N)-1
                    // ? sanity check: if N = 1 evaluates to -1, 2 to 0, 3 to .58, 4 to 1,...
                        //? rounding down to int gives 1:-1, 2:0, 3:0, 4:1, 5:1, 6:1, 7:1, 8:2, 9:2...
            // ! hence FloorToInt(log_d(N)-1) gives the layer the bottom node should be in when attaching node #N to the heap
            int bottomLayer = Mathf.FloorToInt(Mathf.Log(Size + 1, childCapacity) - 1); // ? we add one to the size because we want to know what node to add the next one too
            Debug.Log($"the bottom node is in layer {bottomLayer}");
            List<GraphNode<float>> layerNodes = heapTree.GetLayer(bottomLayer);
            int index = 0;
            GraphNode<float> currentNode = layerNodes[index];
            while(heapTree.GetChildren(currentNode).Count == childCapacity) {
                index++;
                if(index >= layerNodes.Count) {throw new Exception($"bottom node not found in layer {bottomLayer}");}
                currentNode = layerNodes[index];
            }
            // throw new NotImplementedException();
            Debug.Log($"the bottom node is {currentNode.ID} ");
            return currentNode;
        }
        private GraphNode<float> GetGreatestChild(GraphNode<float> node) {
            var children = heapTree.GetChildren(node);
            if(children.Count == 0) return null;
            children = children.OrderByDescending(o => o.Value).ToList();
            return children[0];
        }
        private GraphNode<float> GetSmallestChild(GraphNode<float> node) {
            var children = heapTree.GetChildren(node);
            if(children.Count == 0) return null;
            children = children.OrderBy(o => o.Value).ToList();
            Debug.Log($"the smallest child of node {node.ID} is node {children[0].ID} with key {children[0].Value}");
            return children[0];
        }
        
    }
}
