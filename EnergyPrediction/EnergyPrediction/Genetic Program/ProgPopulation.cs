//
// MIT LICENSE
//
// ProgPopulation.cs
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
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;

namespace EnergyPrediction
{
    public class ProgPopulation : Population
    {
        public ProgPopulation(int minSize, int maxSize, IChromosome adamChromosome) : base(minSize, maxSize, adamChromosome)
        {
        }

        public override void CreateInitialGeneration()
        {
            Generations = new List<Generation>();
            GenerationsNumber = 0;

            var lListChromo = new List<IChromosome>();

            for (int i = 0; i < MinSize; i++)
            {
                var lChromosome = AdamChromosome.CreateNew();

                if (lChromosome == null)
                {
                    throw new InvalidOperationException("The Adam chromosome's 'CreateNew' method generated a null chromosome. This is a invalid behavior, please, check your chromosome code.");
                }

                lListChromo.Add(lChromosome);
            }

            CreateNewGeneration(lListChromo);
        }

        public override void CreateNewGeneration(System.Collections.Generic.IList<IChromosome> chromosomes)
        {
            CurrentGeneration = new Generation(++GenerationsNumber, chromosomes);
            Generations.Add(CurrentGeneration);
            GenerationStrategy.RegisterNewGeneration(this);
        }

        public override void EndCurrentGeneration()
        {
            CurrentGeneration.End(MaxSize);

            if (BestChromosome != CurrentGeneration.BestChromosome)
            {
                BestChromosome = CurrentGeneration.BestChromosome;
                OnBestChromosomeChanged(EventArgs.Empty);
            }
        }
    }
}
