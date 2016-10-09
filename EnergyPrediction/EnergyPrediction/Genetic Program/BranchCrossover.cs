//
// MIT LICENSE
//
// BranchCrossover.cs
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
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
namespace EnergyPrediction
{
    public class BranchCrossover : CrossoverBase
    {
        public BranchCrossover() : base(2, 2)
        {
        }

        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            GeneticProgChromosome lParentOne = parents[0] as GeneticProgChromosome;
            GeneticProgChromosome lParentTwo = parents[1] as GeneticProgChromosome;

            //1) randomly select 1 node of each tree random 

            TreeNode<MathObject> lNode1 = lParentOne.selectRandNode();
            TreeNode<MathObject> lNode2 = lParentTwo.selectRandNode();

            //2) swap all references 

            lParentOne.swap(ref lNode1, ref lNode2);

            //3) validate both new trees
            if (!VisitorPattern.hasX(lParentOne.Root))
            {
                lParentOne.addX(2);
            }

            if (!VisitorPattern.hasX(lParentTwo.Root))
            {
                lParentTwo.addX(2);
            }

            return new List<IChromosome>() { lParentOne, lParentTwo };
        }

        void swap(ref TreeNode<MathObject> lNode1, ref TreeNode<MathObject> lNode2)
        {
            TreeNode<MathObject> parent1 = lNode1.Parent;
            if (parent1.ChildLeft == lNode1)
            {
                parent1.setChildLeft(lNode2);
            }
            else
            {
                parent1.setChildRight(lNode2);
            }
        }
    }
}
