//
// MIT LICENSE
//
// CsvFileReader.cs
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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EnergyPrediction
{
    public class CsvFileReader : CsvFileCommon, IDisposable
    {
        private StreamReader fReader;
        private string fCurrLine;
        private int fCurrPos;
        private EmptyLineBehavior fEmptyLineBehavior;

        public CsvFileReader(Stream stream,
                             EmptyLineBehavior aEmptyLineBehavior = EmptyLineBehavior.NoColumns)
        {
            fReader = new StreamReader(stream);
            fEmptyLineBehavior = aEmptyLineBehavior;
        }

        public CsvFileReader(string path,
                             EmptyLineBehavior aEmptyLineBehavior = EmptyLineBehavior.NoColumns)
        {
            fReader = new StreamReader(path);
            fEmptyLineBehavior = aEmptyLineBehavior;
        }

        public bool ReadRow(List<string> aColumns)
        {
            if (aColumns == null)
                throw new ArgumentNullException("columns");

            ReadNextLine:
            fCurrLine = fReader.ReadLine();
            fCurrPos = 0;
            if (fCurrLine == null)
                return false;
            if (fCurrLine.Length == 0)
            {
                switch (fEmptyLineBehavior)
                {
                    case EmptyLineBehavior.NoColumns:
                        aColumns.Clear();
                        return true;
                    case EmptyLineBehavior.Ignore:
                        goto ReadNextLine;
                    case EmptyLineBehavior.EndOfFile:
                        return false;
                }
            }

            string lColumn;
            int lNumColumns = 0;
            while (true)
            {
                if (fCurrPos < fCurrLine.Length && fCurrLine[fCurrPos] == Quote)
                    lColumn = ReadQuotedColumn();
                else
                    lColumn = ReadUnquotedColumn();
                if (lNumColumns < aColumns.Count)
                    aColumns[lNumColumns] = lColumn;
                else
                    aColumns.Add(lColumn);
                lNumColumns++;
                if (fCurrLine == null || fCurrPos == fCurrLine.Length)
                    break;
                Debug.Assert(fCurrLine[fCurrPos] == Delimiter);
                fCurrPos++;
            }
            if (lNumColumns < aColumns.Count)
                aColumns.RemoveRange(lNumColumns, aColumns.Count - lNumColumns);
            return true;
        }

        private string ReadQuotedColumn()
        {
            Debug.Assert(fCurrPos < fCurrLine.Length && fCurrLine[fCurrPos] == Quote);
            fCurrPos++;

            StringBuilder lBuilder = new StringBuilder();
            while (true)
            {
                while (fCurrPos == fCurrLine.Length)
                {
                    fCurrLine = fReader.ReadLine();
                    fCurrPos = 0;
                    if (fCurrLine == null)
                        return lBuilder.ToString();
                    lBuilder.Append(Environment.NewLine);
                }
                if (fCurrLine[fCurrPos] == Quote)
                {
                    int nextPos = (fCurrPos + 1);
                    if (nextPos < fCurrLine.Length && fCurrLine[nextPos] == Quote)
                        fCurrPos++;
                    else
                        break;
                }

                lBuilder.Append(fCurrLine[fCurrPos++]);
            }

            if (fCurrPos < fCurrLine.Length)
            {

                Debug.Assert(fCurrLine[fCurrPos] == Quote);
                fCurrPos++;

                lBuilder.Append(ReadUnquotedColumn());
            }

            return lBuilder.ToString();
        }


        private string ReadUnquotedColumn()
        {
            int lStartPos = fCurrPos;
            fCurrPos = fCurrLine.IndexOf(Delimiter, fCurrPos);
            if (fCurrPos == -1)
                fCurrPos = fCurrLine.Length;
            if (fCurrPos > lStartPos)
                return fCurrLine.Substring(lStartPos, fCurrPos - lStartPos);
            return String.Empty;
        }
        public void Dispose()
        {
            fReader.Dispose();
        }
    }
}

