﻿//
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
        public IPopulation fPopulation { get; protected set; }
        public IChromosome Chromosome { get; protected set; }
        public ICrossover Crossover { get; protected set; }
        public IFitness Fitness { get; protected set; }
        public IMutation Mutation { get; protected set; }
        public ISelection Selection { get; protected set; }
        public ITermination Termination { get; protected set; }
        public IReinsertion Reinsertion { get; protected set; }
        public int FitnessThreshold { get; protected set; }
        public int GenerationCap { get; protected set; }
        public int MaxElapsedMin { get; protected set; }
        public int PopulationCount { get; protected set; }
        public float CrossoverProbability { get; set; } = 0.8f;
        public float MutationProbability { get; set; } = 0.4f;
        public GeneticAlgorithmState State { get { return fGA.State; } }
        protected GeneticAlgorithm fGA { get; set; }
        protected List<Func<IChromosome, bool>> fGenerationRanEventChromosome = new List<Func<IChromosome, bool>>();
        protected List<Func<Generation, bool>> fGenerationRanEventGeneration = new List<Func<Generation, bool>>();

        public ControllerBase(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, IReinsertion aRein, int aFitnessThres, int aGenCap, double MaxElapMin, int aPop)
        {
            Chromosome = aChromo;
            Crossover = aCross;
            Fitness = aFit;
            Mutation = aMut;
            Selection = aSel;
            Termination = new OrTermination(new FitnessThresholdTermination(aFitnessThres), new GenerationNumberTermination(aGenCap), new TimeEvolvingTermination(System.TimeSpan.FromMinutes(MaxElapMin)));
            Reinsertion = aRein;
            PopulationCount = aPop;

            // Defined in the calling child
            InitPopulation();
            fPopulation.GenerationStrategy = new PerformanceGenerationStrategy(100);

            fGA = new GeneticAlgorithm(fPopulation, Fitness, Selection, Crossover, Mutation);
            fGA.Termination = Termination;
            fGA.CrossoverProbability = CrossoverProbability;
            fGA.MutationProbability = MutationProbability;
            fGA.Reinsertion = Reinsertion;
        }

        public ControllerBase(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, IReinsertion aRein, ITermination aTer, int aPop)
        {
            Chromosome = aChromo;
            Crossover = aCross;
            Fitness = aFit;
            Mutation = aMut;
            Selection = aSel;
            Termination = aTer;
            Reinsertion = aRein;
            PopulationCount = aPop;

            // Defined in the calling child
            InitPopulation();
            fPopulation.GenerationStrategy = new PerformanceGenerationStrategy(100);

            fGA = new GeneticAlgorithm(fPopulation, Fitness, Selection, Crossover, Mutation);
            fGA.Termination = Termination;
            fGA.CrossoverProbability = CrossoverProbability;
            fGA.MutationProbability = MutationProbability;
            fGA.Reinsertion = Reinsertion;
        }

        /// <summary>
        /// To be called by ControllerBase upon construction.
        /// </summary>
        protected abstract void InitPopulation();

        /// <summary>
        /// Adds an Event to be ran at the end of each generation, which takes the parameter of a Chromosome
        /// </summary>
        /// <param name="aFunction">A function.</param>
        public virtual void addEventFunction(Func<IChromosome, bool> aFunction)
        {
            fGenerationRanEventChromosome.Add(aFunction);
        }

        /// <summary>
        /// Removes an Event to be ran at the end of each generation, which takes the parameter of a Chromosome 
        /// </summary>
        /// <param name="aFunction">A function.</param>
        public virtual void removeEventFunction(Func<IChromosome, bool> aFunction)
        {
            fGenerationRanEventChromosome.Remove(aFunction);
        }

        /// <summary>
        /// Adds an Event to be ran at the end of each generation, which takes the whole generation as a parameter
        /// </summary>
        /// <param name="aFunction">A function.</param>
        public virtual void addEventFunction(Func<Generation, bool> aFunction)
        {
            fGenerationRanEventGeneration.Add(aFunction);
        }

        /// <summary>
        /// Removes an Event to be ran at the end of each generation, which takes the whole generation as a parameter
        /// </summary>
        /// <param name="aFunction">A function.</param>
        public virtual void removeEventFunction(Func<Generation, bool> aFunction)
        {
            fGenerationRanEventGeneration.Remove(aFunction);
        }

        /// <summary>
        /// Prediction this instance.
        /// </summary>
        public abstract string prediction();

        /// <summary>
        /// Defaults the draw chromosome.
        /// </summary>
        /// <returns><c>true</c>, if draw chromosome was defaulted, <c>false</c> otherwise.</returns>
        /// <param name="aChromosome">A chromosome.</param>
        public virtual bool DefaultDrawChromosome(IChromosome aChromosome)
        {
            if (fGA.Population.GenerationsNumber % 25 == 0)
            {
                Console.WriteLine("Generations: {0}, Population Size: {1}", fGA.Population.GenerationsNumber, fGA.Population.CurrentGeneration.Chromosomes.Count);
                Console.WriteLine("Time: {0}", fGA.TimeEvolving);
                Console.WriteLine(aChromosome);
            }
            return true;
        }

        /// <summary>
        /// Defaults the draw generation.
        /// </summary>
        /// <returns><c>true</c>, if draw generation was defaulted, <c>false</c> otherwise.</returns>
        /// <param name="aGeneration">A generation.</param>
        public abstract bool DefaultDrawGeneration(Generation aGeneration);

        /// <summary>
        /// Start this instance of the Genetic solution
        /// </summary>
        public virtual void Start()
        {
            foreach (Func<Generation, bool> action in fGenerationRanEventGeneration)
            {
                fGA.GenerationRan += delegate
                {
                    action(fGA.Population.CurrentGeneration);
                };
            }
            foreach (Func<IChromosome, bool> action in fGenerationRanEventChromosome)
            {
                fGA.GenerationRan += delegate
                {
                    action(fGA.Population.BestChromosome);
                };
            }

            fGA.Start();

            Chromosome = fGA.BestChromosome;
            Console.WriteLine("Time: {0}", fGA.TimeEvolving);
            Console.WriteLine("Best Solution: {0}", fGA.BestChromosome);
        }

        /// <summary>
        /// Stop this instance of the genetic solution
        /// </summary>
        public virtual void Stop()
        {
            fGA.Stop();
        }

        /// <summary>
        /// Resume this stoped instance of the genetic solution
        /// if the instance isn't stoped, it has no effect
        /// </summary>
        public virtual void Resume()
        {
            fGA.Resume();
        }
    }
}