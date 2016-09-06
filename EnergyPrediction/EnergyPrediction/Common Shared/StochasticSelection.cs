//
// MIT LICENSE
//
// StochasticUniversalSamplingSelection.cs
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
using System.ComponentModel;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
namespace EnergyPrediction
{
    public class StochasticSelection : SelectionBase
    {
        #region Fields
        double gPercentage = 0.1;
        #endregion

        #region Construstors
        public StochasticSelection(double aPercentage) : base(2)
        {
            if (aPercentage > 100)
            {
                gPercentage = aPercentage % 100;
                gPercentage = gPercentage / 100;
            }
            else if (aPercentage > 0)
            {
                gPercentage = (aPercentage / 100);
            }
        }

        public StochasticSelection() : base(2)
        { }
        #endregion

        /// <summary>
        /// Performs the select chromosomes.
        /// It provides an even spread of then entire range
        /// </summary>
        /// <returns>The select chromosomes.</returns>
        /// <param name="number">Number.</param>
        /// <param name="generation">Generation.</param>
        protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
        {
            var r = new Random(DateTime.Now.Second);
            int lNumGeneration = (int)(number * gPercentage);
            double lTotalFitness = 0;

            var lOrdered = generation.Chromosomes.OrderByDescending(c => c.Fitness);
            foreach (IChromosome c in lOrdered)
            {
                if (Double.IsNaN((double)c.Fitness))
                {
                    c.Fitness = double.MinValue;
                }
                lTotalFitness += (double)c.Fitness;
            }

            double lDistance = lTotalFitness / lNumGeneration;
            int lStartingPoint = r.Next((int)lDistance, 0);
            IList<double> lPointers = new List<double>();

            for (int i = 0; i < lNumGeneration - 1; i++)
            {
                lPointers.Add(lStartingPoint + (i * lDistance));
            }

            IList<IChromosome> lOrderedSet = lOrdered.ToArray();
            IList<IChromosome> lKeep = new List<IChromosome>();

            foreach (var P in lPointers)
            {
                int i = 0;
                double aFitness = 0;
                while (aFitness < P)
                {
                    aFitness += (double)lOrderedSet[i].Fitness;
                    i++;
                }
                lKeep.Add(lOrderedSet[i]);
            }

            return lKeep;
        }
    }
}

