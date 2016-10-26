//
// MIT LICENSE
//
// AssignmentAnalysis.cs
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
using System.IO;
using System.Collections.Generic;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;

namespace EnergyPrediction
{
    public struct test
    {
        public double time { get; set; }
        public int size { get; set; }
        public int length { get; set; }
        public string name { get; set; }
    }

    public class AssignmentAnalysis
    {
        float crossoverProbability = 0.65f;
        float mutationProbability = 0.05f;
        int generationCutoff = Int32.MaxValue;
        int valuePeek = 1000;
        int fitnessCutoff = 0;
        readonly string version;
        readonly List<test> TestToRun = new List<test>()
        {
            new test(){time = 15, size = 1000, length = 10, name = "Test 1: "},
            new test(){time = 30, size = 1000, length = 10, name = "Test 2: "},
            new test(){time = 45, size = 1000, length = 10, name = "Test 3: "},
            new test(){time = 60, size = 1000, length = 10, name = "Test 4: "},
            new test(){time = 30, size = 2000, length = 10, name = "Test 5: "},
            new test(){time = 30, size = 4000, length = 10, name = "Test 6: "},
            new test(){time = 30, size = 8000, length = 10, name = "Test 7: "},
            new test(){time = 30, size = 1000, length = 20, name = "Test 8: "},
            new test(){time = 30, size = 1000, length = 40, name = "Test 9: "}
        };

        public AssignmentAnalysis(string aType)
        {
            version = aType;
        }

        public void run()
        {
            ControllerBase temp;
            switch (version)
            {
                case "Prog":
                    foreach (test t in TestToRun)
                    {
                        temp = new GeneticProgController(new GeneticProgChromosome(valuePeek, t.length),
                                      new BranchCrossover(),
                                      new FitnessFunctions(),
                                      new UniformTreeMutation(),
                                      new TournamentSelection(),
                                                             fitnessCutoff, generationCutoff, t.time,
                                      new CombinedReinsertion(),
                                                             t.size / 2);
                        temp.CrossoverProbability = crossoverProbability;
                        temp.MutationProbability = mutationProbability;
                        Console.WriteLine("Genetic Programming {0} {1} Minute Cut-off || Solution Depth {2} || Population Range: {3} - {4}", t.name, t.time, t.length, t.size, t.size * 2);
                        temp.Start();
                        Console.WriteLine("Finished {0} {1} Minute Cut-off || Solution Depth {2} || Population Range: {3} - {4}", t.name, t.time, t.length, t.size, t.size * 2);
                        using (StreamWriter lWriter = File.AppendText("ProgTesting.txt"))
                        {
                            lWriter.WriteLine(t.name + t.time + " Minute Cut-off || Solution Depth " + t.length + " || Population Range: " + t.size + " - " + t.size * 2 + "\n\tEquation: " + temp.Chromosome + "\n" + temp.prediction());
                        }
                        Console.WriteLine();
                    }
                    break;
                default:
                    foreach (test t in TestToRun)
                    {
                        temp = new GeneticAlgoController(new GeneticAlgoChromosome(valuePeek, t.length),
                                      new AlgoOnePointCrossover(),
                                      new FitnessFunctions(),
                                      new UniformMutation(),
                                      new TournamentSelection(),
                                                             fitnessCutoff, generationCutoff, t.time,
                                      new CombinedReinsertion(),
                                                             t.size);
                        temp.CrossoverProbability = crossoverProbability;
                        temp.MutationProbability = mutationProbability;
                        Console.WriteLine("Genetic Algorithm {0} {1} Minute Cut-off || Solution Depth {2} || Population Range: {3} - {4}", t.name, t.time, t.length, t.size, t.size * 2);
                        temp.Start();
                        Console.WriteLine("Finished {0} {1} Minute Cut-off || Solution Depth {2} || Population Range: {3} - {4}", t.name, t.time, t.length, t.size, t.size * 2);
                        using (StreamWriter lWriter = File.AppendText("AlgoTesting.txt"))
                        {
                            lWriter.WriteLine(t.name + t.time + " Minute Cut-off || Solution Depth " + t.length + " || Population Range: " + t.size + " - " + t.size * 2 + "\n\tEquation: " + temp.Chromosome + "\n" + temp.prediction());
                        }
                        Console.WriteLine();
                    }
                    break;
            }

        }
    }
}
