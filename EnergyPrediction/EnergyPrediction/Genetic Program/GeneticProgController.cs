//
// MIT LICENSE
//
// GeneticProgController.cs
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
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;

namespace EnergyPrediction
{
    public class GeneticProgController : ControllerBase
    {
        public GeneticProgController(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, ITermination aTer, IReinsertion aRein, int aPop)
            : base(aChromo, aCross, aFit, aMut, aSel, aRein, aTer, aPop)
        { }

        public GeneticProgController(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, int aFitnessThres, int aGenCap, int MaxElapMin, IReinsertion aRein, int aPop)
            : base(aChromo, aCross, aFit, aMut, aSel, aRein, aFitnessThres, aGenCap, MaxElapMin, aPop)
        { }

        public override bool DefaultDraw(IChromosome aChromosome)
        {
            Console.WriteLine();
            GeneticProgChromosome lChromo = aChromosome as GeneticProgChromosome;
            Console.WriteLine("Generation: {0}", fGA.Population.CurrentGeneration.Number);
            Console.WriteLine("Equation: {0}", lChromo.Root);
            Console.WriteLine("Fitness: {0}", lChromo.Fitness);

            //Console.Clear();
            //Console.WriteLine();
            //Console.WriteLine("Generations: {0}", fGA.Population.GenerationsNumber);
            //Console.WriteLine("Fitness: {0}", aChromosome.Fitness);
            //Console.WriteLine("Time: {0}", fGA.TimeEvolving);
            return true;
        }

        public override bool DefaultDrawGeneration(Generation aGeneration)
        {
            foreach (GeneticProgChromosome gPC in aGeneration.Chromosomes)
            {
                Console.WriteLine(gPC.Root);
                Console.WriteLine();
            }
            return true;
        }

        public override void Start()
        {
            IPopulation lPop = new ProgPopulation(PopulationCount, PopulationCount * 2, Chromosome);
            lPop.GenerationStrategy = new PerformanceGenerationStrategy(100);

            fGA = new GeneticAlgorithm(lPop, Fitness, Selection, Crossover, Mutation);
            fGA.Termination = Termination;
            fGA.CrossoverProbability = CrossoverProbability;
            fGA.MutationProbability = MutationProbability;
            fGA.Reinsertion = Reinsertion;

            base.Start();

            var lBest = (GeneticProgChromosome)fGA.Population.BestChromosome;
            //Console.WriteLine(lBest.Root);
            //todo: add in final display of best chromosome or other display data
        }
    }
}

