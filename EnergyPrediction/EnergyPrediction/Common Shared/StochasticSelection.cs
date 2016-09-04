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
        public StochasticSelection() : base(2)
        {

        }
        protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
        {
            var r = new Random(DateTime.Now.Second);

            long lTotalFitness = 0;
            foreach (IChromosome c in generation.Chromosomes)
            {
                if ((c.Fitness != null) && (c.Fitness >= 0))
                {
                    lTotalFitness += (long)Math.Round((double)c.Fitness);
                    Console.WriteLine("Fitness: {0}, Total Fitness: {1}", c.Fitness, lTotalFitness);

                }
            }
            foreach (IChromosome c in generation.Chromosomes)
            {
                c.Fitness = c.Fitness / lTotalFitness;
            }
            var lOrdered = generation.Chromosomes.OrderByDescending(c => c.Fitness);
            var lNumOffspring = number;
            var lDistance = lTotalFitness / lNumOffspring;

            var lStartingPoint = r.Next(0, (int)lDistance);
            IList<double> lPointers = new IList<double>();
            for (int i = 0; i < lNumOffspring - 1; i++)
            {
                lPointers.Add(lStartingPoint + (i * lDistance));
            }
            IList<IChromosome> lOrderedSet = lOrdered.ToArray();
            IList<IChromosome> lKeep = null;
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

