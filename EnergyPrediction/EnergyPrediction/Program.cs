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
using System.Threading;
using EnergyPrediction.UI;
using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;

namespace EnergyPrediction
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // If there is a command line argument
            if (args.Length >= 1)
            {
                if (args[0] == "testing")
                {
                    // Do batch testing
                    DataIO.AggregateData(StateType.VIC, DateTime.Parse("1/1/16"), DateTime.Parse("1/2/16"), 288);
                    DataIO.SaveAggregateData();
                    //AssignmentAnalysis AA = new AssignmentAnalysis("Prog");
                    //AA.run();
                    AssignmentAnalysis AA = new AssignmentAnalysis("Algo");
                    AA.run();
                }
            }
            else // Run the program as normal
            {
                object _lock = new object();
                Queue<Func<IChromosome, bool>> chromDelegateQueue = new Queue<Func<IChromosome, bool>>();
                Queue<Func<string, bool>> stringDelegateQueue = new Queue<Func<string, bool>>();
                AutoResetEvent signal = new AutoResetEvent(false);

                // And a thread for the results window
                Thread resultThread = new Thread(() =>
                {
                    Gtk.Application.Init();

                    ResultsWindow resultsWindow = new ResultsWindow();

                    lock (_lock)
                    {
                        chromDelegateQueue.Enqueue(resultsWindow.UpdateResults);
                        stringDelegateQueue.Enqueue(resultsWindow.UpdateAxisTitle);
                        stringDelegateQueue.Enqueue(resultsWindow.UpdatePredictionSteps);
                    }

                    signal.Set();

                    Gtk.Application.Run();
                });
                resultThread.Start();

                Thread parameterThread = new Thread(() =>
                {
                    Eto.Forms.Application formsApplication = new Eto.Forms.Application();

                    ParameterForm parameterForm = new ParameterForm();

                    signal.WaitOne();

                    lock (_lock)
                    {
                        ParameterStack containedStack = parameterForm.Content as ParameterStack;
                        if (containedStack != null)
                        {
                            containedStack.updateResultsDelegate = chromDelegateQueue.Dequeue();
                            containedStack.updateAxisTitleDelegate = stringDelegateQueue.Dequeue();
                            containedStack.updatePredictionStepsDelegate = stringDelegateQueue.Dequeue();
                        }
                        else throw new InvalidCastException("Could not covert parameterForm.Content to ParameterStack - this should never occur");
                    }

                    new Eto.Forms.Application().Run(parameterForm);
                });
                parameterThread.Start();
            }
        }
    }
}
