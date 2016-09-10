//
// MIT LICENSE
//
// GeneticLinker.cs
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
using System.Collections.Generic;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Terminations;

namespace EnergyPrediction
{
    public class Config
    {
        public Config()
        {
            SolutionOptions = new List<string>() { "Genetic Algorithm", "Genetic Programming" };

            CrossoversAlgo = new List<string>() { "Uniform", "One Point", "Two Point", "Three Parent" };
            CrossoversProg = new List<string>() { "PLACE HOLDER" };

            Fitness = new List<string>() { "Error Sum" };

            MutationAlgo = new List<string>() { "Uniform", "Twors", "Reverse Sequence" };
            MutationProg = new List<string>() { "Uniform", "Branch", "Reverse Sequence" };

            Selection = new List<string>() { "Elite", "Stochastic", "Inverse Elite" };

            Reinsertion = new List<string>() { "PLACE HOLDER" };

        }

        #region Default Options
        public List<string> SolutionOptions { get; private set; }
        public List<string> CrossoversAlgo { get; private set; }
        public List<string> CrossoversProg { get; private set; }
        public List<string> Fitness { get; private set; }
        public List<string> MutationAlgo { get; private set; }
        public List<string> MutationProg { get; private set; }
        public List<string> Selection { get; private set; }
        public List<string> Reinsertion { get; private set; }
        #endregion

        #region Option Choice
        public int DefaultTerminationTimeMinutes { get; private set; }
        public int DefaultTerminationGenerationThreshold { get; private set; }
        public double DefaultTerminationFitnessThreshold { get; private set; }
        public double DefaultCrossoverProbility { get; private set; }
        public double DefaultMutationProbility { get; private set; }
        public int DefaultPopulation { get; private set; }
        #endregion
    }
}

