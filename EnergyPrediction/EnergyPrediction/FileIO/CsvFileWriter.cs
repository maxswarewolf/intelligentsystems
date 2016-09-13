//
// MIT LICENSE
//
// CsvFileWriter.cs
//
// Author:
//       Adrian Pellegrino <adrianpellegrino.licensing@gmail.com>
//
// Copyright (c) 2016 Adrian Pellegrino
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

namespace EnergyPrediction
{
    public class CsvFileWriter : CsvFileCommon, IDisposable
    {
        private StreamWriter fWriter;
        private string fOneQuote = null;
        private string fTwoQuotes = null;
        private string fQuotedFormat = null;

        public CsvFileWriter(Stream aStream)
        {
            fWriter = new StreamWriter(aStream);
        }

        public CsvFileWriter(string aPath)
        {
            fWriter = new StreamWriter(aPath);
        }

        public void WriteRow(List<string> aColumns)
        {
            if (aColumns == null)
                throw new ArgumentNullException("columns");

            if (fOneQuote == null || fOneQuote[0] != Quote)
            {
                fOneQuote = String.Format("{0}", Quote);
                fTwoQuotes = String.Format("{0}{0}", Quote);
                fQuotedFormat = String.Format("{0}{{0}}{0}", Quote);
            }

            for (int i = 0; i < aColumns.Count; i++)
            {
                if (i > 0)
                    fWriter.Write(Delimiter);
                if (aColumns[i].IndexOfAny(SpecialChars) == -1)
                    fWriter.Write(aColumns[i]);
                else
                    fWriter.Write(fQuotedFormat, aColumns[i].Replace(fOneQuote, fTwoQuotes));
            }
            fWriter.WriteLine();
        }

        public void Dispose()
        {
            fWriter.Dispose();
        }
    }
}

