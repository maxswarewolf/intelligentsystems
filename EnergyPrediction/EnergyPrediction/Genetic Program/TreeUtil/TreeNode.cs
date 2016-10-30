//
// MIT LICENSE
//
// EmptyStruct.cs
//
// Author:
//       Katie Clark, Sean Grinter, Adrian Pellegrino <Energy Prediction>
//
// Copyright (c) 2016 Katie Clark, Sean Grinter, Adrian Pellegrino
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections;
using System.Collections.Generic;

namespace EnergyPrediction
{
    public class TreeNode<T>
    { // source: http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp (scroll down!) 

        public T Data { get; set; }
        public bool isLeftChild { get; private set; }
        public int Depth { get; private set; }
        public TreeNode<T> Parent { get; protected set; }
        public TreeNode<T> ChildRight { get; private set; }
        public TreeNode<T> ChildLeft { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EnergyPrediction.TreeNode`1"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        public TreeNode(T data)
        {
            this.Data = data;
            this.Depth = 1;
            this.ChildLeft = null;
            this.ChildRight = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EnergyPrediction.TreeNode`1"/> class.
        /// Copy Constructor
        /// </summary>
        /// <param name="root">Root.</param>
        public TreeNode(TreeNode<T> root)
        {
            this.Data = root.Data;
            this.isLeftChild = root.isLeftChild;
            this.Depth = root.Depth;
            this.Parent = root.Parent;
            if (root.ChildLeft != null)
            {
                this.ChildLeft = new TreeNode<T>(root.ChildLeft);
                this.ChildRight = new TreeNode<T>(root.ChildRight);
            }
            else {
                this.ChildLeft = null;
                this.ChildRight = null;
            }

        }

        /// <summary>
        /// Sets the child right to null.
        /// </summary>
        public void setChildRight()
        {
            this.ChildRight = null;
        }

        /// <summary>
        /// Sets the child right.
        /// </summary>
        /// <param name="aChild">A child.</param>
        public void setChildRight(TreeNode<T> aChild)
        {
            this.ChildRight = aChild;
            this.ChildRight.isLeftChild = false;
            this.ChildRight.Depth = this.Depth + 1;
            this.ChildRight.Parent = this;
        }

        /// <summary>
        /// Sets the child left to null.
        /// </summary>
        public void setChildLeft()
        {
            this.ChildLeft = null;
        }

        /// <summary>
        /// Sets the child left.
        /// </summary>
        /// <param name="aChild">A child.</param>
        public void setChildLeft(TreeNode<T> aChild)
        {
            this.ChildLeft = aChild;
            this.ChildLeft.isLeftChild = true;
            this.ChildLeft.Depth = this.Depth + 1;
            this.ChildLeft.Parent = this;
        }

        /// <summary>
        /// Recursive Search to Find bottom most Child
        /// </summary>
        /// <returns>The max depth.</returns>
        public int getMaxDepth()
        {
            if (ChildLeft != null)
            {
                return Math.Max(ChildLeft.getMaxDepth(), ChildRight.getMaxDepth());
            }
            return Depth;
        }

    }
}

