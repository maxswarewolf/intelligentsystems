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
        //todo: NEED TO COMMENT EVERYTHING
        //todo: NEED TO FIX UP OTHER LOOPS BASED ON THE 5MIN GEN
        //todo: ADD Ageration based on the increments
        static bool Testing = true;

        static IDictionary<string, List<string>> GeneratorLinks = new Dictionary<string, List<string>>() {
                { "ACT", new List<string>() { "ROYALLA1" }},
                { "VIC", new List<string>() { "RUBICON", "CLOVER", "YAMBUKWF", "MLWF1", "CHALLHWF", "PORTWF", "WAUBRAWF"} },
                { "TAS", new List<string>() { "BBDISEL1", "BUTLERSG", "CATAGUN1", "CLUNY", "GEORGTN1", "GEORGTN2", "MEADOWB2", "PALOONA", "PORTLAT1", "QUERIVE1", "REPULSE", "ROWALLAN", "WOOLNTH1"} },
                { "SA" , new List<string>() { "CNUNDAWF", "ANGAS1", "ANGAS2", "WPWF", "CATHROCK", "LKBONNY1", "MTMILLAR", "STARHLWF"} },
                { "QLD", new List<string>() { "PIONEER", "CALL_A_4", "GERMCRK", "MBAHNTH", "RPCG", "INVICTA"} },
                { "NSW", new List<string>() { "GB01", "CAPTL_WF", "CULLRGWF", "ERGT01"} }
            };

        public static DateTime MinDate { get; private set; } = DateTime.Parse("21/2/2015");
        public static DateTime MaxDate { get; private set; } = DateTime.Parse("7/9/2016");

        public static StateType StateSelection { get; set; } = StateType.VIC;
        public static DateTime StartDate { get; set; } = MinDate;
        public static DateTime EndDate { get; set; } = MaxDate;

        static List<double> fData = new List<double>();

        public static int getLength()
        {
            if (Testing)
            {
                return 1000;
            }
            return fData.Count;
        }

        public static double getActualY(int x)
        {
            if (Testing)
            {
                return 1.5 * Math.Sin(2 * Math.Pow(x, 2)) + 3.3;
            }
            if (x >= 0 && x < fData.Count)
                return fData[x];
            return double.PositiveInfinity;
        }

        public static void LoadMin(StateType aState)
        {
            if (Testing)
                return;
            LoadMin(aState, StartDate, EndDate);
        }
        public static void LoadMin(string aGenCode)
        {
            if (Testing)
                return;
            LoadMin(aGenCode, StartDate, EndDate);
        }
        public static void LoadMin(AppType aApp)
        {
            if (Testing)
                return;
            LoadMin(aApp, StartDate, EndDate);
        }

        public static void LoadMin(StateType aState, DateTime aStartDate, DateTime aEndDate)
        {
            if (Testing)
                return;
            fData.Clear();
            List<string> lGenerators = GeneratorLinks[aState.ToString()];

            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/5min";

            string[] lFilePaths = Directory.GetFiles(Root, "*.csv", SearchOption.TopDirectoryOnly);

            List<string> lFiles = new List<string>();

            foreach (string s in lFilePaths)
            {
                lFiles.Add(Path.GetFileNameWithoutExtension(s));
            }

            DateTime timeStamp = DateTime.MinValue; //init value - not used

            double wattage = 0;
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
                                if (timeStamp == DateTime.Parse(lColumns[0]))
                                {
                                    wattage += Double.Parse(lColumns[2]);
                                }
                                else
                                {
                                    if (timeStamp != DateTime.MinValue)
                                    {
                                        fData.Add(wattage);
                                    }
                                    timeStamp = DateTime.Parse(lColumns[0]);
                                    wattage = Double.Parse(lColumns[2]);
                                }
                            }
                        }
                        fData.Add(wattage);
                    }
                }
            }
        }

        public static void LoadMin(string aGenCode, DateTime aStartDate, DateTime aEndDate)
        {
            if (Testing)
                return;
            fData.Clear();
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
                }
            }
        }

        public static void LoadMin(AppType aApp, DateTime aStartData, DateTime aEndDate)
        {
            if (Testing)
                return;
            fData.Clear();
            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/App";
            string[] lFilePaths = Directory.GetFiles(Root, "*.csv", SearchOption.TopDirectoryOnly);
            List<string> lFiles = new List<string>();
            foreach (string s in lFilePaths)
            {
                lFiles.Add(Path.GetFileNameWithoutExtension(s));
            }
            foreach (string s in lFiles)
            {
                DateTime temp = DateTime.Parse(s.Substring(0, 10));
                if (temp >= aStartData)
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
                            fData.Add(Double.Parse(lColumns[(int)aApp]));
                        }
                    }
                }
            }
        }
    }
}

