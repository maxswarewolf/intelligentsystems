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

            plotView = new PlotView { Model = resultsModel, Visible = true };

            Add(plotView);
            Focus = plotView;

            ShowAll();
        }

        public bool UpdateResults(IChromosome aBestChromosome)
        {
            GeneticAlgoChromosome asAlgo = aBestChromosome as GeneticAlgoChromosome;
            if (asAlgo != null)
            {
                resultsModel.InvalidatePlot(true);
                resultsModel.Series.Add(new FunctionSeries(asAlgo.getCalculatedY, -10, 10, 0.1, "Results"));
                return true;
            }

            GeneticProgChromosome asProg = aBestChromosome as GeneticProgChromosome;
            if (asProg != null)
            {
                resultsModel.InvalidatePlot(true);
                resultsModel.Series.Add(new FunctionSeries(asProg.getCalculatedY, -10, 10, 0.1, "Results"));
                return true;
            }
            
            return false;
        }
    }
}