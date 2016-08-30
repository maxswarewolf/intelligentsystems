//
// MIT LICENSE
//
// ErrorValueFitness.cs
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
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
namespace EnergyPrediction
{
    public class ErrorValueFitness : IFitness;
    {
        /// <summary>
        /// Evaluate the specified aChromosome and realValues error to give a fitness value
        /// In this case the lower the value the better the score. 
        /// </summary>
        /// <param name="aChromosome">A chromosome.</param>
        /// <param name="realValues">Real values.</param>
        public double Evaluate(IChromosome aChromosome, double[] realValues)
        {
            double lErrorValue = 0.0;
            if (aChromosome.Length == realValues.Length)
            {
                for (int i = 0; i < aChromosome.Length; i++)
                {
                    lErrorValue += Math.sqrt(Math.Pow((int)aChromosome.GetGene(i).Value - realValues[i], 2));
                }
            }
            else {
                throw new Exception();
            }
        }
    }
}

