//
// MIT LICENSE
//
// AlgoReliabilityFitness.cs
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

namespace EnergyPrediction
{
    public static class AlgoReliabilityFitness
    {
        public static double ReliabailityThreshold { get; set; } = 0.7;
        public static IList<IChromosome> CalculateDifference(List<GeneticAlgoChromosome> aParents, List<GeneticAlgoChromosome> aChildren)
        {
            IList<IChromosome> result = new List<IChromosome>();
            for (int i = 0; i < aChildren.Count; i++)
            {
                double[] lDifference = new double[aParents.Count];
                double childFitness = 0;
                double childReliability = 0;
                double devideBy = 0;
                for (int j = 0; j < aParents.Count; j++)
                {
                    lDifference[j] = Differnece(aChildren[i], aParents[j]);
                    lDifference[j] = (lDifference[j] < 0) ? 0 : (lDifference[j] > 1) ? 1 : lDifference[j];

                    childFitness += lDifference[j] * aParents[j].Reliability * (double)aParents[j].Fitness;
                    childReliability += Math.Pow(lDifference[j] * (double)aParents[j].Reliability, 2);
                    devideBy += lDifference[j] * aParents[j].Reliability;
                }

                childFitness = childFitness / devideBy;
                childReliability = childReliability / devideBy;

                childReliability = (childReliability < 0) ? 0 : (childReliability > 1) ? 1 : childReliability;

                if (childReliability > ReliabailityThreshold)
                {
                    aChildren[i].Reliability = childReliability;
                    aChildren[i].Fitness = childFitness;
                }
                result.Add(aChildren[i]);
            }

            return result;
        }

        private static double Differnece(GeneticAlgoChromosome aParent, GeneticAlgoChromosome aChild)
        {
            double lSum = 0;
            for (int i = 0; i < aParent.Length; i++)
            {
                lSum += Math.Abs((double)aChild.GetGene(i).Value - (double)aParent.GetGene(i).Value);
            }
            return lSum / aParent.Length;
        }
    }
}
