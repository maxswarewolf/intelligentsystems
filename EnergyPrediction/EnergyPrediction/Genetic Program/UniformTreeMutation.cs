//
// MIT LICENSE
//
// UniformTreeMutation.cs
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
using GeneticSharp.Domain.Mutations;
namespace EnergyPrediction
{
    public class UniformTreeMutation : MutationBase
    {
        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            // 1st) decide whether mutation is performed
            GeneticProgChromosome lChrom = chromosome as GeneticProgChromosome;

            Queue<TreeNode<MathObject>> parentList = new Queue<TreeNode<MathObject>>();
            parentList.Enqueue(lChrom.Root.ChildLeft);
            parentList.Enqueue(lChrom.Root.ChildRight);

            TreeNode<MathObject> currentNode;

            while (parentList.Count > 0)
            {
                currentNode = parentList.Dequeue();
                if (Randomizer.NextDouble(0, 1) <= probability)
                {
                    if (Randomizer.NextDouble(0, 1) <= 0.8)
                    {
                        currentNode.Data.ChangeValue();
                    }
                    else {
                        lChrom.replaceNode(ref currentNode);
                    }
                }
            }

            if (!VisitorPattern.hasX(lChrom.Root))
            {
                lChrom.addX(2);
            }
        }
    }
}
