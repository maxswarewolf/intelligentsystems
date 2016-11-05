using System;

using Gtk;

using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Axes;
using OxyPlot.Series;

using GeneticSharp.Domain.Chromosomes;

namespace EnergyPrediction.UI
{
    // Will display the results of the solutions to the user as a visual graph
    class ResultsWindow : Window
    {
        private PlotModel resultsModel;
        private PlotView plotView;
        private string xAxisTitle;
        private LinearAxis xAxis;

        public ResultsWindow() : base("Results Window")
        {
            SetDefaultSize(1200, 800);
            xAxisTitle = "Hours";

            resultsModel = new PlotModel
            {
                Title = "Results",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };
            resultsModel.Axes.Add(new LinearAxis
            {
                Title = "Predicted Power Usage",
                Position = AxisPosition.Left,
                MajorStep = 1.0,
                Minimum = 0.0,
                MinorTickSize = 0
            });

            xAxis = new LinearAxis
            {
                Title = xAxisTitle,
                Position = AxisPosition.Bottom,
                MajorStep = 1.0,
                FontSize = 10,
                MinorTickSize = 0
            };

            resultsModel.Axes.Add(xAxis);

            plotView = new PlotView { Model = resultsModel, Visible = true };

            Add(plotView);
            Focus = plotView;

            ShowAll();
        }

        public bool UpdateResults(IChromosome aBestChromosome)
        {
            plotView.InvalidatePlot(true);
            resultsModel.InvalidatePlot(true);
            resultsModel.Series.Clear();

            GeneticAlgoChromosome asAlgo = aBestChromosome as GeneticAlgoChromosome;
            if (asAlgo != null)
            {
                resultsModel.Series.Add(new FunctionSeries(asAlgo.getCalculatedY, 0, 10, 0.1, "Results"));
                return true;
            }

            GeneticProgChromosome asProg = aBestChromosome as GeneticProgChromosome;
            if (asProg != null)
            {
                resultsModel.Series.Add(new FunctionSeries(asProg.getCalculatedY, 0, 10, 0.1, "Results"));
                return true;
            }
            
            return false;
        }

        public bool SwitchTo(AxisType aAxisType)
        {
            switch (aAxisType)
            {
                case AxisType.Hours:
                    plotView.InvalidatePlot(true);
                    resultsModel.InvalidatePlot(true);
                    resultsModel.Series.Clear();
                    xAxis.Title = "Hours";
                    return true;
                case AxisType.Days:
                    plotView.InvalidatePlot(true);
                    resultsModel.InvalidatePlot(true);
                    resultsModel.Series.Clear();
                    xAxis.Title = "Days";
                    return true;
                case AxisType.Weeks:
                    plotView.InvalidatePlot(true);
                    resultsModel.InvalidatePlot(true);
                    resultsModel.Series.Clear();
                    xAxis.Title = "Weeks";
                    return true;
                default:
                    return false;
            }
        }
    }
}