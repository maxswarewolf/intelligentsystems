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
        public int Depth { get; private set; }
        public TreeNode<T> Parent { get; protected set; }
        public TreeNode<T> ChildRight { get; private set; }
        public TreeNode<T> ChildLeft { get; private set; }

        public TreeNode(T data, int depth)
        {
            this.Data = data;
            this.Depth = depth;
            this.ChildLeft = null;
            this.ChildRight = null;
        }

        public TreeNode(TreeNode<T> root)
        {
            this.Data = root.Data;
            this.Depth = root.Depth;
            this.Parent = root.Parent;
            if (!root.ChildLeft.Equals(null))
            {
                this.ChildLeft = new TreeNode<T>(root.ChildLeft);
                this.ChildRight = new TreeNode<T>(root.ChildRight);
            }
            else {
                this.ChildLeft = null;
                this.ChildRight = null;
            }

        }

        public void setChildRight(TreeNode<T> aChild)
        {
            this.ChildRight = aChild;
            this.ChildRight.Parent = this;
        }

        public void setChildLeft(TreeNode<T> aChild)
        {
            this.ChildLeft = aChild;
            this.ChildLeft.Parent = this;
        }

        public TreeNode<T> cloneTree()
        {
            return new TreeNode<T>(this);
        }

        public int getMaxDepth()
        {
            //recursive search of bottom most child
            if (ChildLeft == null && ChildRight == null)
            {
                return Depth;
            }
            else if (ChildLeft == null)
            {
                return ChildRight.getMaxDepth();
            }
            else if (ChildRight == null)
            {
                return ChildLeft.getMaxDepth();
            }

            return Math.Max(ChildLeft.getMaxDepth(), ChildRight.getMaxDepth());
        }

        public double doCalculation(int x)
        {
            if (Data.GetType().Equals(typeof(MathNumber)))
            {
                MathNumber temp = Data as MathNumber;
                return temp.doCalculation(x);
            }
            else
            {
                MathSymbol temp = Data as MathSymbol;
                return temp.doCalculation(ChildLeft.doCalculation(x), ChildRight.doCalculation(x));
            }
        }

        // other features ...
    }

}

