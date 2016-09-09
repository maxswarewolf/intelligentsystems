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
    public class GeneticProgChromosome : ChromosomeBase
    {

        int gNumberOfGenes;
        public int gRangePeek { get; internal set; }
        TreeNode<MathObject> root;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EnergyPrediction.GeneticAlgoChromosome"/> class.
        /// </summary>
        /// <param name="aResultRange">A result range. eg -20 to 20</param>
        /// KC: chromosone is initialized as a tree from file TreeNode
        /// must use "root" object to access tree
        /// last level (i.e. level == depth) is ensured to be a numerical value, everything else is a MathObject 
        public GeneticProgChromosome(int aResultPeek) : base(0)
        {
            int depth = 3; //wanted depth in initial tree
            int valueRange = gRangePeek;
            int currentDepth = 1;

            root = new TreeNode<MathObject>(new MathSymbol(), 0); //  data is randomly chosen at initiallization 

            List<TreeNode<MathObject>> parentList = new List<TreeNode<MathObject>>();
            parentList.Add(root);

            //initial construction of tree
            while (currentDepth < depth)
            {
                // add math operation (attention: can also be number - exception handling?!)  
                foreach (TreeNode<MathObject> currentNode in parentList)
                {
                    currentNode.ChildRight = new TreeNode<MathObject>(new MathObject()); //Right Child
                    currentNode.ChildLeft = new TreeNode<MathObject>(new MathObject()); //Left Child

                    if (!currentNode.ChildLeft.Data.GetType().Equals(new MathNumber(0)))
                    {
                        parentList.Add(currentNode.ChildLeft);
                    }
                    if (!currentNode.ChildRight.Data.GetType().Equals(new MathNumber(0)))
                    {
                        parentList.Add(currentNode.ChildRight);
                    }
                }
            }// last layer: add number instead of math op for leafs
            foreach (TreeNode<MathObject> currentNode in parentList)
            {
                currentNode.ChildRight = new TreeNode<MathObject>(new MathNumber(valueRange));
                currentNode.ChildLeft = new TreeNode<MathObject>(new MathNumber(valueRange));
            }

        }

        /// <summary>
        /// Creates a random math object. If the Boolean mustBranch is true only Object types that have children can be selected, 
        /// meaning MathX anf MathNumber are not created
        /// </summary>
        /// <returns>The random math object.</returns>
        /// <param name="mustBranch">If set to <c>true</c> must branch.</param>
        MathObject createRandomMathObject(Boolean mustBranch)
        {
            if (mustBranch)
            {
                return new MathSymbol();
            }
            else {
                return new MathNumber(gRangePeek);
            }

            //Contract.Ensures(Contract.Result<MathObject>() != null);
            //Random R = new Random();
            //int rand = R.Next(0, MathObject.totalNrMathObjects);
            //while (mustBranch && rand < MathObject.unbranchableNrMathObjects)
            //{
            //    rand = MathObject.Rand.Next(MathObject.unbranchableNrMathObjects, MathObject.totalNrMathObjects);
            //}
            //switch (rand)
            //{
            //    case 0:
            //        return new MathX();
            //    case 1:
            //        return new MathNumber(gRangePeek);
            //    case 2:
            //        return new MathOpAdd();
            //    default:
            //        return new MathOpMult();
            //}

        }

        public double getCalculatedY(int x)
        {
            // recursive calculation of tree value for given x
            TreeNode<MathObject> resultTree = root; // todo: DOUBLE AND TRIPLE CHECK THAT THIS IS A COPY; OTHERWISE IMPLEMENTE A COPY FUNCTION

            double result = root.doCalculation(x);
            // TODO test!
            return result;
        }

        //double doRecursiveCalc(TreeNode<MathObject> currentRoot, int x)
        //{            //  LinkedList<TreeNode<MathObject>> list = (System.Collections.Generic.LinkedList<EnergyPrediction.TreeNode<EnergyPrediction.MathObject>>)root.Children;
        //    LinkedList<TreeNode<MathObject>> children = (System.Collections.Generic.LinkedList<EnergyPrediction.TreeNode<EnergyPrediction.MathObject>>)currentRoot.Children; ;
        //    TreeNode<MathObject> child1 = children.First.Value;
        //    TreeNode<MathObject> child2 = children.Last.Value;

        //    // end of recursion reached when booth children are of type MathNumer
        //    if (child1.Data.getMathObjectType().equals(new MathNumber(0)))
        //    {
        //        if (child2.Data.getMathObjectType().equals(new MathNumber(0)))
        //        {// END of recursion 
        //            double result = currentRoot.Data.doCalc(child1.Data.getNumberValue(), child2.Data.getNumberValue(), x);
        //            return result;
        //        }
        //        else {  // child 2 not of type MathNumber
        //            return currentRoot.Data.doCalc(child1.Data.getNumberValue(), doRecursiveCalc(child2, x), x);
        //        }
        //    }
        //    else { // child 1 not MathNumber 
        //        if (child2.Data.getMathObjectType().equals(new MathNumber(0)))
        //        {
        //            return currentRoot.Data.doCalc(doRecursiveCalc(child1, x), child2.Data.getNumberValue(), x);
        //        }
        //        else {
        //            return currentRoot.Data.doCalc(doRecursiveCalc(child1, x), doRecursiveCalc(child2, x), x);
        //        }




        /// <summary>
        /// Creates a new Chromosome.
        /// </summary>
        /// <returns>The new.</returns>
        public override IChromosome CreateNew()
        {
            return new GeneticProgChromosome(gRangePeek);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            throw new NotImplementedException(); // not needed here - alternatively one may have to break the tree down into an array-ish structure
        }
    }
}