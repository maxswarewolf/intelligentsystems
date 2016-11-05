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
        public double Reliability { get; set; } = 1;

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

        public double getCalculatedY(double x)
        {
            //return ((double)this.GetGene(0).Value / Math.Sqrt((double)this.GetGene(1).Value * Math.PI)) * (Math.Pow(Math.E, -Math.Pow(x - (double)this.GetGene(2).Value, 2) / (2 * (double)this.GetGene(3).Value)));
            //return (double)this.GetGene(0).Value * Math.Sin((double)this.GetGene(1).Value * (double)this.GetGene(2).Value) + (double)this.GetGene(3).Value;
            double result = 0;
            for (int i = 0; i < Length; i++)
            {
                result += (double)GetGene(i).Value * Math.Pow(x, i);
            }
            return result;
        }

        /// <summary>
        /// Generates a gene with a random integer between -RangePeek and RangePeek.
        /// </summary>
        /// <returns>The gene.</returns>
        /// <param name="geneIndex">Gene index.</param>
        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(Math.Round(Randomizer.NextDouble(RangePeek * -1, RangePeek), 4));
        }

        /// <summary>
        /// Creates a new Chromosome.
        /// </summary>
        /// <returns>The new.</returns>
        public override IChromosome CreateNew()
        {
            return new GeneticAlgoChromosome(RangePeek, Length);
        }

        public override string ToString()
        {
            string result = "y = ";
            if ((double)GetGene(0).Value < 0.0)
            {
                result += " -";
            }
            for (int i = 0; i < Length; i++)
            {
                result += Math.Abs((double)GetGene(i).Value) + " * x^" + i;
                if (i < Length - 1)
                {
                    if ((double)GetGene(i + 1).Value < 0.0)
                    {
                        result += " - ";
                    }
                    else {
                        result += " + ";
                    }
                }
            }
            result += "\n";
            result += "Reliability: " + Reliability + "\n";
            result += "Fitness: " + Fitness + "\n";
            result += "+- Peek Value: " + RangePeek + "\n";
            return result;
        }
    }
}

