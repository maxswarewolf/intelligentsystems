﻿//
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

            GeneticProgChromosome lChild1 = new GeneticProgChromosome(new TreeNode<MathObject>(lParentOne.Root));
            GeneticProgChromosome lChild2 = new GeneticProgChromosome(new TreeNode<MathObject>(lParentTwo.Root));

            //1) randomly select 1 node of each tree random 

            TreeNode<MathObject> lNode1 = lChild1.selectRandNode();
            TreeNode<MathObject> lNode2 = lChild2.selectRandNode();

            //2) swap all references 

            lChild1.swap(lNode1, lNode2);

            //3) Validate has Enough X references 
            lChild1.confirmNumX(lChild1.numXThreshold);
            lChild2.confirmNumX(lChild2.numXThreshold);

            return new List<IChromosome>() { lChild1, lChild2 };
        }
    }
}
