using System;

using Gtk;

using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EnergyPrediction.UI
{
    class ResultsWindow : Window
    {
        private PlotModel resultsModel;
        private PlotView plotView;

        public ResultsWindow() : base("Results Window")
        {
            SetDefaultSize(1200, 800);

            resultsModel = new PlotModel
            {
                Title = "Results",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };
            resultsModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineThickness = 1,
                MinorGridlineThickness = 1,
                MajorGridlineColor = OxyColors.DarkSlateGray,
                MinorGridlineColor = OxyColors.LightGray,
                MajorStep = 1.0,
                Minimum = 0.0
            });
            resultsModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineThickness = 1,
                MinorGridlineThickness = 1,
                MajorGridlineColor = OxyColors.DarkSlateGray,
                MinorGridlineColor = OxyColors.LightGray,
                MajorStep = 1.0,
                FontSize = 10
            });
            resultsModel.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));

            plotView = new PlotView { Model = resultsModel, Visible = true };

            Add(plotView);
            Focus = plotView;

            ShowAll();
        }
    }
}