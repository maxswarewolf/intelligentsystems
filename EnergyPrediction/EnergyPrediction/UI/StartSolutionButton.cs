using System;
#if WIN64
using Eto.Forms;
#else
    using Eto.GtkSharp;
#endif

namespace EnergyPrediction.UI
{
    class StartSolutionButton : Button
    {
        GeneticOptions fOwningOptions;
        public StartSolutionButton(GeneticOptions aOwningOptions)
        {
            fOwningOptions = aOwningOptions;
            Text = "Start Solution";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            fOwningOptions.StartSolution();
        }
    }
}
