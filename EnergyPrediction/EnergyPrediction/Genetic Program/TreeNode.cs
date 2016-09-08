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

{public class TreeNode<T> : IEnumerable<TreeNode<T>>
    { // source: http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp (scroll down!) 
        internal double getChild2;

        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public ICollection<TreeNode<T>> Children { get; set; }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new LinkedList<TreeNode<T>>();
        }


        public TreeNode<T> AddChild(T child)
        {
            if (Children.Count < 2)
            {
                TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
                this.Children.Add(childNode);
                return childNode;
            }
            else throw new Exception("This should be used as a binary tree- cannot have more than 2 children");
        }


        // other features ...
    }


    /* Usage examples
     * 
     * TreeNode<string> root = new TreeNode<string>("root");
{
    TreeNode<string> node0 = root.AddChild("node0");
    TreeNode<string> node1 = root.AddChild("node1");
    TreeNode<string> node2 = root.AddChild("node2");
    {
        TreeNode<string> node20 = node2.AddChild(null);
        TreeNode<string> node21 = node2.AddChild("node21");
        {
            TreeNode<string> node210 = node21.AddChild("node210");
            TreeNode<string> node211 = node21.AddChild("node211");
        }
    }
    TreeNode<string> node3 = root.AddChild("node3");
    {
        TreeNode<string> node30 = node3.AddChild("node30");
    }
}
*/

}

