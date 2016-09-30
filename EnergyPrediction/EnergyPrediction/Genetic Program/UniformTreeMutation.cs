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
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
namespace EnergyPrediction
{
    public class UniformTreeMutation : MutationBase
    {

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            // 1st) decide whether mutation is performed

            if (Randomizer.NextDouble(0, 1) <= probability)
            {
                // 2nd) mutate

                //a) pick a random node
                TreeNode<MathObject> lRoot = chromosome.GetGene(0).Value as TreeNode<MathObject>;
                GeneticProgChromosome lChrom = chromosome as GeneticProgChromosome;
                TreeNode<MathObject> randNode = lChrom.selectRandNode();


                //b) swap out value (and type??) s
                int r = Randomizer.NextInt(0, 1);
                TreeNode<MathObject> newNode = randNode;
                switch (r)
                {
                    case 0:
                        //change value, not type
                        randNode.Data.ChangeValue();
                        break;

                    case 1:
                        //change type 
                        newNode = changeObject(randNode);
                        replaceNode(randNode, newNode);
                        break;
                        //todo: create case 2 where node is transformed into an x-node?
                        //currently not possible - x creation is random

                }
                VisitorPattern.visit(lRoot);
                //todo: do we need to fix it? 
            }
        }

        TreeNode<MathObject> changeObject(TreeNode<MathObject> randNode)
        {
            Boolean isSymbol = randNode.Data.GetType().Equals(typeof(MathSymbol));
            Boolean isNumber = randNode.Data.GetType().Equals(typeof(MathNumber));
            if (isSymbol)
            {
                MathNumber newNr = new MathNumber();
                TreeNode<MathObject> newNode = new TreeNode<MathObject>(newNr, randNode.Depth);
                return newNode;
            }
            else if (isNumber)
            {
                MathSymbol newSymb = new MathSymbol();
                TreeNode<MathObject> newNode = new TreeNode<MathObject>(newSymb, randNode.Depth);
                return newNode;
            }
            else
                throw new NotSupportedException();
        }



        /// /////////////////////////////////////move eventually!//////////////////////////////////////
        //todo: move to util tree folder .... 
        void replaceNode(TreeNode<MathObject> oldNode, TreeNode<MathObject> newNode)
        {
            if (!newNode.Equals(oldNode))
            {
                // save old relationships
                TreeNode<MathObject> parent = oldNode.Parent;
                TreeNode<MathObject> leftChild = oldNode.ChildLeft;
                TreeNode<MathObject> rightChild = oldNode.ChildRight;

                //change references of others
                //a) parent
                if (parent.ChildLeft == oldNode)
                {
                    parent.setChildLeft(newNode);
                }
                else
                {
                    parent.setChildRight(newNode);
                }
                //b) children's parent is set automatically

                // change (and create) children if necessary 
                if (newNode.Data.GetType().Equals(typeof(MathSymbol)))
                {
                    if (leftChild == null)
                    {
                        leftChild = new TreeNode<MathObject>(new MathNumber(), newNode.Depth + 1);
                    }
                    if (rightChild == null)
                    {
                        rightChild = new TreeNode<MathObject>(new MathNumber(), newNode.Depth + 1);
                    }
                    newNode.setChildLeft(leftChild);
                    newNode.setChildRight(rightChild);
                }


            }
        }



    }
}
