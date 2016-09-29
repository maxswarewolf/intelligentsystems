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

        TreeNode<MathObject> root;

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

            root = new TreeNode<MathObject>(new MathSymbol(), 1); //  data is randomly chosen at initiallization 
            TreeNode<MathObject> currentNode;

            parentList.Enqueue(root);

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
            //todo: make sure tree contains at least one x (and is a valid tree!!!) 
        }

        public double getCalculatedY(int x)
        {
            // TODO test!
            return root.doCalculation(x);
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
            return new Gene(root);
        }



    }
}