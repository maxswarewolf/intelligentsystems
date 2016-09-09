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
            for (int x = -100; x < 100; x++)
            {
                // A * Sin(Bx^C) + D
                lCalculatedY = lChromosome.getCalculatedY(x);
                    //(int)lChromosome.GetGene(0).Value * Math.Sin((int)lChromosome.GetGene(1).Value * Math.Pow(x, (int)lChromosome.GetGene(2).Value)) + (int)lChromosome.GetGene(3).Value;
                lActualY = Math.Sin(2 * x);
                lErrorSum += Math.Pow(lActualY - lCalculatedY, 2);
            }
            return (-1 * lErrorSum);
        }
    }
}

