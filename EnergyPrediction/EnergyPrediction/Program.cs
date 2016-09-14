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
using Gtk;

namespace EnergyPrediction
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Config setup = new Config();
            //foreach (string key in setup.GeneratorLinks.Keys)
            //{
            //    DataIO.Load5MinState((StateType)Enum.Parse(typeof(StateType), key, true), DateTime.Parse("2015-02-21"), DateTime.Parse("2015-03-03"));
            //    Console.WriteLine("{0:N} MW", DataIO.getActualY(0));
            //}
            //foreach (string code in setup.GeneratorLinks["VIC"])
            //{
            //    DataIO.Load5MinGen(code, DateTime.Parse("2015-02-21"), DateTime.Parse("1/9/16"));
            //    for (int i = 0; i < DataIO.getLength(); i++)
            //    {
            //        Console.WriteLine("Gen:{1} {0:N} MW", DataIO.getActualY(0), code);
            //    }
            //    if (DataIO.getLength() > 0)
            //        Console.WriteLine();
            //}

            foreach (string s in DataIO.getGenerators())
            {
                Console.WriteLine(s);
            }

            //var test1 = new EnergyPrediction.GeneticAlgoController(200);
            //test1.Start();
            //Application.Init();
            //var win = new MainWindow();
            //win.Show();
            //Application.Run();
        }
    }
}
