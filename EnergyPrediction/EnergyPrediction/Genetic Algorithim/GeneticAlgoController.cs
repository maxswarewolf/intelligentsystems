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
namespace EnergyPrediction
{
    public class GeneticAlgoController
    {
        GeneticAlgorithm GA { get; private set; };
        int gPopulationSize;
        //y = Sin(x) + 2
        CommonXY[] gDataSet = new CommonXY[]
        {
            new CommonXY(0, 2) ,
            new CommonXY(30, 2.5),
            new CommonXY(90, 3),
            new CommonXY(150, 2.5),
            new CommonXY(180, 2),
            new CommonXY(210, 1.5),
            new CommonXY(270, 1),
            new CommonXY(330, 1.5),
            new CommonXY(360, 2) ,
            new CommonXY(390, 2.5),
            new CommonXY(450, 3),
            new CommonXY(510, 2.5),
            new CommonXY(540, 2),
            new CommonXY(570, 1.5),
            new CommonXY(630, 1),
            new CommonXY(690, 1.5),
            new CommonXY(720, 2)
        };

        public GeneticAlgoController(int aPopulationSize)
        {
            gPopulationSize = aPopulationSize;
        }

        public ITermination CreateTermination()
        {
            return new FitnessThresholdTermination(0.02);
        }

        public IChromosome CreateChromosome()
        {
            return new GeneticAlgoChromosome(50);
        }

        public IFitness CreateFitness()
        {
            return new GeneticAlgoFitnessPercentage(gDataSet);
        }

        public ICrossover CreateCrossover()
        {
            return new OnePointCrossover(2);
        }

        public IMutation CreateMutation()
        {
            return new UniformMutation();
        }


    }
}

