using System;
using Gtk;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EnergyPrediction.UI
{
    class ResultsWindow
    {
        private PlotModel geneticGraphingModel;
        private Window resultsWindow;

        public ResultsWindow()
        {
            geneticGraphingModel = new PlotModel 
            { 
                Title = "Results",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };
            geneticGraphingModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineThickness = 3,
                MinorGridlineThickness = 3,
                MajorGridlineColor = OxyColors.DarkSlateGray,
                MinorGridlineColor = OxyColors.LightGray,
                MinorStep = 0.5,
                MajorStep = 1.0
            });
            geneticGraphingModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineThickness = 3,
                MinorGridlineThickness = 3,
                MajorGridlineColor = OxyColors.DarkSlateGray,
                MinorGridlineColor = OxyColors.LightGray,
                MinorStep = 0.5,
                MajorStep = 1.0
            });
            geneticGraphingModel.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
        }
    }
}