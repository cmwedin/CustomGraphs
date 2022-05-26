using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SadSapphicGames.CustomGraphs;
using System;

namespace SadSapphicGames.DataStructures{
    // ! IMPORTANT NOTE : 
    // ! this is not intended to be an efficient implementation of a heap 
    // ! (well it is hopefully time efficient but a space efficient implementation would just use an array)
    // ! but rather use case to identify any additional behavior that might be needed for a extensibility of the tree class
    // ! and provide a d-ary heap for myself to use in dijkstra and prim MST that could be replaced with a more efficient one in the future. 
    // ? since this isnt intended for general use and I mostly need it for pathfinding I'm only going to implement a min heap 
    public class D_aryHeap<THeapType> : MonoBehaviour {
        private Tree<float> heapTree = new Tree<float>();
        private Dictionary<THeapType,int> objectIDs = new Dictionary<THeapType, int>();
        private int IDcounter = 0;
// * Properties
        public int Size { get => heapTree.Size; }
        public bool isEmpty { get => Size == 0;}

// * Constructors
        public D_aryHeap() {
            // ? empty constructor
        }
        
        public THeapType Peek() {
            throw new NotImplementedException();
        }
        public void Push(THeapType inObject, float key) {
            if(objectIDs.ContainsKey(inObject)) {
                Debug.LogWarning($"Heap already contains object");
            }
            objectIDs.Add(inObject,IDcounter);
            IDcounter++;
            if(Size == 0) {
                heapTree.AddNode(new GraphNode<float>(objectIDs[inObject],null,key));
            } else {
                heapTree.TryAddEdge(GetBottomNode(),new GraphNode<float>(objectIDs[inObject],null,key)); 
            }
            SiftUp(inObject);
            throw new NotImplementedException();
        }
        public bool TryPop(out THeapType outObject) {
            throw new NotImplementedException();
        }
        public void DeleteRoot() {
            throw new NotImplementedException();
        }
        public void DeleteAndReplaceRoot() {
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
            throw new NotImplementedException();
        }
        public void SiftUp(THeapType obj) {
            throw new NotImplementedException();
        }
        public void SiftDown(THeapType obj) {
            throw new NotImplementedException();
        }

        private GraphNode<float> GetHeapNode(THeapType obj) {
            return heapTree.GetNode(objectIDs[obj]);
        }
        private GraphNode<float> GetBottomNode(){
            throw new NotImplementedException();
        }
        
    }
}
