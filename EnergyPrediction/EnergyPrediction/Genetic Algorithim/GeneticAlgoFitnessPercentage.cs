//
// MIT LICENSE
//
// GeneticAlgoFitness.cs
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
using GeneticSharp.Domain.Fitnesses;
using EnergyPrediction;

namespace EnergyPrediction
{
    public class GeneticAlgoFitnessPercentage : IFitness
    {
        private CommonXY[] gExpectedResults;
        public GeneticAlgoFitnessPercentage(CommonXY[] aExpectedResults)
        {
            gExpectedResults = aExpectedResults;
        }

        /// <summary>
        /// Gives the sum of the difference between Expected and Calaulated results
        /// </summary>
        /// <param name="aChromosome">A chromosome.</param>
        public double Evaluate(IChromosome aChromosome)
        {
            var lChromosome = aChromosome as GeneticAlgoChromosome;

            double fitness = 0;
            double calculatedY = 0;
            for (int i = 0; i < gExpectedResults.Length; i++)
            {
                calculatedY = (double)aChromosome.GetGene(0).Value *
                                                Math.Sin((double)aChromosome.GetGene(1).Value *
                                                         Math.Pow(gExpectedResults[i].X, (double)aChromosome.GetGene(2).Value)) +
                                                (double)aChromosome.GetGene(3).Value;
                fitness += (Math.Sqrt(Math.Pow(gExpectedResults[i].Y - calculatedY, 2)) / gExpectedResults[i].Y);
            }

            fitness = 100 - (fitness * 100);
            return fitness;
        }
    }
}

