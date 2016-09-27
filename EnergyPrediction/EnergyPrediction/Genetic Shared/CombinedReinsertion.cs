//
// MIT LICENSE
//
// CombinedReinsertion.cs
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
using System.Linq;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;
//USED FOR TESTING THE REINSERTION CODE
namespace EnergyPrediction
{
    public class CombinedReinsertion : IReinsertion
    {
        public CombinedReinsertion()
        {
        }

        public bool CanCollapse { get; } = true;

        public bool CanExpand { get; } = false;

        /// <summary>
        /// Combines the parents with the children, and then returns a list the meets the population demands
        /// </summary>
        /// <returns>The chromosomes.</returns>
        /// <param name="population">Population.</param>
        /// <param name="offspring">Offspring.</param>
        /// <param name="parents">Parents.</param>
        public IList<IChromosome> SelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
        {
            List<IChromosome> lCombined = new List<IChromosome>();
            /// <summary>
            /// combine when size is greater than the max population.
            /// </summary>
            if (offspring.Count + parents.Count > population.MaxSize)
            {
                lCombined.AddRange(offspring);
                lCombined.AddRange(parents.OrderBy(c => c.Fitness).Take(population.MaxSize - lCombined.Count).ToList());
            }
            /// <summary>
            /// combine when population is less than min population.
            /// </summary>
            else if (offspring.Count + parents.Count < population.MinSize)
            {
                var diff = population.MaxSize - (offspring.Count + parents.Count);
                lCombined.AddRange(offspring);
                lCombined.AddRange(parents);
                lCombined.AddRange(parents.OrderByDescending(c => c.Fitness).Take(diff).ToList());
            }
            /// <summary>
            /// combine when inbetween the max in min population.
            /// </summary>
            else {
                lCombined.AddRange(offspring);
                lCombined.AddRange(parents);
            }

            if (lCombined.Count > population.MaxSize)
                return lCombined.Take(population.MaxSize).OrderBy(c => c.Fitness).ToList();

            return lCombined.OrderBy(c => c.Fitness).ToList();
        }
    }
}
