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

        public TreeNode<MathObject> Root { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:EnergyPrediction.GeneticAlgoChromosome"/> class.
        /// </summary>
        /// <param name="aResultRange">A result range. eg -20 to 20</param>
        /// KC: chromosone is initialized as a tree from file TreeNode
        /// must use "root" object to access tree
        /// last level (i.e. level == depth) is ensured to be a numerical value, everything else is a MathObject 
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
            if (!VisitorPattern.hasX(Root))
            {
                addX(0);
            }
            //todo: make sure tree contains at least one x (and is a valid tree!!!) 
            VisitorPattern.hasX(Root);
            //todo: fix if not!
        }

        public GeneticProgChromosome(TreeNode<MathObject> lRoot1) : base(2)
        {
            Root = lRoot1;
            Depth = lRoot1.getMaxDepth();
        }

        public double getCalculatedY(int x)
        {
            // TODO test!
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

        public bool swap(ref TreeNode<MathObject> aNode1, ref TreeNode<MathObject> aNode2)
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

        public void replaceNode(ref TreeNode<MathObject> aTargetNode)
        {
            if (aTargetNode.GetType().Equals(typeof(MathSymbol)))
            {
                aTargetNode.Data = new MathNumber();
                aTargetNode.setChildLeft(null);
                aTargetNode.setChildRight(null);
            }
            else if (aTargetNode.GetType().Equals(typeof(MathNumber)))
            {
                aTargetNode.Data = new MathSymbol();
                aTargetNode.setChildLeft(new TreeNode<MathObject>(new MathNumber(), aTargetNode.Depth + 1));
                aTargetNode.setChildRight(new TreeNode<MathObject>(new MathNumber(), aTargetNode.Depth + 1));
            }
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Adds the x.
        /// </summary>
        /// <param name="aDir">A dir.</param>
        public void addX(int aDir)
        {
            Queue<TreeNode<MathObject>> parentList = new Queue<TreeNode<MathObject>>();
            parentList.Enqueue(Root);
            bool lMidToggle = true;
            TreeNode<MathObject> currentNode;
            while (parentList.Count > 0)
            {
                currentNode = parentList.Dequeue();
                if (currentNode.Data.GetType().Equals(typeof(MathNumber)))
                {
                    currentNode.Data = new MathNumber(0); //sets the node to X
                    break;
                }
                else
                {
                    switch (aDir)
                    {
                        case -1: //Left
                            parentList.Enqueue(currentNode.ChildLeft);
                            break;
                        case 0: //Mid
                            parentList.Enqueue((lMidToggle) ? currentNode.ChildLeft : currentNode.ChildRight);
                            lMidToggle = !lMidToggle;
                            break;
                        case 1: //Right
                            parentList.Enqueue(currentNode.ChildRight);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}