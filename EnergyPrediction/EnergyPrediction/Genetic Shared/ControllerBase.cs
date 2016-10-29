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

        // These events occur at the end of each generation
        protected List<Func<IChromosome, bool>> fBestChromEvents = new List<Func<IChromosome, bool>>();
        protected List<Func<Generation, bool>> fGenerationEvents = new List<Func<Generation, bool>>();

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
        }

        // Add an event that will be triggered at the end of a generation, accepting the best chromosome of that generation
        public virtual void addbestChromEvent(Func<IChromosome, bool> aFunction)
        {
            fBestChromEvents.Add(aFunction);
        }

        // Remove an event that will be triggered at the end of a generation, accepting the best chromosome of that generation
        public virtual void removeBestChromEvent(Func<IChromosome, bool> aFunction)
        {
            fBestChromEvents.Remove(aFunction);
        }

        // Add an event that will be triggered at the end of a generation, accepting that generation
        public virtual void addGenerationEvent(Func<Generation, bool> aFunction)
        {
            fGenerationEvents.Add(aFunction);
        }

        // Remove an event that will be triggered at the end of a generation, accepting that generation
        public virtual void removeGenerationEvent(Func<Generation, bool> aFunction)
        {
            fGenerationEvents.Remove(aFunction);
        }

        public abstract string prediction();

        public virtual bool DefaultDraw(IChromosome aChromosome)
        {
            double cal = 150 - 16 * Math.Log(PopulationCount);
            int modBy = (cal < 10) ? 10 : (int)cal;
            if (fGA.Population.GenerationsNumber % 1 == 0)
            {
                Console.WriteLine("Generations: {0}, Population Size: {1}", fGA.Population.GenerationsNumber, fGA.Population.CurrentGeneration.Chromosomes.Count);
                Console.WriteLine("Time: {0}", fGA.TimeEvolving);
                Console.WriteLine(aChromosome);
            }
            return true;
        }
        public abstract bool DefaultDrawGeneration(Generation aGeneration);
        public virtual void Start()
        {
            foreach (Func<Generation, bool> action in fGenerationEvents)
            {
                fGA.GenerationRan += delegate
                {
                    action(fGA.Population.CurrentGeneration);
                };
            }
            foreach (Func<IChromosome, bool> action in fBestChromEvents)
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

            //Console.WriteLine("Generations: {0}, Population Size: {1}", fGA.Population.GenerationsNumber, fGA.Population.CurrentGeneration.Chromosomes.Count);
            Console.WriteLine("Time: {0}", fGA.TimeEvolving);
        }
        public virtual void Stop()
        {
            fGA.Stop();
        }
        public virtual void Resume()
        {
            fGA.Resume();
        }
    }
}