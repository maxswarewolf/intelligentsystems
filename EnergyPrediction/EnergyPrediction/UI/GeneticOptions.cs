#if WIN64
using Eto.Forms;
#else
using Eto.GtkSharp;
#endif

using System.Collections.Generic;

using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Reinsertions;

namespace EnergyPrediction.UI
{
    /// <summary>
    /// Genetic Options is a Stack Layout derived from Eto.Forms that contains all of the elements
    /// used to define what solution shall be run, and with what parameters. This is done in the form 
    /// of combo boxes, each of which are labeled. Each of the critical combo boxes will also affect 
    /// the UI through events and event handlers e.g The very first combo box contains options as to 
    /// which solution shall be used, either a Genetic Algorithm, or Genetic Programming. This will 
    /// trigger an event to hide/show other elements that are specific to the selected option. 
    /// The final element is a button that triggers the Start Solution method using events.
    /// </summary>
    class GeneticOptions : StackLayout
    {
        // Each of the following is a list of options that may be selected, followed by the combo box that will contain it

        List<string> fSolutionOptions = new List<string>() { "Genetic Algorithm", "Genetic Programming" };
        ComboBox fSolutionComboBox;

        List<string> fCrossoversAlgo = new List<string>() { "Uniform", "One Point", "Two Point", "Three Parent" };
        ComboBox fCrossoversAlgoComboBox;

        List<string> fCrossoversProg = new List<string>() { "PLACE HOLDER" };
        ComboBox fCrossoversProgComboBox;

        List<string> fFitness = new List<string>() { "Error Sum" };
        ComboBox fFitnessgComboBox;

        List<string> fMutationAlgo = new List<string>() { "Uniform", "Twors", "Reverse Sequence" };
        ComboBox fMutationAlgoComboBox;

        List<string> fMutationProg = new List<string>() { "Uniform", "Branch", "Reverse Sequence" };
        ComboBox fMutationProgComboBox;

        List<string> fSelection = new List<string>() { "Elite", "Stochastic", "Inverse Elite" };
        ComboBox fSelectionComboBox;

        List<string> fReinsertion = new List<string>() { "PLACE HOLDER" };
        ComboBox fReinsertionComboBox;

        List<string> fTimeFrames = new List<string>() { "Day", "Week", "Month", "Year" };
        ComboBox ffTimeFramesComboBox;

        List<string> fStates = new List<string>() { "VIC", "NSW", "QLD", "ACT", "NT", "SA", "WA", "TAS" };
        ComboBox fStatesComboBox;

        // This button will use an event to trigger the the start of a solution
        Button fStartSolutionButton;

        public GeneticOptions()
        {
            // Initialise all of the combo boxes
            fSolutionComboBox = new ComboBox
            {
                DataStore = fSolutionOptions
            };
            // Make sure that when the desired solution type is changed, the UI can trigger an update
            fSolutionComboBox.SelectedValueChanged += new System.EventHandler<System.EventArgs>(SolutionValueChanged);

            fCrossoversAlgoComboBox = new ComboBox
            {
                DataStore = fCrossoversAlgo,
                SelectedIndex = 0
            };
            fCrossoversProgComboBox = new ComboBox
            {
                DataStore = fCrossoversProg,
                SelectedIndex = 0
            };
            fFitnessgComboBox = new ComboBox
            {
                DataStore = fFitness,
                SelectedIndex = 0
            };
            fMutationAlgoComboBox = new ComboBox
            {
                DataStore = fMutationAlgo,
                SelectedIndex = 0
            };
            fMutationProgComboBox = new ComboBox
            {
                DataStore = fMutationProg,
                SelectedIndex = 0
            };
            fSelectionComboBox = new ComboBox
            {
                DataStore = fSelection,
                SelectedIndex = 0
            };
            fReinsertionComboBox = new ComboBox
            {
                DataStore = fReinsertion,
                SelectedIndex = 0
            };
            ffTimeFramesComboBox = new ComboBox
            {
                DataStore = fTimeFrames,
                SelectedIndex = 0
            };
            fStatesComboBox = new ComboBox
            {
                DataStore = fStates,
                SelectedIndex = 0
            };

            // Initialise the button
            fStartSolutionButton = new Button();
            fStartSolutionButton.MouseDown += new System.EventHandler<Eto.Forms.MouseEventArgs>(StartSolution);
            fStartSolutionButton.Text = "Start Solution";

            // The direction the list shall face (Top to bottom)
            Orientation = Orientation.Vertical;
            // How much space will exist between elements
            Spacing = 5;

            // Initilise the internal list of item for the stack layout
            InitItems();
        }

        private void InitItems()
        {
            // Add a label for each option, followed by the accompanying combo box / element
            Items.Add(new Label
            {
                Text = "Solution"
            });
            Items.Add(fSolutionComboBox);

            Items.Add(new Label
            {
                Text = "Crossover"
            });
            Items.Add(fCrossoversAlgoComboBox);
            Items.Add(fCrossoversProgComboBox);

            Items.Add(new Label
            {
                Text = "Fitness"
            });
            Items.Add(fFitnessgComboBox);

            Items.Add(new Label
            {
                Text = "Mutation"
            });
            Items.Add(fMutationAlgoComboBox);
            Items.Add(fMutationProgComboBox);

            Items.Add(new Label
            {
                Text = "Selection"
            });
            Items.Add(fSelectionComboBox);

            Items.Add(new Label
            {
                Text = "Reinsertion"
            });
            Items.Add(fReinsertionComboBox);

            Items.Add(new Label
            {
                Text = "Prediction Start Date"
            });
            Items.Add(new DateTimePicker());

            Items.Add(new Label
            {
                Text = "Prediction Time Frame"
            });
            Items.Add(ffTimeFramesComboBox);

            Items.Add(new Label
            {
                Text = "State to Predict From"
            });
            Items.Add(fStatesComboBox);
            Items.Add(fStartSolutionButton);

            // Select something manually in the fSolutionComboBox first, to trigger the hide/show method SolutionValueChanged
            fSolutionComboBox.SelectedIndex = 0;
        }

        // Whenever the solution combo box value is changed, this method will be called
        // This will show/hide the appropriate combo boxes for the currently selected solution method
        private void SolutionValueChanged(object sender, System.EventArgs e)
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    System.Console.WriteLine("GeneticAlgorithm");
                    fCrossoversAlgoComboBox.Visible = true;
                    fMutationAlgoComboBox.Visible = true;
                    fCrossoversProgComboBox.Visible = false;
                    fMutationProgComboBox.Visible = false;
                    break;
                case "Genetic Programming":
                    System.Console.WriteLine("GeneticProgramming");
                    fCrossoversAlgoComboBox.Visible = false;
                    fMutationAlgoComboBox.Visible = false;
                    fCrossoversProgComboBox.Visible = true;
                    fMutationProgComboBox.Visible = true;
                    break;
                default:
                    break;
            }
        }

        // Triggered once the StartSolutionBUtton is clicked
        private void StartSolution(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("Solution Started");

            //DataIO.LoadMin(StateType.VIC, DateTime.Parse("1/9/16"), DateTime.Parse("1/9/16"));

            var AlgoTest = new GeneticAlgoController(new GeneticAlgoChromosome(1000, 4),
                                                     new OnePointCrossover(2),
                                                     new ErrorSquaredFitness(),
                                                     new TworsMutation(),
                                                     new EliteSelection(),
                                                     new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(System.TimeSpan.FromSeconds(90))),
                                                     new ElitistReinsertion(), 200);
            AlgoTest.CrossoverProbability = 0.6f;
            AlgoTest.MutationProbability = 0.6f;
            AlgoTest.addEventFunction(AlgoTest.DefaultDraw);
            AlgoTest.Start();
        }
    }
}
