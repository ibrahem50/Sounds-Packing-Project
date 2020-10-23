using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace projectalgo
{
    //public class PriorityQueue<T>
    //{
    //    private IComparer<T> _comparer;
    //    private T[] _data;
    //    private int _count = 0;

    //    public PriorityQueue()
    //      : this(11)
    //    {

    //    }

    //    public PriorityQueue(int initialCapacity, IComparer<T> comparer)
    //    {
    //        if (initialCapacity < 0)
    //        {
    //            throw new ArgumentOutOfRangeException("initialCapacity");
    //        }

    //        if (comparer == null)
    //        {
    //            comparer = Comparer<T>.Default;
    //        }

    //        _data = new T[initialCapacity];

    //        _comparer = comparer;
    //    }

    //    public PriorityQueue(int initialCapacity)
    //      : this(initialCapacity, null)
    //    {

    //    }

    //    private void IncreaseCapacity()
    //    {
    //        int size = (_count + 1) << 1;

    //        T[] data = new T[size];

    //        Array.Copy(_data, data, _data.Length);

    //        _data = data;
    //    }

    //    public void Enqueue(T item)
    //    {
    //        if (_count == _data.Length)
    //        {
    //            IncreaseCapacity();
    //        }

    //        int index = _count;

    //        _data[_count] = item;

    //        _count += 1;

    //        while (index > 0)
    //        {
    //            int parent = (index - 1) / 2;

    //            if (_comparer.Compare(_data[index], _data[parent]) >= 0)
    //            {
    //                break;
    //            }

    //            T element = _data[index];

    //            _data[index] = _data[parent];
    //            _data[parent] = element;

    //            index = parent;
    //        }
    //    }

    //    public void Clear()
    //    {
    //        if (_count > 0)
    //        {
    //            Array.Clear(this._data, 0, _count);

    //            _count = 0;
    //        }
    //    }

    //    public int Count
    //    {
    //        get
    //        {
    //            return _count;
    //        }
    //    }

    //    public T Dequeue()
    //    {
    //        if (_count <= 0)
    //        {
    //            throw new InvalidOperationException("Queue empty.");
    //        }

    //        _count -= 1;

    //        T first = _data[0];

    //        _data[0] = _data[_count];

    //        _data[_count] = default(T);

    //        int index = 0;

    //        while (true)
    //        {
    //            int left = (index << 1) + 1;

    //            if (left >= _count)
    //            {
    //                return first;
    //            }

    //            int right = left + 1;

    //            if (right < _count)
    //            {
    //                if (_comparer.Compare(_data[left], _data[right]) > 0)
    //                {
    //                    left = right;
    //                }
    //            }

    //            if (_comparer.Compare(_data[index], _data[left]) <= 0)
    //            {
    //                return first;
    //            }

    //            T element = _data[index];

    //            _data[index] = _data[left];
    //            _data[left] = element;

    //            index = left;
    //        }
    //    }

    //}


    //-----------------------------------------------------------------------
    //
    //  Microsoft Windows Client Platform
    //  Copyright (C) Microsoft Corporation. All rights reserved.
    //
    //  File:      PriorityQueue.cs
    //
    //  Contents:  Implementation of PriorityQueue class.
    //
    //  Created:   2-14-2005 Niklas Borson (niklasb)
    //
    //------------------------------------------------------------------------

    /// <summary>
    /// PriorityQueue provides a stack-like interface, except that objects
    /// "pushed" in arbitrary order are "popped" in order of priority, i.e., 
    /// from least to greatest as defined by the specified comparer.
    /// </summary>
    /// <remarks>
    /// Push and Pop are each O(log N). Pushing N objects and them popping
    /// them all is equivalent to performing a heap sort and is O(N log N).
    /// </remarks>
    //public class PriorityQueue<T>
    //{
    //    //
    //    // The _heap array represents a binary tree with the "shape" property.
    //    // If we number the nodes of a binary tree from left-to-right and top-
    //    // to-bottom as shown,
    //    //
    //    //             0
    //    //           /   \
    //    //          /     \
    //    //         1       2
    //    //       /  \     / \
    //    //      3    4   5   6
    //    //     /\    /
    //    //    7  8  9
    //    //
    //    // The shape property means that there are no gaps in the sequence of
    //    // numbered nodes, i.e., for all N > 0, if node N exists then node N-1
    //    // also exists. For example, the next node added to the above tree would
    //    // be node 10, the right child of node 4.
    //    //
    //    // Because of this constraint, we can easily represent the "tree" as an
    //    // array, where node number == array index, and parent/child relationships
    //    // can be calculated instead of maintained explicitly. For example, for
    //    // any node N > 0, the parent of N is at array index (N - 1) / 2.
    //    //
    //    // In addition to the above, the first _count members of the _heap array
    //    // compose a "heap", meaning each child node is greater than or equal to
    //    // its parent node; thus, the root node is always the minimum (i.e., the
    //    // best match for the specified style, weight, and stretch) of the nodes 
    //    // in the heap.
    //    //
    //    // Initially _count < 0, which means we have not yet constructed the heap.
    //    // On the first call to MoveNext, we construct the heap by "pushing" all
    //    // the nodes into it. Each successive call "pops" a node off the heap
    //    // until the heap is empty (_count == 0), at which time we've reached the
    //    // end of the sequence.
    //    //

    //    #region constructors

    //    internal PriorityQueue(int capacity, IComparer<T> comparer)
    //    {

    //        _heap = new T[capacity > 0 ? capacity : DefaultCapacity];
    //        _count = 0;
    //        _comparer = comparer;
    //    }

    //    #endregion

    //    #region internal members

    //    /// <summary>
    //    /// Gets the number of items in the priority queue.
    //    /// </summary>
    //    internal int Count
    //    {
    //        get { return _count; }
    //    }

    //    /// <summary>
    //    /// Gets the first or topmost object in the priority queue, which is the
    //    /// object with the minimum value.
    //    /// </summary>
    //    internal T Top
    //    {
    //        get
    //        {
    //            Debug.Assert(_count > 0);
    //            return _heap[0];
    //        }
    //    }

    //    /// <summary>
    //    /// Adds an object to the priority queue.
    //    /// </summary>
    //    internal void Push(T value)
    //    {
    //        // Increase the size of the array if necessary.
    //        if (_count == _heap.Length)
    //        {
    //            T[] temp = new T[_count * 2];
    //            for (int i = 0; i < _count; ++i)
    //            {
    //                temp[i] = _heap[i];
    //            }
    //            _heap = temp;
    //        }

    //        // Loop invariant:
    //        //
    //        //  1.  index is a gap where we might insert the new node; initially
    //        //      it's the end of the array (bottom-right of the logical tree).
    //        //
    //        int index = _count;
    //        while (index > 0)
    //        {
    //            int parentIndex = HeapParent(index);
    //            if (_comparer.Compare(value, _heap[parentIndex]) < 0)
    //            {
    //                // value is a better match than the parent node so exchange
    //                // places to preserve the "heap" property.
    //                _heap[index] = _heap[parentIndex];
    //                index = parentIndex;
    //            }
    //            else
    //            {
    //                // we can insert here.
    //                break;
    //            }
    //        }

    //        _heap[index] = value;
    //        _count++;
    //    }

    //    /// <summary>
    //    /// Removes the first node (i.e., the logical root) from the heap.
    //    /// </summary>
    //    internal void Pop()
    //    {
    //        Debug.Assert(_count != 0);

    //        if (_count > 1)
    //        {
    //            // Loop invariants:
    //            //
    //            //  1.  parent is the index of a gap in the logical tree
    //            //  2.  leftChild is
    //            //      (a) the index of parent's left child if it has one, or
    //            //      (b) a value >= _count if parent is a leaf node
    //            //
    //            int parent = 0;
    //            int leftChild = HeapLeftChild(parent);

    //            while (leftChild < _count)
    //            {
    //                int rightChild = HeapRightFromLeft(leftChild);
    //                int bestChild =
    //                    (rightChild < _count && _comparer.Compare(_heap[rightChild], _heap[leftChild]) < 0) ?
    //                    rightChild : leftChild;

    //                // Promote bestChild to fill the gap left by parent.
    //                _heap[parent] = _heap[bestChild];

    //                // Restore invariants, i.e., let parent point to the gap.
    //                parent = bestChild;
    //                leftChild = HeapLeftChild(parent);
    //            }

    //            // Fill the last gap by moving the last (i.e., bottom-rightmost) node.
    //            _heap[parent] = _heap[_count - 1];
    //        }

    //        _count--;
    //    }

    //    #endregion

    //    #region private members

    //    /// <summary>
    //    /// Calculate the parent node index given a child node's index, taking advantage
    //    /// of the "shape" property.
    //    /// </summary>
    //    private static int HeapParent(int i)
    //    {
    //        return (i - 1) / 2;
    //    }

    //    /// <summary>
    //    /// Calculate the left child's index given the parent's index, taking advantage of
    //    /// the "shape" property. If there is no left child, the return value is >= _count.
    //    /// </summary>
    //    private static int HeapLeftChild(int i)
    //    {
    //        return (i * 2) + 1;
    //    }

    //    /// <summary>
    //    /// Calculate the right child's index from the left child's index, taking advantage
    //    /// of the "shape" property (i.e., sibling nodes are always adjacent). If there is
    //    /// no right child, the return value >= _count.
    //    /// </summary>
    //    private static int HeapRightFromLeft(int i)
    //    {
    //        return i + 1;
    //    }

    //    private T[] _heap;
    //    private int _count;
    //    private IComparer<T> _comparer;
    //    private const int DefaultCapacity = 6;

    //    #endregion
    //}
    /// </summary>
    public class PriorityQueue<T>
        where T : IComparable
    {
        protected List<T> storedValues;

        public PriorityQueue()
        {
            //Initialize the array that will hold the values
            storedValues = new List<T>();

            //Fill the first cell in the array with an empty value
            storedValues.Add(default(T));
        }

        /// <summary>
        /// Gets the number of values stored within the Priority Queue
        /// </summary>
        public virtual int Count
        {
            get { return storedValues.Count - 1; }
        }

        /// <summary>
        /// Returns the value at the head of the Priority Queue without removing it.
        /// </summary>
        public virtual T Peek()
        {
            if (this.Count == 0)
                return default(T); //Priority Queue empty
            else
                return storedValues[1]; //head of the queue
        }

        /// <summary>
        /// Adds a value to the Priority Queue
        /// </summary>
        public virtual void Enqueue(T value)
        {
            //Add the value to the internal array
            storedValues.Add(value);

            //Bubble up to preserve the heap property,
            //starting at the inserted value
            this.BubbleUp(storedValues.Count - 1);
        }

        /// <summary>
        /// Returns the minimum value inside the Priority Queue
        /// </summary>
        public virtual T Dequeue()
        {
            if (this.Count == 0)
                return default(T); //queue is empty
            else
            {
                //The smallest value in the Priority Queue is the first item in the array
                T minValue = this.storedValues[1];

                //If there's more than one item, replace the first item in the array with the last one
                if (this.storedValues.Count > 2)
                {
                    T lastValue = this.storedValues[storedValues.Count - 1];

                    //Move last node to the head
                    this.storedValues.RemoveAt(storedValues.Count - 1);
                    this.storedValues[1] = lastValue;

                    //Bubble down
                    this.BubbleDown(1);
                }
                else
                {
                    //Remove the only value stored in the queue
                    storedValues.RemoveAt(1);
                }

                return minValue;
            }
        }

        /// <summary>
        /// Restores the heap-order property between child and parent values going up towards the head
        /// </summary>
        protected virtual void BubbleUp(int startCell)
        {
            int cell = startCell;

            //Bubble up as long as the parent is greater
            while (this.IsParentBigger(cell))
            {
                //Get values of parent and child
                T parentValue = this.storedValues[cell / 2];
                T childValue = this.storedValues[cell];

                //Swap the values
                this.storedValues[cell / 2] = childValue;
                this.storedValues[cell] = parentValue;

                cell /= 2; //go up parents
            }
        }

        /// <summary>
        /// Restores the heap-order property between child and parent values going down towards the bottom
        /// </summary>
        protected virtual void BubbleDown(int startCell)
        {
            int cell = startCell;

            //Bubble down as long as either child is smaller
            while (this.IsLeftChildSmaller(cell) || this.IsRightChildSmaller(cell))
            {
                int child = this.CompareChild(cell);

                if (child == -1) //Left Child
                {
                    //Swap values
                    T parentValue = storedValues[cell];
                    T leftChildValue = storedValues[2 * cell];

                    storedValues[cell] = leftChildValue;
                    storedValues[2 * cell] = parentValue;

                    cell = 2 * cell; //move down to left child
                }
                else if (child == 1) //Right Child
                {
                    //Swap values
                    T parentValue = storedValues[cell];
                    T rightChildValue = storedValues[2 * cell + 1];

                    storedValues[cell] = rightChildValue;
                    storedValues[2 * cell + 1] = parentValue;

                    cell = 2 * cell + 1; //move down to right child
                }
            }
        }

        /// <summary>
        /// Returns if the value of a parent is greater than its child
        /// </summary>
        protected virtual bool IsParentBigger(int childCell)
        {
            if (childCell == 1)
                return false; //top of heap, no parent
            else
                return storedValues[childCell / 2].CompareTo(storedValues[childCell]) > 0;
            //return storedNodes[childCell / 2].Key > storedNodes[childCell].Key;
        }

        /// <summary>
        /// Returns whether the left child cell is smaller than the parent cell.
        /// Returns false if a left child does not exist.
        /// </summary>
        protected virtual bool IsLeftChildSmaller(int parentCell)
        {
            if (2 * parentCell >= storedValues.Count)
                return false; //out of bounds
            else
                return storedValues[2 * parentCell].CompareTo(storedValues[parentCell]) < 0;
            //return storedNodes[2 * parentCell].Key < storedNodes[parentCell].Key;
        }

        /// <summary>
        /// Returns whether the right child cell is smaller than the parent cell.
        /// Returns false if a right child does not exist.
        /// </summary>
        protected virtual bool IsRightChildSmaller(int parentCell)
        {
            if (2 * parentCell + 1 >= storedValues.Count)
                return false; //out of bounds
            else
                return storedValues[2 * parentCell + 1].CompareTo(storedValues[parentCell]) < 0;
            //return storedNodes[2 * parentCell + 1].Key < storedNodes[parentCell].Key;
        }

        /// <summary>
        /// Compares the children cells of a parent cell. -1 indicates the left child is the smaller of the two,
        /// 1 indicates the right child is the smaller of the two, 0 inidicates that neither child is smaller than the parent.
        /// </summary>
        protected virtual int CompareChild(int parentCell)
        {
            bool leftChildSmaller = this.IsLeftChildSmaller(parentCell);
            bool rightChildSmaller = this.IsRightChildSmaller(parentCell);

            if (leftChildSmaller || rightChildSmaller)
            {
                if (leftChildSmaller && rightChildSmaller)
                {
                    //Figure out which of the two is smaller
                    int leftChild = 2 * parentCell;
                    int rightChild = 2 * parentCell + 1;

                    T leftValue = this.storedValues[leftChild];
                    T rightValue = this.storedValues[rightChild];

                    //Compare the values of the children
                    if (leftValue.CompareTo(rightValue) <= 0)
                        return -1; //left child is smaller
                    else
                        return 1; //right child is smaller]
                }
                else if (leftChildSmaller)
                    return -1; //left child is smaller
                else
                    return 1; //right child smaller
            }
            else
                return 0; //both children are bigger or don't exist
        }

    }
}

