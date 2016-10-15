//
// MIT LICENSE
//
// GeneticAlgoGenome.cs
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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnergyPrediction;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using Gtk;

namespace EnergyPrediction
{
    public class GeneticProgChromosome : ChromosomeBase, ChromosomeExt
    {
        public int Depth { get; private set; }
        public int numXThreshold { get; private set; } = 1;
        public TreeNode<MathObject> Root { get; private set; }

        public GeneticProgChromosome(int aResultPeek, int aDepth) : base(2)
        {
            MathObject.RangePeek = aResultPeek;
            Depth = aDepth; //wanted depth in initial tree
            Queue<TreeNode<MathObject>> parentList = new Queue<TreeNode<MathObject>>();

            Root = new TreeNode<MathObject>(new MathSymbol(), 1); //  data is randomly chosen at initiallization 
            TreeNode<MathObject> currentNode;

            parentList.Enqueue(Root);

            //initial construction of tree
            while (parentList.Count > 0)
            {
                currentNode = parentList.Dequeue();
                if (currentNode.Data.GetType().Equals(typeof(MathSymbol)))
                {
                    if (currentNode.Depth + 1 < Depth)
                    {
                        currentNode.setChildLeft(new TreeNode<MathObject>(MathObject.randomMathObject(), currentNode.Depth + 1));
                        currentNode.setChildRight(new TreeNode<MathObject>(MathObject.randomMathObject(), currentNode.Depth + 1));
                        parentList.Enqueue(currentNode.ChildLeft);
                        parentList.Enqueue(currentNode.ChildRight);
                    }
                    else {
                        currentNode.setChildLeft(new TreeNode<MathObject>(new MathNumber(), currentNode.Depth + 1));
                        currentNode.setChildRight(new TreeNode<MathObject>(new MathNumber(), currentNode.Depth + 1));
                    }
                }
            }
            //Is setting a MathNumber to X
            if (VisitorPattern.NumX(Root) < numXThreshold)
            {
                confirmNumX(numXThreshold);
            }
        }

        public GeneticProgChromosome(TreeNode<MathObject> lRoot1) : base(2)
        {
            Root = lRoot1;
            Depth = lRoot1.getMaxDepth();
        }

        public double getCalculatedY(int x)
        {
            return Root.doCalculation(x);
        }

        /// <summary>
        /// Creates a new Chromosome.
        /// </summary>
        /// <returns>The new.</returns>
        public override IChromosome CreateNew()
        {
            return new GeneticProgChromosome((int)MathObject.RangePeek, Length);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(Root);
        }

        //returns a random node of the tree (not the root)
        public TreeNode<MathObject> selectRandNode()
        {
            TreeNode<MathObject> node = Root;
            // min value is 2 as 1 is the root, which is not desired
            int rand = Randomizer.NextInt(2, Root.getMaxDepth());

            for (int i = 1; i < rand; i++)
            {
                int direction = Randomizer.NextInt(1, 2);
                // 1 = goLeft, 2=goRight
                if (direction == 1 && node.ChildLeft != null)
                {
                    node = node.ChildLeft;
                }
                else if (direction == 2 && node.ChildRight != null)
                {
                    node = node.ChildRight;
                }
            }
            return node;
        }

        public bool swap(TreeNode<MathObject> aNode1, TreeNode<MathObject> aNode2)
        {
            TreeNode<MathObject> lP1 = aNode1.Parent;
            TreeNode<MathObject> lP2 = aNode2.Parent;

            bool aNode2isLeft = false;
            if (lP2.ChildLeft == aNode2)
            {
                aNode2isLeft = true;
            }

            if (lP1.ChildLeft == aNode1)
            {
                lP1.setChildLeft(aNode2);
            }
            else {
                lP1.setChildRight(aNode2);
            }

            if (aNode2isLeft)
            {
                lP2.setChildLeft(aNode1);
            }
            else {
                lP2.setChildRight(aNode2);
            }

            return true;
        }

        public void replaceNode(TreeNode<MathObject> aTargetNode)
        {
            if (aTargetNode.Data.GetType().Equals(typeof(MathSymbol)))
            {
                aTargetNode.Data = new MathNumber();
                aTargetNode.setChildLeft();
                aTargetNode.setChildRight();
            }
            else if (aTargetNode.Data.GetType().Equals(typeof(MathNumber)))
            {
                aTargetNode.Data = new MathSymbol();
                aTargetNode.setChildLeft(new TreeNode<MathObject>(new MathNumber(), aTargetNode.Depth + 1));
                aTargetNode.setChildRight(new TreeNode<MathObject>(new MathNumber(), aTargetNode.Depth + 1));
            }
            else
                throw new NotSupportedException();
        }

        public bool confirmNumX(int aNumX)
        {
            int counter = VisitorPattern.NumX(Root);

            while (counter < aNumX)
            {
                counter = aNumX - VisitorPattern.confirmNumXInOrder(Root, aNumX - counter, true);
            }
            return true;
        }
    }
}