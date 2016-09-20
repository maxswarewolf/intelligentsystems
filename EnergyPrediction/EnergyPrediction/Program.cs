//
// MIT LICENSE
//
// Program.cs
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
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Reinsertions;
using Eto.Forms;
using Eto.GtkSharp;

namespace EnergyPrediction
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            DataIO.LoadMin(StateType.VIC, DateTime.Parse("1/9/16"), DateTime.Parse("1/9/16"));

            var AlgoTest = new GeneticAlgoController(new GeneticAlgoChromosome(1000, 4),
                                                     new OnePointCrossover(2),
                                                     new ErrorSquaredFitness(),
                                                     new TworsMutation(),
                                                     new EliteSelection(),
                                                     new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(TimeSpan.FromSeconds(90))),
                                                     new ElitistReinsertion(), 200);
            AlgoTest.CrossoverProbability = 0.6f;
            AlgoTest.MutationProbability = 0.6f;
            AlgoTest.addEventFunction(AlgoTest.DefaultDraw);
            AlgoTest.Start();

            //var ProgTest = new GeneticProgController(new GeneticProgChromosome(10, 3),
            //                                         new BranchCrossover(),
            //                                         new ErrorSquaredFitness(),
            //                                         new UniformTreeMutation(),
            //                                         new EliteSelection(),
            //                                         new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(TimeSpan.FromMinutes(1))),
            //                                         new ElitistReinsertion(), 200);
            //ProgTest.CrossoverProbability = 0.6f;
            //ProgTest.MutationProbability = 0.6f;
            //ProgTest.addEventFunction(ProgTest.DefaultDraw);
            //ProgTest.Start();

            new Application().Run(new MainWindow());
        }
    }
}
