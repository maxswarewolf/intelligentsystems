using Gtk;

using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Axes;
using OxyPlot.Series;

using GeneticSharp.Domain.Chromosomes;
using System;

namespace EnergyPrediction.UI
{
    // Will display the results of the solutions to the user as a visual graph
    class ResultsWindow : Window
    {
        private PlotModel resultsModel;
        private PlotView plotView;
        private LinearAxis xAxis;

        private string xAxisTitle = "Hours";
        private int predictionSteps = 1;

        public ResultsWindow() : base("Results Window")
        {
            SetDefaultSize(1200, 800);

            resultsModel = new PlotModel
            {
                Title = "Results",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White,
                Padding = new OxyThickness(50, 50, 50, 50),
                PlotAreaBorderThickness = new OxyThickness(1, 0, 0, 1)
            };
            resultsModel.Axes.Add(new LinearAxis
            {
                Title = "Predicted Power Usage",
                TitleFontSize = 10,
                TitleColor = OxyColors.Black,
                Position = AxisPosition.Left,
                MajorStep = 1.0,
                Minimum = 0.0,
                MinorTickSize = 0
            });

            xAxis = new LinearAxis
            {
                Title = xAxisTitle,
                TitleFontSize = 10,
                TitleColor = OxyColors.Black,
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
                resultsModel.Series.Add(new FunctionSeries(asAlgo.getCalculatedY, 0, predictionSteps, 0.1, "Results"));
                return true;
            }

            GeneticProgChromosome asProg = aBestChromosome as GeneticProgChromosome;
            if (asProg != null)
            {
                resultsModel.Series.Add(new FunctionSeries(asProg.getCalculatedY, 0, predictionSteps, 0.1, "Results"));
                return true;
            }
            
            return false;
        }

        public bool UpdateAxisTitle(string aAxisTitle)
        {
            xAxis.Title = aAxisTitle;
            return true;
        }

        public bool UpdatePredictionSteps(string aSteps)
        {
            System.Console.Write(aSteps);
            predictionSteps = int.Parse(aSteps);
            System.Console.Write(predictionSteps);
            return true;
        }
    }
}