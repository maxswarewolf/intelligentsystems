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
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Terminations;

namespace EnergyPrediction
{
    public class GeneticLinker : GeneticLinkerInterface, UILinkerInterface
    {
        #region Fields
        IDictionary<string, string> gAlgorithimOptions = new Dictionary<string, string>() {
            {"Genetic Algorithm", "A"},
            {"Genetic Program", "P"}
        };

        IDictionary<string, string> gCrossoverOptions = new Dictionary<string, string>() {
            {"UNI-A", "Uniform"},
            {"ONE-A", "One Point"},
            {"TWO-A", "Two Point"},
            {"PAR-A", "Three Parent"},
            {"SWP-P", "Branch Swap - Place Holder"},
            {"SWR-P", "Branch Swap and Rotate - Place Holder"},
            {"UNI-P", "Uniform - Place Holder"}
        };

        IDictionary<string, string> gFitnessOptions = new Dictionary<string, string>(){
            {"ESUM-A","Error Sum"},
            {"ESUM-P","Error Sum"}
        };

        IDictionary<string, string> gMutationOptions = new Dictionary<string, string>(){
            {"UNI-A", "Uniform"},
            {"UNI-P", "Uniform"},
            {"WOR-A", "Twors"},
            {"BRA-P", "Branch"},
            {"RSM-A", "Reverse Sequence Mutation"},
            {"RSM-P", "Reverse Sequence Mutation"}
        };

        IDictionary<string, string> gSelectionOptions = new Dictionary<string, string>() {
            {"ELITE", "Elite"},
            {"STOCH", "Stochastic Uniform Sampling"},
            {"I-ELE", "Inverse Elite"},
            {"TOURM", "Tournament"}
        };

        IDictionary<string, string> gTerminationOptions = new Dictionary<string, string>() {
            {"T-MIN", "Time in Minutes"},
            {"N-GEN", "Generation Number"},
            {"T-FIT", "Fitness Threshold"},
            {"OR-TE", "OR Termination"}
        };

        IDictionary<string, string> gReinsertionOptions = new Dictionary<string, string>(){
            {"ELITE","Elite"},
            {"FITNE","Fitness Based"},
            {"IPURE","Pure"},
            {"UNIFO","Uniform"}
        };
        #endregion

        public List<string> getAlgorithims()
        {
            throw new NotImplementedException();
        }
        public List<string> getCrossovers()
        {
            throw new NotImplementedException();
        }
        public List<string> getFitnessFunctions()
        {
            throw new NotImplementedException();
        }
        public List<string> getMutations()
        {
            throw new NotImplementedException();
        }
        public List<string> getSelections()
        {
            throw new NotImplementedException();
        }
        public List<string> getTerminations()
        {
            throw new NotImplementedException();
        }
        public List<string> getReinsertions()
        {
            throw new NotImplementedException();
        }


        public GeneticAlgorithm getConfig()
        {
            throw new NotImplementedException();
        }
        public float getCrossoverProbility()
        {
            throw new NotImplementedException();
        }
        public float getMutationProbility()
        {
            throw new NotImplementedException();
        }
        public int getPopulationSize()
        {
            throw new NotImplementedException();
        }
        public IReinsertion getReinsertion()
        {
            throw new NotImplementedException();
        }
        public ITermination getTermination()
        {
            throw new NotImplementedException();
        }
        public int getTimeOut()
        {
            throw new NotImplementedException();
        }


        #region SetFunctions
        public bool seCrossover(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setAlgorithm(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setFitnessFunctions(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setMutation(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setPopulationSize(int aSize)
        {
            throw new NotImplementedException();
        }

        public bool setResertion(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setSelection(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setTermination(string aOption)
        {
            throw new NotImplementedException();
        }

        public bool setTimeOut(int aSize)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

