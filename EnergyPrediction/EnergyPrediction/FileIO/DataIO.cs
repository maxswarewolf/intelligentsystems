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

        /// <summary>
        /// The toggle for testing or use real data.
        /// </summary>
        static bool Testing = false;

        /// <summary>
        /// The generator links.A
        /// </summary>
        public readonly static IDictionary<string, List<string>> GeneratorLinks = new Dictionary<string, List<string>>() {
                { "ACT", new List<string>() { "ROYALLA1" }},
                { "VIC", new List<string>() { "RUBICON", "CLOVER", "YAMBUKWF", "MLWF1", "CHALLHWF", "PORTWF", "WAUBRAWF"} },
                { "TAS", new List<string>() { "BBDISEL1", "BUTLERSG", "CATAGUN1", "CLUNY", "GEORGTN1", "GEORGTN2", "MEADOWB2", "PALOONA", "PORTLAT1", "QUERIVE1", "REPULSE", "ROWALLAN", "WOOLNTH1"} },
                { "SA" , new List<string>() { "CNUNDAWF", "ANGAS1", "ANGAS2", "WPWF", "CATHROCK", "LKBONNY1", "MTMILLAR", "STARHLWF"} },
                { "QLD", new List<string>() { "PIONEER", "CALL_A_4", "GERMCRK", "MBAHNTH", "RPCG", "INVICTA"} },
                { "NSW", new List<string>() { "GB01", "CAPTL_WF", "CULLRGWF", "ERGT01"} }
            };

        /// <summary>
        /// Gets the minimum date.
        /// </summary>
        /// <value>The minimum date.</value>
        public static DateTime MinDate { get; private set; } = DateTime.Parse("21/2/2015");
        /// <summary>
        /// Gets the max date.
        /// </summary>
        /// <value>The max date.</value>
        public static DateTime MaxDate { get; private set; } = DateTime.Parse("7/9/2016");

        /// <summary>
        /// Gets or sets the state selection.
        /// </summary>
        /// <value>The state selection.</value>
        public static StateType StateSelection { get; set; } = StateType.VIC;
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public static DateTime StartDate { get; set; } = MinDate;
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public static DateTime EndDate { get; set; } = MaxDate;
        /// <summary>
        /// The data storage for imparted data.
        /// </summary>
        static List<double> fData = new List<double>();
        static List<double> fPrediction = new List<double>();

        /// <summary>
        /// Gets the length, of the data imported or returns 1000 if testing is turned on.
        /// </summary>
        /// <returns>The length.</returns>
        public static int getLength()
        {
            return (Testing) ? 1000 : fData.Count;
        }

        /// <summary>
        /// Gets the imported data relating to the supplied index point x
        /// if testing is true then it will return a value based on a Sin equation.
        /// </summary>
        /// <returns>The actual y.</returns>
        /// <param name="x">The x coordinate.</param>
        public static double getActualY(int x)
        {
            if (Testing)
            {
                //return (1 / Math.Sqrt(2 * Math.PI)) * (Math.Pow(Math.E, -Math.Pow(x - 0, 2) / (2 * 0.5)));
                return -1 * Math.Sin(2 * Math.Pow(x, 1)) + 0;
            }
            return (x >= 0 && x < fData.Count) ? fData[x] : double.PositiveInfinity;
        }

        public static double getPredictY(int x)
        {
            return (x >= 0 && x < fPrediction.Count) ? fPrediction[x] : double.PositiveInfinity;
        }
        public static int getPredictLength()
        {
            return fPrediction.Count;
        }

        public static void SaveAggerateData()
        {
            using (StreamWriter writer = File.AppendText(StateSelection + "-" + DateTime.Now.DayOfYear + ".csv"))
            {
                foreach (double d in fData)
                {
                    writer.WriteLine(d);
                }
            }
        }

        /// <summary>
        /// Loads the 5 Minute data based on state, for full data range
        /// </summary>
        /// <param name="aState">A state.</param>
        public static void LoadData(StateType aState)
        {
            if (!Testing)
                LoadData(aState, StartDate, EndDate);
        }

        /// <summary>
        /// Loads the 5 Minute data based on Generator Code, for full data range
        /// </summary>
        /// <param name="aGenCode">A gen code.</param>
        public static void LoadData(string aGenCode)
        {
            if (!Testing)
                LoadData(aGenCode, StartDate, EndDate);
        }

        /// <summary>
        /// Loads the 5 Minute data based on Appliance, for full data range
        /// </summary>
        /// <param name="aApp">A app.</param>
        public static void LoadData(AppType aApp)
        {
            if (!Testing)
                LoadData(aApp, StartDate, EndDate);
        }

        /// <summary>
        /// Loads the 5 Minute data based on state, for partial or full data range
        /// </summary>
        /// <param name="aState">A state.</param>
        /// <param name="aStartDate">A start date.</param>
        /// <param name="aEndDate">A end date.</param>
        public static void LoadData(StateType aState, DateTime aStartDate, DateTime aEndDate)
        {
            if (Testing)
                return;
            fData = new List<double>();
            List<string> lGenerators = GeneratorLinks[aState.ToString()];

            string lRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/5min";

            string[] lFilePaths = Directory.GetFiles(lRoot, "*.csv", SearchOption.TopDirectoryOnly);

            List<string> lFiles = new List<string>();

            foreach (string s in lFilePaths)
            {
                lFiles.Add(Path.GetFileNameWithoutExtension(s));
            }

            DateTime lTimeStamp = DateTime.MinValue; //init value - not used

            double lWattage = 0;
            foreach (string s in lFiles)
            {
                DateTime temp = DateTime.Parse(s);
                if (temp >= aStartDate)
                {
                    string path = lRoot + "/" + s + ".csv";
                    List<string> lColumns = new List<string>();
                    using (var lReader = new CsvFileReader(path))
                    {
                        if (temp > aEndDate)
                        {
                            while (lReader.ReadRow(lColumns))
                            {
                                if (lGenerators.Contains(lColumns[1]))
                                {
                                    if (lTimeStamp == DateTime.Parse(lColumns[0]))
                                    {
                                        lWattage += Double.Parse(lColumns[2]);
                                    }
                                    else
                                    {
                                        if (lTimeStamp != DateTime.MinValue)
                                        {
                                            fPrediction.Add(lWattage);
                                        }
                                        lTimeStamp = DateTime.Parse(lColumns[0]);
                                        lWattage = Double.Parse(lColumns[2]);
                                    }
                                }
                            }
                            fPrediction.Add(lWattage);
                        }
                        else {
                            while (lReader.ReadRow(lColumns))
                            {
                                if (lGenerators.Contains(lColumns[1]))
                                {
                                    if (lTimeStamp == DateTime.Parse(lColumns[0]))
                                    {
                                        lWattage += Double.Parse(lColumns[2]);
                                    }
                                    else
                                    {
                                        if (lTimeStamp != DateTime.MinValue)
                                        {
                                            fData.Add(lWattage);
                                        }
                                        lTimeStamp = DateTime.Parse(lColumns[0]);
                                        lWattage = Double.Parse(lColumns[2]);
                                    }
                                }
                            }
                            fData.Add(lWattage);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the 5 Minute data based on Generator Code, for partial or full data range
        /// </summary>
        /// <param name="aGenCode">A gen code.</param>
        /// <param name="aStartDate">A start date.</param>
        /// <param name="aEndDate">A end date.</param>
        public static void LoadData(string aGenCode, DateTime aStartDate, DateTime aEndDate)
        {
            if (Testing)
                return;
            fData = new List<double>();
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
                    string path = Root + "/" + s + ".csv";
                    List<string> lColumns = new List<string>();
                    using (var reader = new CsvFileReader(path))
                    {
                        while (reader.ReadRow(lColumns))
                        {
                            if (aGenCode.Equals(lColumns[1]))
                            {
                                if (temp > aEndDate)
                                {
                                    fPrediction.Add(Double.Parse(lColumns[2]));
                                }
                                else {
                                    fData.Add(Double.Parse(lColumns[2]));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the 5 Minute data based on Appliance, for partial or full data range
        /// </summary>
        /// <param name="aApp">A app.</param>
        /// <param name="aStartData">A start data.</param>
        /// <param name="aEndDate">A end date.</param>
        public static void LoadData(AppType aApp, DateTime aStartData, DateTime aEndDate)
        {
            if (Testing)
                return;
            fData = new List<double>();
            string Root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Data/App";
            string[] lFilePaths = Directory.GetFiles(Root, "*.csv", SearchOption.TopDirectoryOnly);
            List<string> lFiles = new List<string>();
            foreach (string s in lFilePaths)
            {
                lFiles.Add(Path.GetFileNameWithoutExtension(s));
            }
            bool lGrabAll = ((int)aApp >= 12);
            foreach (string s in lFiles)
            {
                DateTime temp = DateTime.Parse(s);
                if (temp >= aStartData)
                {
                    string path = Root + "/" + s + ".csv";
                    List<string> lColumns = new List<string>();
                    using (var reader = new CsvFileReader(path))
                    {
                        while (reader.ReadRow(lColumns))
                        {
                            double total = 0;
                            if (lGrabAll)
                            {
                                foreach (String col in lColumns)
                                {
                                    total += Double.Parse(col);
                                }
                            }
                            else {
                                total = Double.Parse(lColumns[(int)aApp]);
                            }
                            if (temp > aEndDate)
                            {
                                fPrediction.Add(total);
                            }
                            else {
                                fData.Add(total);
                            }

                        }
                    }
                }
            }
        }

        public static void AggregateData(StateType aState, int aTimeInterval)
        {
            AggregateData(aState, StartDate, EndDate, aTimeInterval);
        }

        public static void AggregateData(string aGenCode, int aTimeInterval)
        {
            AggregateData(aGenCode, StartDate, EndDate, aTimeInterval);
        }

        public static void AggregateData(AppType aApp, int aTimeInterval)
        {
            AggregateData(aApp, StartDate, EndDate, aTimeInterval);
        }

        public static void AggregateData(StateType aState, DateTime aStartDate, DateTime aEndDate, int aTimeInterval)
        {
            if (!Testing)
            {
                LoadData(aState, aStartDate, aEndDate);
                AggregateDataSet(aTimeInterval);
            }
        }

        public static void AggregateData(string aGenCode, DateTime aStartDate, DateTime aEndDate, int aTimeInterval)
        {
            if (!Testing)
            {
                LoadData(aGenCode, aStartDate, aEndDate);
                AggregateDataSet(aTimeInterval);
            }
        }

        public static void AggregateData(AppType aApp, DateTime aStartDate, DateTime aEndDate, int aNumTimeIntervals)
        {
            if (!Testing)
            {
                LoadData(aApp, aStartDate, aEndDate);
                AggregateDataSet(aNumTimeIntervals);
            }
        }

        private static void AggregateDataSet(int aTimeInterval)
        {
            List<double> lTemp = new List<double>();
            double lWattage = 0;
            for (int i = 0; i < fData.Count; i++)
            {
                if ((i + 1) % aTimeInterval == 0)
                {
                    lTemp.Add(lWattage);
                    lWattage = 0;
                }
                lWattage += fData[i];
            }
            if (fData.Count % aTimeInterval != 0)
            {
                lTemp.Add(lWattage);
            }
            fData = new List<double>();
            fData.AddRange(lTemp);

            lTemp = new List<double>();
            lWattage = 0;
            for (int i = 0; i < fPrediction.Count; i++)
            {
                if ((i + 1) % aTimeInterval == 0)
                {
                    lTemp.Add(lWattage);
                    lWattage = 0;
                }
                lWattage += fPrediction[i];
            }
            if (fPrediction.Count % aTimeInterval != 0)
            {
                lTemp.Add(lWattage);
            }
            fPrediction = new List<double>();
            fPrediction.AddRange(lTemp);
        }

        public static void TestingFunction(int a)
        {
            bool testingTemp = Testing;
            Testing = false;
            Console.WriteLine("\nLoad Data Test");
            LoadData(AppType.All, DateTime.Parse("1/12/15"), DateTime.Parse("1/12/15"));
            Console.WriteLine(fData.Count);

            Console.WriteLine("\nAggregate Data Test");
            AggregateData(AppType.All, DateTime.Parse("1/12/15"), DateTime.Parse("1/12/15"), a);
            Console.WriteLine(fData.Count);
            Testing = testingTemp;
        }
    }
}
