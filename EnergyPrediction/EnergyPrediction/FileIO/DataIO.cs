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

        static List<double> fData = new List<double>();

        public static int getLength()
        {
            switch (fDataSetType)
            {
                case DataType.Generator:
                case DataType.State:
                    return fData.Count;
                case DataType.Applicance:
                case DataType.Test:
                    return 0;
            }
            return 0;
        }

        public static double getActualY(int x)
        {
            try
            {
                switch (fDataSetType)
                {
                    case DataType.Generator:
                    case DataType.State:
                        return fData[x];
                    case DataType.Applicance:
                    case DataType.Test:
                        return Math.Sin(2 * x);
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //Console.WriteLine(ex.Message);
                return 0;
            }
            return 0;
        }

        public static void LoadMin(StateType aState)
        {
            LoadMin(aState, StartDate, EndDate);
        }
        public static void LoadMin(string aGenCode)
        {
            LoadMin(aGenCode, StartDate, EndDate);
        }
        public static void LoadHalfHour(StateType aState)
        {
            LoadHalfHour(aState, StartDate, EndDate);
        }

        public static void LoadMin(StateType aState, DateTime aStartDate, DateTime aEndDate)
        {
            fData.Clear();
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
                                    fData.Add(value);
                                    currentTime = DateTime.Parse(lColumns[0]);
                                    value = Double.Parse(lColumns[2]);
                                }
                            }
                        }
                    }
                    if (fData.Count > 1)
                        fData.RemoveAt(0);
                }
            }
        }

        public static void LoadMin(string aGenCode, DateTime aStartDate, DateTime aEndDate)
        {
            fData.Clear();
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
                                fData.Add(Double.Parse(lColumns[2]));
                            }
                        }
                    }
                    //if (fStateData.Count > 1)
                    //    fStateData.RemoveAt(0);
                }
            }
        }

        public static void LoadHalfHour(StateType aState, DateTime aStartDate, DateTime aEndDate)
        {
            fData.Clear();
            fDataSetType = DataType.State;
            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/30min";
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
                            if (lColumns[1].Equals(aState + "1"))
                            {
                                fData.Add(Double.Parse(lColumns[2]));
                            }
                        }
                    }
                }
            }
        }

        public static void LoadMin(AppType aApp, DateTime aStartData, DateTime aEndDate)
        {

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

