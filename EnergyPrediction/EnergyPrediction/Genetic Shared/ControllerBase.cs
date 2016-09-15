//
// MIT LICENSE
//
// ControllerInterface.cs
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
using System.Collections.Generic;

namespace EnergyPrediction
{
    public abstract class ControllerBase
    {
        public IChromosome Chromosome { get; set; }
        public ICrossover Crossover { get; set; }
        public IFitness Fitness { get; set; }
        public IMutation Mutation { get; set; }
        public ISelection Selection { get; set; }
        public ITermination Termination { get; set; }
        public IReinsertion Reinsertion { get; set; }
        public int PopulationCount { get; set; }
        public float CrossoverProbability { get; set; } = 0.8f;
        public float MutationProbability { get; set; } = 0.4f;
        protected GeneticAlgorithm fGA { get; set; }
        protected List<Func<IChromosome, bool>> fGenerationRanEventFunctions = new List<Func<IChromosome, bool>>();

        public ControllerBase(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, ITermination aTer, IReinsertion aRein, int aPop)
        {
            Chromosome = aChromo;
            Crossover = aCross;
            Fitness = aFit;
            Mutation = aMut;
            Selection = aSel;
            Termination = aTer;
            Reinsertion = aRein;
            PopulationCount = aPop;
        }

        public virtual void addEventFunction(Func<IChromosome, bool> aFunction)
        {
            fGenerationRanEventFunctions.Add(aFunction);
        }

        public virtual void removeEventFunction(Func<IChromosome, bool> aFunction)
        {
            fGenerationRanEventFunctions.Remove(aFunction);
        }

        public abstract bool DefaultDraw(IChromosome aChromosome);
        public virtual void Start()
        {
            IPopulation lPop = new ProgPopulation(PopulationCount, PopulationCount * 2, Chromosome);
            //todo: Dicusss the Need or want for a generation strategy 
            fGA = new GeneticAlgorithm(lPop, Fitness, Selection, Crossover, Mutation);
            fGA.Termination = Termination;
            fGA.CrossoverProbability = CrossoverProbability;
            fGA.MutationProbability = MutationProbability;
            fGA.Reinsertion = Reinsertion;


            foreach (Func<IChromosome, bool> action in fGenerationRanEventFunctions)
            {
                fGA.GenerationRan += delegate
                {
                    action(fGA.Population.BestChromosome);
                };
            }

            try
            {
                fGA.Start();
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
        }
    }
}