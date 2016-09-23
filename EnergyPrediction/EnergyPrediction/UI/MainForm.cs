﻿//
// MIT LICENSE
//
// Window.cs
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
#if WIN64
using Eto.Forms;
#else
    using Eto.GtkSharp;
#endif
using Eto.Drawing;
using EnergyPrediction.UI;

public class MainForm : Form
{
    int MainFormWidth = 1000;
    int MainFormHeight = 510;
    public MainForm()
    {
        Title = "Intelligent Systems Assignment - Energy Prediction";
        ClientSize = new Size(MainFormWidth, MainFormHeight);
        Content =
            // Will hold two controls, one to the left, one to the right
            new Splitter
            {
                // Contained elements will be held horizontally adjacent
                Orientation = Orientation.Horizontal,
                // The left control (A custom panel, will contain the graph output)
                Panel1 = new GraphImage(),
                // The right control (A custom a stack layout, will contain all 
                // of the options for starting a new genetic algo/prog)
                Panel2 = new GeneticOptions(),
                // The initial position for the split
                Position = (int)(MainFormWidth - 210)
            };
    }
}