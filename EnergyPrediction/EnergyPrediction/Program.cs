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

namespace EnergyPrediction
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var AlgoTest = new GeneticAlgoController(new GeneticAlgoChromosome(2, 4),
                                                     new AlgoOnePointCrossover(),
                                                     new FitnessFunctions(),
                                                     new UniformMutation(),
                                                     new TournamentSelection(2),
                                                      0, 1000, 10,
                                                     new CombinedReinsertion(),
                                                     4000);
            AlgoTest.CrossoverProbability = 0.65f; //smaller = more parents less children - larger = less parents more children
            AlgoTest.MutationProbability = 0.05f; // according to guest lecture 1%< prob < 10%
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

            new Application().Run(new MainForm());
        }
    }
}
