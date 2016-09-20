//
// MIT LICENSE
//
// GeneticAlgoGenome.cs
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
namespace EnergyPrediction
{
    public class GeneticAlgoChromosome : ChromosomeBase, ChromosomeExt
    {
        public double RangePeek { get; private set; }
        private double OldFitness = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:EnergyPrediction.GeneticAlgoChromosome"/> class.
        /// </summary>
        /// <param name="aResultRange">A result range. eg -20 to 20</param>
        public GeneticAlgoChromosome(double aResultPeek, int aNumberOfGenes) : base(aNumberOfGenes)
        {
            RangePeek = aResultPeek;

            for (int i = 0; i < Length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }
        protected GeneticAlgoChromosome(double aResultPeek, int aNumberOfGenes, double aOldFitness) : this(aResultPeek, aNumberOfGenes)
        {
            OldFitness = aOldFitness;
        }

        public double getCalculatedY(int x)
        {
            return (int)this.GetGene(0).Value * Math.Sin((int)this.GetGene(1).Value * Math.Pow(x, (int)this.GetGene(2).Value)) + (int)this.GetGene(3).Value;
        }

        /// <summary>
        /// Generates a gene with a random integer between -RangePeek and RangePeek.
        /// </summary>
        /// <returns>The gene.</returns>
        /// <param name="geneIndex">Gene index.</param>
        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(Randomizer.NextDouble(RangePeek * -1, RangePeek + 1));
        }

        /// <summary>
        /// Creates a new Chromosome.
        /// </summary>
        /// <returns>The new.</returns>
        public override IChromosome CreateNew()
        {
            double value = 0;
            if (Fitness > OldFitness)
            {
                value = RangePeek + 2;
            }
            else if (Fitness < OldFitness)
            {
                value = RangePeek - 2;
            }
            else {
                value = RangePeek / 2;
            }
            return new GeneticAlgoChromosome(value, Length, (double)Fitness);
        }
    }
}

