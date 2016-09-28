//
// MIT LICENSE
//
// AlgoOnePointCrossover.cs
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
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
namespace EnergyPrediction
{
    public class AlgoOnePointCrossover : CrossoverBase
    {
        public AlgoOnePointCrossover() : base(2, 2, 4)
        {
        }

        /// <summary>
        /// Performs the crossover, as well as dose pre fitness to speed up the whole process
        /// </summary>
        /// <returns>The cross.</returns>
        /// <param name="parents">Parents.</param>
        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            var firstParent = parents[0] as GeneticAlgoChromosome;
            var secondParent = parents[1] as GeneticAlgoChromosome;

            IList<IChromosome> temp = CreateChildren(firstParent, secondParent);
            List<GeneticAlgoChromosome> Children = new List<GeneticAlgoChromosome>() { (GeneticAlgoChromosome)temp[0], (GeneticAlgoChromosome)temp[1] };
            List<GeneticAlgoChromosome> lParents = new List<GeneticAlgoChromosome>() { firstParent, secondParent };

            return AlgoReliabilityFitness.CalculateDifference(lParents, Children);

            //for (int i = 0; i < children.Count; i++)
            //{
            //    double S1 = 1 - Difference(children[i], firstParent);
            //    S1 = (S1 < 0) ? 0 : (S1 > 1) ? 1 : S1;
            //    double S2 = 1 - Difference(children[i], secondParent);
            //    S2 = (S2 < 0) ? 0 : (S2 > 1) ? 1 : S2;

            //    double childFitness = ((S1 * firstParent.Reliability * (double)firstParent.Fitness) + (S2 * secondParent.Reliability * (double)secondParent.Fitness)) / ((S1 * firstParent.Reliability) + (S2 * secondParent.Reliability));
            //    double childReliability = (Math.Pow(S1 * firstParent.Reliability, 2) + Math.Pow(S2 * secondParent.Reliability, 2)) / ((S1 * firstParent.Reliability) + (S2 * secondParent.Reliability));
            //    childReliability = (childReliability < 0) ? 0 : (childReliability > 1) ? 1 : childReliability;

            //    if (childReliability > 0.70)
            //    {
            //        children[i].Reliability = childReliability;
            //        children[i].Fitness = childFitness;
            //    }
            //}

            //return new List<IChromosome>() { children[0], children[1] };
        }

        protected double Difference(IChromosome child, IChromosome parent)
        {
            double sum = 0;
            for (int i = 0; i < parent.Length; i++)
            {
                sum += Math.Abs((double)child.GetGene(i).Value - (double)parent.GetGene(i).Value);
            }
            return sum / parent.Length;
        }

        protected IList<IChromosome> CreateChildren(IChromosome firstParent, IChromosome secondParent)
        {
            var firstChild = CreateChild(firstParent, secondParent);
            var secondChild = CreateChild(secondParent, firstParent);

            return new List<IChromosome>() { firstChild, secondChild };
        }

        protected virtual IChromosome CreateChild(IChromosome leftParent, IChromosome rightParent)
        {
            var cutGenesCount = 2;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.GetGenes().Take(cutGenesCount).ToArray());
            child.ReplaceGenes(cutGenesCount, rightParent.GetGenes().Skip(cutGenesCount).ToArray());

            return child;
        }
    }
}
