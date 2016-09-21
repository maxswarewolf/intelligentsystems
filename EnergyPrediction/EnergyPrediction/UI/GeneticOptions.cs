#if WIN64
using Eto.Forms;
#else
    using Eto.GtkSharp;
#endif
using System.Collections.Generic;

namespace EnergyPrediction.UI
{
    class GeneticOptions : StackLayout
    {
        List<string> SolutionOptions = new List<string>() { "Genetic Algorithm", "Genetic Programming" };

        List<string> CrossoversAlgo = new List<string>() { "Uniform", "One Point", "Two Point", "Three Parent" };
        List<string> CrossoversProg = new List<string>() { "PLACE HOLDER" };

        List<string> Fitness = new List<string>() { "Error Sum" };

        List<string> MutationAlgo = new List<string>() { "Uniform", "Twors", "Reverse Sequence" };

        List<string> MutationProg = new List<string>() { "Uniform", "Branch", "Reverse Sequence" };

        List<string> Selection = new List<string>() { "Elite", "Stochastic", "Inverse Elite" };

        List<string> Reinsertion = new List<string>() { "PLACE HOLDER" };

        public GeneticOptions()
        {
            Orientation = Orientation.Vertical;
            Spacing = 5;
            Items.Add(new Label
            {
                Text = "Solution"
            });
            Items.Add(new ComboBox {
                DataStore = SolutionOptions,
                SelectedIndex = 0
            });

            Items.Add(new Label
            {
                Text = "Crossover"
            });
            Items.Add(new ComboBox {
                DataStore = CrossoversAlgo,
                SelectedIndex = 0
            });
            Items.Add(new ComboBox {
                DataStore = CrossoversProg,
                SelectedIndex = 0
            });

            Items.Add(new Label
            {
                Text = "Fitness"
            });
            Items.Add(new ComboBox {
                DataStore = Fitness,
                SelectedIndex = 0
            });

            Items.Add(new Label
            {
                Text = "Mutation"
            });
            Items.Add(new ComboBox {
                DataStore = MutationAlgo,
                SelectedIndex = 0
            });

            Items.Add(new Label
            {
                Text = "Mutation"
            });
            Items.Add(new ComboBox {
                DataStore = MutationProg,
                SelectedIndex = 0
            });

            Items.Add(new Label
            {
                Text = "Selection"
            });
            Items.Add(new ComboBox {
                DataStore = Selection,
                SelectedIndex = 0
            });

            Items.Add(new Label
            {
                Text = "Reinsertion"
            });
            Items.Add(new ComboBox {
                DataStore = Reinsertion,
                SelectedIndex = 0
            });
            Items.Add(new StartSolutionButton(this));
        }

        public void StartSolution()
        {
            System.Console.WriteLine("Solution Started");
        }
    }
}
