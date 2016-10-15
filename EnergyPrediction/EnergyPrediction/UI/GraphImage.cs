using Eto.Forms;
using Eto.Drawing;
using Eto.OxyPlot;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EnergyPrediction.UI
{
    class GraphImage : Panel
    {
        private PlotModel geneticGraphingModel;
        public GraphImage()
        {
            geneticGraphingModel = new PlotModel { Title = "Results" };
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
            var areaSeries = new AreaSeries();
            geneticGraphingModel.Series.Add(areaSeries);
            BackgroundColor = Colors.White;
        }
    }
}