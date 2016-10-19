//
// MIT LICENSE
//
// GeneticAlgoController.cs
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
    public class GeneticAlgoController : ControllerBase
    {
        public GeneticAlgoController(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, ITermination aTer, IReinsertion aRein, int aPop)
            : base(aChromo, aCross, aFit, aMut, aSel, aRein, aTer, aPop)
        { }

        public GeneticAlgoController(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, int aFitnessThres, int aGenCap, int MaxElapMin, IReinsertion aRein, int aPop)
            : base(aChromo, aCross, aFit, aMut, aSel, aRein, aFitnessThres, aGenCap, MaxElapMin, aPop)
        { }

        public override bool DefaultDraw(IChromosome aChromosome)
        {
            //var lBest = aChromosome.GetGenes();
            var lChromosome = aChromosome as GeneticAlgoChromosome;
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Generations: {0}, Population Size: {1}", fGA.Population.GenerationsNumber, fGA.Population.CurrentGeneration.Chromosomes.Count);
            Console.WriteLine(lChromosome);
            Console.WriteLine("Time: {0}", fGA.TimeEvolving);
            return true;
        }

        public override bool DefaultDrawGeneration(Generation aGeneration)
        {
            //foreach (GeneticAlgoChromosome gAC in aGeneration.Chromosomes){

            //}
            Console.WriteLine();
            return true;
        }

        public override void Start()
        {
            IPopulation lPop = new Population(PopulationCount, PopulationCount * 2, Chromosome);
            lPop.GenerationStrategy = new PerformanceGenerationStrategy(100);
            //todo: Dicusss the Need or want for a generation strategy 
            fGA = new GeneticAlgorithm(lPop, Fitness, Selection, Crossover, Mutation);
            fGA.Termination = Termination;
            fGA.CrossoverProbability = CrossoverProbability;
            fGA.MutationProbability = MutationProbability;
            fGA.Reinsertion = Reinsertion;

            base.Start();

            var lBest = fGA.Population.BestChromosome.GetGenes();
            Console.WriteLine();
            Console.WriteLine("Evolved: " + lBest[0].Value + ", " + lBest[1].Value + ", " + lBest[2].Value + ", " + lBest[3].Value);
        }
    }
}

