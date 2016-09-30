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
            List<GeneticProgChromosome> children =  new List<GeneticProgChromosome>(); 

            GeneticProgChromosome lParentOne = parents[0] as GeneticProgChromosome;
            GeneticProgChromosome lParentTwo = parents[1] as GeneticProgChromosome;

            /* ????
            TreeNode<MathObject> lNode1 = lParentOne.GetGene(0).Value as TreeNode<MathObject>;
            return parents;
            */ 

            // 1) grab the root of both chromosones that act as parent tree for new chromosones
            //todo: do we need a clone/deep copy method? 
            TreeNode<MathObject> lRoot1 = lParentOne.GetGene(0).Value as TreeNode<MathObject>;
            TreeNode<MathObject> lRoot2 = lParentTwo.GetGene(0).Value as TreeNode<MathObject>;

            //2) randomly select 1 node of each tree random 

            TreeNode<MathObject> lNode1 = lParentOne.selectRandNode();
            TreeNode<MathObject> lNode2 = lParentTwo.selectRandNode();

            //3) swap all references 

            swap(lNode1, lNode2);
            swap(lNode2, lNode1);

            //4) validate both new trees
            Boolean valid1 = lRoot1.isValidSubTree();
            Boolean valid2 = lRoot2.isValidSubTree();

            //TODO: fix trees if necessary 


            //5) add to children list
            // Todo how do I cast children or the two trees to the correct type?  
            //lRoot1+2
            GeneticProgController child1 = new GeneticProgChromosome(lRoot1); 

            throw new NotImplementedException();
        }



        void swap(TreeNode<MathObject> lNode1, TreeNode<MathObject> lNode2)
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
