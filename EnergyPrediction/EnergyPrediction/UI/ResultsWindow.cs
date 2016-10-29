using System;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EnergyPrediction.UI
{
    class ResultsWindow
    {
        private PlotModel resultsModel;

        public ResultsWindow()
        {
            resultsModel = new PlotModel
            {
                Title = "Results",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };
            resultsModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineThickness = 3,
                MinorGridlineThickness = 3,
                MajorGridlineColor = OxyColors.DarkSlateGray,
                MinorGridlineColor = OxyColors.LightGray,
                MinorStep = 0.5,
                MajorStep = 1.0
            });
            resultsModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineThickness = 3,
                MinorGridlineThickness = 3,
                MajorGridlineColor = OxyColors.DarkSlateGray,
                MinorGridlineColor = OxyColors.LightGray,
                MinorStep = 0.5,
                MajorStep = 1.0
            });
            resultsModel.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
        }
    }
}