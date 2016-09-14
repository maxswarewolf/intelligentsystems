//
// MIT LICENSE
//
// DataIO.cs
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
using System.IO;
using System.Reflection;

namespace EnergyPrediction
{
    public static class DataIO
    {
        static DataType fDataSetType = DataType.Generator;

        public static DateTime MinDate { get; private set; } = DateTime.Parse("21/2/2015");
        public static DateTime MaxDate { get; private set; } = DateTime.Parse("7/9/2016");

        public static StateType StateSelection { get; set; } = StateType.VIC;
        public static DateTime StartDate { get; set; } = MinDate;
        public static DateTime EndDate { get; set; } = MaxDate;

        static List<double> fGenData = new List<double>();

        static List<StateFileData> fStateData = new List<StateFileData>();

        public static int getLength()
        {
            switch (fDataSetType)
            {
                case DataType.Generator:
                    return fGenData.Count;
                case DataType.State:
                    return fStateData.Count;
                case DataType.Applicance:
                case DataType.Test:
                default:
                    return 0;
            }
        }

        public static double getActualY(int x)
        {
            try
            {
                switch (fDataSetType)
                {
                    case DataType.Generator:
                        return fGenData[x];
                    case DataType.State:
                        return fStateData[x].MegaWatts;
                    case DataType.Applicance:
                    case DataType.Test:
                    default:
                        return Math.Sin(2 * x);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public static void Load5MinState(StateType aState, DateTime aStartDate, DateTime aEndDate)
        {
            fStateData.Clear();
            fDataSetType = DataType.State;
            List<string> lGenerators = new Config().GeneratorLinks[aState.ToString()];
            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/5min";
            string[] lFilePaths = Directory.GetFiles(Root, "*.csv", SearchOption.TopDirectoryOnly);
            List<string> lFiles = new List<string>();
            foreach (string s in lFilePaths)
            {
                lFiles.Add(Path.GetFileNameWithoutExtension(s));
            }
            DateTime currentTime = DateTime.Now;
            double value = 0;
            foreach (string s in lFiles)
            {
                DateTime temp = DateTime.Parse(s);
                if (temp >= aStartDate)
                {
                    if (temp > aEndDate)
                    {
                        break;
                    }
                    string path = Root + "/" + s + ".csv";
                    List<string> lColumns = new List<string>();
                    using (var reader = new CsvFileReader(path))
                    {
                        while (reader.ReadRow(lColumns))
                        {
                            if (lGenerators.Contains(lColumns[1]))
                            {
                                if (currentTime == DateTime.Parse(lColumns[0]))
                                {
                                    value += Double.Parse(lColumns[2]);
                                }
                                else {
                                    fStateData.Add(new StateFileData(aState, value));
                                    currentTime = DateTime.Parse(lColumns[0]);
                                    value = Double.Parse(lColumns[2]);
                                }
                            }
                        }
                    }
                    if (fStateData.Count > 1)
                        fStateData.RemoveAt(0);
                }
            }
        }

        public static void Load5MinGen(string aGenCode, DateTime aStartDate, DateTime aEndDate)
        {
            fGenData.Clear();
            fDataSetType = DataType.Generator;
            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/5min";
            string[] lFilePaths = Directory.GetFiles(Root, "*.csv", SearchOption.TopDirectoryOnly);
            List<string> lFiles = new List<string>();
            foreach (string s in lFilePaths)
            {
                lFiles.Add(Path.GetFileNameWithoutExtension(s));
            }

            foreach (string s in lFiles)
            {
                DateTime temp = DateTime.Parse(s);
                if (temp >= aStartDate)
                {
                    if (temp > aEndDate)
                    {
                        break;
                    }
                    string path = Root + "/" + s + ".csv";
                    List<string> lColumns = new List<string>();
                    using (var reader = new CsvFileReader(path))
                    {
                        while (reader.ReadRow(lColumns))
                        {
                            if (aGenCode.Equals(lColumns[1]))
                            {
                                fGenData.Add(Double.Parse(lColumns[2]));
                            }
                        }
                    }
                    //if (fStateData.Count > 1)
                    //    fStateData.RemoveAt(0);
                }
            }
        }

        public static List<string> getGenerators()
        {
            List<string> current = new List<string>();
            List<string> result = new List<string>();
            List<string> lVic = new Config().GeneratorLinks["VIC"];
            List<string> lAct = new Config().GeneratorLinks["ACT"];
            List<string> lTas = new Config().GeneratorLinks["TAS"];
            List<string> lSa = new Config().GeneratorLinks["SA"];
            List<string> lQld = new Config().GeneratorLinks["QLD"];
            List<string> lNsw = new Config().GeneratorLinks["NSW"];

            current.AddRange(lVic);
            current.AddRange(lAct);
            current.AddRange(lTas);
            current.AddRange(lSa);
            current.AddRange(lQld);
            current.AddRange(lNsw);

            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/5min";
            string[] lFilePaths = Directory.GetFiles(Root, "*.csv", SearchOption.TopDirectoryOnly);
            foreach (string s in lFilePaths)
            {
                List<string> lColumns = new List<string>();
                using (var reader = new CsvFileReader(s))
                {
                    while (reader.ReadRow(lColumns))
                    {
                        if (!current.Contains(lColumns[1]))
                        {
                            current.Add(lColumns[1]);
                            result.Add(lColumns[1]);
                        }
                    }
                }

            }
            return result;
        }

        public static void getData(DataType aType, string filename)
        {
            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data";


            string[] folders = Directory.GetDirectories(Root);
            Console.WriteLine(Root);
            foreach (string folder in folders)
            {
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
        }
        //todo: Add File IO
        //todo: Add Internal Storage
    }
}

