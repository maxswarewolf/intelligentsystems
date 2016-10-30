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
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Randomizations;
using Gtk;

namespace EnergyPrediction
{
    public class GeneticProgChromosome : ChromosomeBase, ChromosomeExt
    {
        public int Depth { get; private set; }
        public int numXThreshold { get; private set; } = 2;
        public TreeNode<MathObject> Root { get; private set; }

        public GeneticProgChromosome(int aResultPeek, int aDepth) : base(2)
        {
            MathObject.RangePeek = aResultPeek;
            Depth = aDepth; //wanted depth in initial tree
            Queue<TreeNode<MathObject>> parentList = new Queue<TreeNode<MathObject>>();

            //Symbol is randomly selected
            Root = new TreeNode<MathObject>(new MathSymbol());
            TreeNode<MathObject> currentNode;

            //add the root of the tree to the queue
            parentList.Enqueue(Root);

            //initial construction of tree
            while (parentList.Count > 0)
            {
                currentNode = parentList.Dequeue();
                if (currentNode.Data.GetType().Equals(typeof(MathSymbol)))
                {
                    if (currentNode.Depth + 1 < Depth)
                    {
                        currentNode.setChildLeft(new TreeNode<MathObject>(MathObject.randomMathObject()));
                        currentNode.setChildRight(new TreeNode<MathObject>(MathObject.randomMathObject()));
                        parentList.Enqueue(currentNode.ChildLeft);
                        parentList.Enqueue(currentNode.ChildRight);
                    }
                    else {
                        currentNode.setChildLeft(new TreeNode<MathObject>(new MathNumber()));
                        currentNode.setChildRight(new TreeNode<MathObject>(new MathNumber()));
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
            return VisitorPattern.Calculate(Root, x);
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

        /// <summary>
        /// Selects the rand node, and return is. This method exclused the root node.
        /// </summary>
        /// <returns>The rand node.</returns>
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

        /// <summary>
        /// Swap the specified aNode1 and aNode2.
        /// </summary>
        /// <param name="aNode1">A node1.</param>
        /// <param name="aNode2">A node2.</param>
        public bool swap(TreeNode<MathObject> aNode1, TreeNode<MathObject> aNode2)
        {
            TreeNode<MathObject> lNewNode1 = new TreeNode<MathObject>(aNode1);
            TreeNode<MathObject> lNewNode2 = new TreeNode<MathObject>(aNode2);

            if (aNode1.isLeftChild)
            {
                lNewNode1.Parent.setChildLeft(aNode2);
            }
            else
            {
                lNewNode1.Parent.setChildRight(aNode2);
            }

            if (aNode2.isLeftChild)
            {
                lNewNode2.Parent.setChildLeft(aNode1);
            }
            else
            {
                lNewNode2.Parent.setChildRight(aNode1);
            }

            return true;
        }

        /// <summary>
        /// Replaces the node vlaue, with a newly generated value.
        /// </summary>
        /// <param name="aTargetNode">A target node.</param>
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
                aTargetNode.setChildLeft(new TreeNode<MathObject>(new MathNumber()));
                aTargetNode.setChildRight(new TreeNode<MathObject>(new MathNumber()));
            }
            else
                throw new NotSupportedException();
        }

        public bool confirmNumX(int aNumX)
        {
            int counter = 0;
            do
            {
                counter = VisitorPattern.addXPostOrder(Root);
            }
            while (counter < aNumX);
            return true;
        }

        public override string ToString()
        {
            if (Fitness == null)
            {
                IFitness com = new FitnessFunctions();
                Fitness = com.Evaluate(this);
            }
            string res = "Fitness: " + Fitness + "\n";

            return VisitorPattern.RootToString(Root) + "\n" + res;
        }
    }
}