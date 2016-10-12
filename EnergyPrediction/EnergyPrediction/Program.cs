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
        //todo: add batch testing functionality
        public static void Main(string[] args)
        {
            if (args.Length >= 8)
            {
                int fitnessThresh = Int32.Parse(args[1]);
                int GenThresh = Int32.Parse(args[2]);
                int MinTimeOut = Int32.Parse(args[3]);
                int Pop = Int32.Parse(args[4]);
                int ResPeek = Int32.Parse(args[5]);
                int Length = Int32.Parse(args[6]);
                int interations = Int32.Parse(args[7]);

                ControllerBase Test = null;
                DataIO.AggregateData(AppType.Fridge, DateTime.Parse("31/7/15"), DateTime.Parse("2/8/15"), 1);
                for (int i = 0; i < interations; i++)
                {
                    switch (args[0])
                    {
                        case "Algo":
                            Console.WriteLine("Starting Genetic Algorithim Batch Run");
                            Test = new GeneticAlgoController(new GeneticAlgoChromosome(3, 4),
                                                         new AlgoOnePointCrossover(),
                                                         new FitnessFunctions(),
                                                         new UniformMutation(),
                                                         new TournamentSelection(2),
                                                             fitnessThresh, GenThresh, MinTimeOut,
                                                         new CombinedReinsertion(),
                                                         Pop);
                            break;
                        case "Prog":
                            Console.WriteLine("Starting Genetic Programming Batch Run");
                            Test = new GeneticProgController(new GeneticProgChromosome(3, 6),
                                                         new BranchCrossover(),
                                                         new FitnessFunctions(),
                                                         new UniformTreeMutation(),
                                                         new TournamentSelection(2),
                                                         fitnessThresh, GenThresh, MinTimeOut,
                                                         new CombinedReinsertion(), Pop);
                            break;
                    }

                    if (!Test.Equals(null))
                    {
                        Test.CrossoverProbability = 0.65f;
                        Test.MutationProbability = 0.1f;
                        Test.addEventFunction(Test.DefaultDraw);
                        Test.Start();
                        Console.WriteLine("FINSIHED\n");
                    }
                    else {
                        Console.WriteLine("Something Went Wrong");
                    }
                }
            }
            else {
                new Application().Run(new MainForm());
            }
        }
    }
}
