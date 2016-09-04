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
    public class GeneticAlgoController
    {
        public GeneticAlgorithm GA { get; private set; }
        int gPopulationSize;
        //y = Sin(2x)

        public GeneticAlgoController(int aPopulationSize)
        {
            gPopulationSize = aPopulationSize;
        }

        public ITermination CreateTermination()
        {
            //return new FitnessThresholdTermination(44);
            //return new GenerationNumberTermination(1000);
            return new TimeEvolvingTermination(TimeSpan.FromMinutes(4));
        }

        public IChromosome CreateChromosome()
        {
            return new GeneticAlgoChromosome(10);
        }

        public IFitness CreateFitness()
        {
            return new GeneticAlgoFitness();
        }

        public ICrossover CreateCrossover()
        {
            return new OnePointCrossover(2);
        }

        public IMutation CreateMutation()
        {
            return new UniformMutation();
        }

        public ISelection CreateSelection()
        {
            return new EliteSelection();
        }

        public IPopulation CreatePopulation()
        {
            var lPopulation = new Population(gPopulationSize, gPopulationSize * 2, CreateChromosome());
            //lPopulation.GenerationStrategy = new PerformanceGenerationStrategy();
            return lPopulation;
        }

        public void Draw(IChromosome aBestChromosome)
        {
            var lBest = aBestChromosome.GetGenes();
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Generations: {0}", GA.Population.GenerationsNumber);
            Console.WriteLine("Fitness: {0}", aBestChromosome.Fitness);
            Console.WriteLine("Genes: {0:00.####}, {1:00.####}, {2:00.####}, {3:00.####}", lBest[0].Value, lBest[1].Value, lBest[2].Value, lBest[3].Value);
            Console.WriteLine("Time: {0}", GA.TimeEvolving);

        }

        public void Start()
        {
            GA = new GeneticAlgorithm(CreatePopulation(), CreateFitness(), CreateSelection(), CreateCrossover(), CreateMutation());
            GA.Termination = CreateTermination();
            GA.CrossoverProbability = 0.90f;
            GA.MutationProbability = 0.2f;
            GA.Reinsertion = new ElitistReinsertion();

            Console.WriteLine("STARTING...");

            var lTerminationName = GA.Termination.GetType().Name;
            GA.GenerationRan += delegate
            {
                var lBestChromosome = GA.Population.BestChromosome;
                Draw(lBestChromosome);
            };

            try
            {
                GA.Start();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Error Message: {0}", ex.Message);
                Console.WriteLine("Stack: {0}", ex.StackTrace);
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
            var lBest = GA.Population.BestChromosome.GetGenes();
            Console.WriteLine();
            Console.WriteLine("Evolved: " + lBest[0].Value + ", " + lBest[1].Value + ", " + lBest[2].Value + ", " + lBest[3].Value);
        }

    }
}

