#if WIN64
using Eto.Forms;
#else
    using Eto.GtkSharp;
#endif
using Eto.Drawing;

namespace EnergyPrediction.UI
{
    class GraphImage : Panel
    {
        public GraphImage()
        {
            BackgroundColor = Colors.White;
        }
    }
}