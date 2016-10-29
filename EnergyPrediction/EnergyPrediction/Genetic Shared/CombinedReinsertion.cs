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
using GeneticSharp.Domain.Fitnesses;
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
        /// Elists Reinsertion, adding the best chromosome to the next population, 
        /// and then adjusting the population to fit within the population.MinSize and population.MaxSize
        /// </summary>
        /// <returns>The chromosomes.</returns>
        /// <param name="population">Population.</param>
        /// <param name="offspring">Offspring.</param>
        /// <param name="parents">Parents.</param>
        public IList<IChromosome> SelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
        {
            List<IChromosome> lCombined = new List<IChromosome>();
            lCombined.AddRange(offspring);
            lCombined.Add(population.CurrentGeneration.BestChromosome);

            if (lCombined.Count < population.MinSize)
            {
                parents = parents.OrderBy(c => c.Fitness).ToList();
                var diff = population.MaxSize - lCombined.Count;
                lCombined.AddRange(parents.Take(diff).ToList());
            }
            else if (lCombined.Count > population.MaxSize)
            {
                lCombined = lCombined.Take(population.MaxSize).ToList();
            }
            //IFitness fit = new FitnessFunctions();
            foreach (IChromosome c in lCombined)
            {
                c.Fitness = null;
            }
            return lCombined.OrderBy(c => c.Fitness).ToList();
        }
    }
}
