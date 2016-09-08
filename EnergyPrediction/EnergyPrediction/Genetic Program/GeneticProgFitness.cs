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
    // todo:  what has to be on which abstraction level ??????


    public class GeneticProgFitness : IFitness
    {
        /// <summary>
        /// Gives the sum of the difference between Expected and Calaulated results Squared
        /// </summary>
        /// <param name="aChromosome">A chromosome.</param>
        double IFitness.Evaluate(GeneticProgChromosome aChromosome)
        {
            //Error of the Difference Squared
            var lChromosome = aChromosome as GeneticProgChromosome;

            //initial values
            double lErrorSum = 0.0;
            double lCalculatedY = 0.0;
            double lActualY = 0.0;

            for (int x = -100; x < 100; x++) // todo: make x range abstrakt! put in config file
            {
                // generic function instead of fixed such as A * Sin(Bx^C) + D
                lCalculatedY = aChromosome.getCalculatedY( x);
                lActualY = getActualY(x); 
                lErrorSum += Math.Pow(lActualY - lCalculatedY, 2);
            }
            return (-1 * lErrorSum);
        }

        double getActualY(int x) // todo: move to abstract place!
        {
            return Math.Sin(2 * x);
        }
   }
}

