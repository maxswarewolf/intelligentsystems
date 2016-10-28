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

        public GeneticAlgoController(IChromosome aChromo, ICrossover aCross, IFitness aFit, IMutation aMut, ISelection aSel, int aFitnessThres, int aGenCap, double MaxElapMin, IReinsertion aRein, int aPop)
            : base(aChromo, aCross, aFit, aMut, aSel, aRein, aFitnessThres, aGenCap, MaxElapMin, aPop)
        { }

        public override bool DefaultDrawGeneration(Generation aGeneration)
        {
            return true;
        }

        public override string prediction()
        {
            GeneticAlgoChromosome temp = fGA.BestChromosome as GeneticAlgoChromosome;
            double lNextDay = temp.getCalculatedY(DataIO.getLength());
            double lNextWeek = temp.getCalculatedY(DataIO.getLength() + 7);
            double lNextMonth = temp.getCalculatedY(DataIO.getLength() + 30);
            double lNextQuarter = temp.getCalculatedY(DataIO.getLength() + 91);

            double lTotalWeek = 0, lTotalMonth = 0, lTotalQuarter = 0;
            for (int i = 0; i < 91; i++)
            {
                double value = temp.getCalculatedY(DataIO.getLength() + i);
                lTotalWeek += (i < 7) ? value : 0;
                lTotalMonth += (i < 30) ? value : 0;
                lTotalQuarter += value;
            }

            double lActualNextDay = DataIO.getPredictY(0);
            double lActualNextWeek = DataIO.getPredictY(7);
            double lActualNextMonth = DataIO.getPredictY(30);
            double lActualNextQuarter = DataIO.getPredictY(91);

            double lActualTotalWeek = 0, lActualTotalMonth = 0, lActualTotalQuarter = 0;
            for (int i = 0; i < 91; i++)
            {
                double value = DataIO.getPredictY(i);
                lActualTotalWeek += (i < 7) ? value : 0;
                lActualTotalMonth += (i < 30) ? value : 0;
                lActualTotalQuarter += value;
            }

            String predictionString = "";
            predictionString += "Next Day:      " + lNextDay + "W predicted || " + lActualNextDay + "W actual \n";
            predictionString += "Next Week:     " + lNextWeek + "W predicted || " + lActualNextWeek + "W actual \n";
            predictionString += "Next Month:    " + lNextMonth + "W predicted || " + lActualNextMonth + "W actual \n";
            predictionString += "Next Quarter:  " + lNextQuarter + "W predicted || " + lActualNextQuarter + "W actual \n";
            predictionString += "\n";
            predictionString += "Aggerate Next Week:     " + lTotalWeek + "W predicted || " + lActualTotalWeek + "W actual \n";
            predictionString += "Aggerate Next Month:    " + lTotalMonth + "W predicted || " + lActualTotalMonth + "W actual \n";
            predictionString += "Aggerate Next Quarter:  " + lTotalQuarter + "W predicted || " + lActualTotalQuarter + "W actual \n";

            return predictionString;
        }

        public override void Start()
        {
            IPopulation lPop = new Population(PopulationCount, PopulationCount * 2, Chromosome);
            lPop.GenerationStrategy = new PerformanceGenerationStrategy(100);
            fGA = new GeneticAlgorithm(lPop, Fitness, Selection, Crossover, Mutation);
            fGA.Termination = Termination;
            fGA.CrossoverProbability = CrossoverProbability;
            fGA.MutationProbability = MutationProbability;
            fGA.Reinsertion = Reinsertion;

            base.Start();
        }
    }
}

