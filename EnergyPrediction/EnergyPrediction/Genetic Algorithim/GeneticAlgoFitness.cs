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
    public class GeneticAlgoFitness : IFitness
    {
        double[] gExpectedResults = { Math.Sin(0),
                                  Math.Sin(2),
                                  Math.Sin(4),
                                  Math.Sin(6),
                                  Math.Sin(8),
                                  Math.Sin(10),
                                  Math.Sin(12),
                                  Math.Sin(14),
                                  Math.Sin(16),
                                  Math.Sin(18),
                                  Math.Sin(20),
                                  Math.Sin(22),
                                  Math.Sin(24),
                                  Math.Sin(26),
                                  Math.Sin(28),
                                  Math.Sin(30),
                                  Math.Sin(32),
                                  Math.Sin(34),
                                  Math.Sin(36),
                                  Math.Sin(38),
                                  Math.Sin(40)};

        /// <summary>
        /// Gives the sum of the difference between Expected and Calaulated results Squared
        /// </summary>
        /// <param name="aChromosome">A chromosome.</param>
        public double Evaluate(IChromosome aChromosome)
        {
            //Error of the Difference Squared
            var lChromosome = aChromosome as GeneticAlgoChromosome;

            double lErrorSum = 0.0;
            double lCalculatedY = 0.0;
            double lActualY = 0.0;
            for (int i = 0; i < gExpectedResults.Length; i++)
            {
                // A * Sin(Bx^C) + D
                lCalculatedY = (double)lChromosome.GetGene(0).Value * Math.Sin((double)lChromosome.GetGene(1).Value * Math.Pow(i, (double)lChromosome.GetGene(2).Value)) + (double)lChromosome.GetGene(3).Value;
                lActualY = gExpectedResults[i];
                lErrorSum -= Math.Pow(lActualY - lCalculatedY, 2);
            }
            return (lErrorSum);
        }
    }
}

