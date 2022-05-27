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
    public class D_aryHeap<THeapType> : MonoBehaviour {
        private GraphNode<float> rootNode;
        private int childCapacity; // ? the D in D-ary
        private RootedTree<float> heapTree = new RootedTree<float>();
        private Dictionary<THeapType,int> objectIDs = new Dictionary<THeapType, int>();
        private Dictionary<int,THeapType> reverseID = new Dictionary<int, THeapType>();
        private int IDcounter = 0;
// * Properties
        public int Size { get => heapTree.Size; }
        public bool isEmpty { get => Size == 0;}

// * Constructors
        public D_aryHeap(int D) {
            childCapacity = D;
        }
        
        public THeapType Peek() {
            return reverseID[rootNode.ID];
        }
        public void Push(THeapType inObject, float key) {
            if(objectIDs.ContainsKey(inObject)) {
                Debug.LogWarning($"Heap already contains object");
            }
            objectIDs.Add(inObject,IDcounter);
            reverseID.Add(IDcounter,inObject);
            IDcounter++;
            if(isEmpty) {
                heapTree.AddNode(new GraphNode<float>(objectIDs[inObject],null,key));
                rootNode = heapTree.RootNode;
                return;
            } else {
                heapTree.TryAddEdge(GetBottomNode(),new GraphNode<float>(objectIDs[inObject],null,key)); 
            }
            SiftUp(inObject);
            if(key < rootNode.Value) {
                //? update root
                rootNode = heapTree.RootNode;
            }
            // throw new NotImplementedException();
        }
        public bool TryPop(out THeapType outObject) {
            if(Size == 0) {
                outObject = default(THeapType);
                return false;
            }
            outObject = Peek(); //placeholder 
            DeleteRoot();
            return true;
        }
        public void DeleteRoot() {
            throw new NotImplementedException();
        }
        public void DeleteAndReplaceRoot(THeapType inObject) {
            throw new NotImplementedException();
        }
        public void IncreaseKey(THeapType obj, float newValue) {
            var node = GetHeapNode(obj);
            if(node.Value >= newValue) {
                Debug.LogWarning("this object already has a greater key in the heap");
                return;
            }
            SiftDown(obj);
            node.SetValue(newValue);
            // throw new NotImplementedException();
        }
        public void DecreaseKey(THeapType obj, float newValue) {
            var node = GetHeapNode(obj);
            if(node.Value <= newValue) {
                Debug.LogWarning("this object already has a lower key in the heap");
                return;
            }
            node.SetValue(newValue);
            SiftUp(obj);
            // throw new NotImplementedException();
        }
        public void DeleteElement(THeapType obj) {
            var objNode = GetHeapNode(obj);
            while(heapTree.GetChildren(objNode).Count != 0) {
                var smallestChild = GetSmallestChild(objNode);
                heapTree.GetEdge($"{objNode.ID},{smallestChild.ID}").TrySwapNodes();
            }
            heapTree.RemoveNode(objNode);     
            // throw new NotImplementedException();
        }
        public void SiftUp(THeapType obj) {
            var objNode = GetHeapNode(obj);
            if( heapTree.GetParentNode(objNode) == null) {
                Debug.LogWarning("object is already the root of the heap");
                return;
            } else while (objNode.Value < heapTree.GetParentNode(objNode).Value) {
                //TODO not implemented
                objNode.GetInEdges()[0].TrySwapNodes();
            }
            throw new NotImplementedException();
        }
        public void SiftDown(THeapType obj) {
            var objNode = GetHeapNode(obj);
            if( heapTree.GetChildren(objNode).Count == 0) {
                Debug.LogWarning("object is already the bottom-most node");
                return;
            } else while (objNode.Value > GetSmallestChild(objNode).Value) {
                //TODO not implemented
                heapTree.GetEdge($"{objNode.ID},{GetSmallestChild(objNode).ID}").TrySwapNodes();
            }
            throw new NotImplementedException();
        }

        private GraphNode<float> GetHeapNode(THeapType obj) {
            return heapTree.GetNode(objectIDs[obj]);
        }
        private GraphNode<float> GetBottomNode() {
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
            int bottomLayer = Mathf.FloorToInt(Mathf.Log(Size + 1,childCapacity) - 1); // ? we add one to the size because we want to know what node to add the next one too
            List<GraphNode<float>> layerNodes = heapTree.GetLayer(bottomLayer);
            int index = 0;
            //TODO NOT IMPLEMENTED
            GraphNode<float> currentNode = layerNodes[index];
            while(heapTree.GetChildren(currentNode).Count == childCapacity) {
                index++;
                if(index >= layerNodes.Count) {throw new Exception($"bottom node not found in layer {bottomLayer}");}
                currentNode = layerNodes[index];
            }
            // throw new NotImplementedException();
            return currentNode;
        }
        private GraphNode<float> GetGreatestChild(GraphNode<float> node) {
            var children = heapTree.GetChildren(node);
            children = children.OrderBy(o => o.Value).ToList();
            return children[0];
        }
        private GraphNode<float> GetSmallestChild(GraphNode<float> node) {
            var children = heapTree.GetChildren(node);
            children = children.OrderByDescending(o => o.Value).ToList();
            return children[0];
        }
        
    }
}
