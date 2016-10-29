using Eto.Forms;
using System;

using System.Collections.Generic;

using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain;

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
    class ParameterStack : StackLayout
    {
        #region Class Variables
        // Each of the following is a list of options that may be selected, followed by the combo box that will contain it

        private List<string> fSolutionOptions = new List<string>() { "Genetic Algorithm", "Genetic Programming" };
        private ComboBox fSolutionComboBox;

        private List<string> fCrossoversAlgo = new List<string>() { "Uniform", "One Point", "Two Point", "Three Parent" };
        private List<string> fCrossoversProg = new List<string>() { "PLACE HOLDER" };
        private ComboBox fCrossoverComboBox;

        private List<string> fFitness = new List<string>() { "Error Sum" };
        private ComboBox fFitnessComboBox;

        private List<string> fMutationAlgo = new List<string>() { "Uniform", "Twors", "Reverse Sequence" };
        private List<string> fMutationProg = new List<string>() { "Uniform", "Branch", "Reverse Sequence" };
        private ComboBox fMutationComboBox;

        private List<string> fSelection = new List<string>() { "Elite", "Stochastic", "Inverse Elite" };
        private ComboBox fSelectionComboBox;

        private List<string> fReinsertion = new List<string>() { "PLACE HOLDER" };
        private ComboBox fReinsertionComboBox;

        private List<string> fTimeFrames = new List<string>() { "Day", "Week", "Month", "Year" };
        private ComboBox fTimeFramesComboBox;

        private List<string> fStates = new List<string>() { "VIC", "NSW", "QLD", "ACT", "NT", "SA", "WA", "TAS" };
        private ComboBox fStatesComboBox;

        // Will use an events to trigger
        private Button fStart;
        private Button fStop;
        private Button fResume;
        private StackLayout fButtonStack;

        // Contains the currently running solution
        private ControllerBase fSolution = null;

        #endregion

        public ParameterStack()
        {
            // Initialise all of the combo boxes
            fSolutionComboBox = new ComboBox
            {
                DataStore = fSolutionOptions,
                SelectedIndex = 0
            };

            // Make sure that when the desired solution type is changed, the UI can trigger an update
            fSolutionComboBox.SelectedValueChanged += new EventHandler<EventArgs>(SolutionValueChanged);

            fCrossoverComboBox = new ComboBox
            {
                DataStore = fCrossoversAlgo,
                SelectedIndex = 0
            };
            fFitnessComboBox = new ComboBox
            {
                DataStore = fFitness,
                SelectedIndex = 0
            };
            fMutationComboBox = new ComboBox
            {
                DataStore = fMutationAlgo,
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
            fTimeFramesComboBox = new ComboBox
            {
                DataStore = fTimeFrames,
                SelectedIndex = 0
            };
            fStatesComboBox = new ComboBox
            {
                DataStore = fStates,
                SelectedIndex = 0
            };

            // Initialise the buttons
            fStart = new Button();
            fStart.MouseDown += new EventHandler<MouseEventArgs>(StartSolution);
            fStart.Text = "Start Solution";

            fStop = new Button();
            fStop.MouseDown += new EventHandler<MouseEventArgs>(StopSolution);
            fStop.Text = "Stop Solution";

            fResume = new Button();
            fResume.MouseDown += new EventHandler<MouseEventArgs>(ResumeSolution);
            fResume.Text = "Resume Solution";

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
            Items.Add(fCrossoverComboBox);

            Items.Add(new Label
            {
                Text = "Fitness"
            });
            Items.Add(fFitnessComboBox);

            Items.Add(new Label
            {
                Text = "Mutation"
            });
            Items.Add(fMutationComboBox);

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
            Items.Add(fTimeFramesComboBox);

            Items.Add(new Label
            {
                Text = "Start/Stop/Resume the prediction"
            });

            fButtonStack = new StackLayout();
            fButtonStack.Items.Add(fStart);
            fButtonStack.Items.Add(fStop);
            fButtonStack.Items.Add(fResume);
            fButtonStack.Orientation = Orientation.Horizontal;

            Items.Add(fButtonStack);
        }

        // Whenever the solution combo box value is changed, this method will be called
        // This will show/hide the appropriate combo boxes for the currently selected solution method
        private void SolutionValueChanged(object sender, EventArgs e)
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    fCrossoverComboBox.DataStore = fCrossoversAlgo;
                    fCrossoverComboBox.SelectedIndex = 0;

                    fMutationComboBox.DataStore = fMutationAlgo;
                    fMutationComboBox.SelectedIndex = 0;
                    break;
                case "Genetic Programming":
                    fCrossoverComboBox.DataStore = fCrossoversProg;
                    fCrossoverComboBox.SelectedIndex = 0;

                    fMutationComboBox.DataStore = fMutationProg;
                    fMutationComboBox.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        // Triggered once the StartSolutionBUtton is clicked
        private void StartSolution(object sender, EventArgs e)
        {
            if (fSolution != null)
                if (fSolution.State == GeneticAlgorithmState.Started || fSolution.State == GeneticAlgorithmState.Resumed) fSolution.Stop();

            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    fSolution = new GeneticAlgoController(new GeneticAlgoChromosome(1000, 4),
                                                     GetSelectedCrossoverMethod(),
                                                     GetSelectedFitnessFuntion(),
                                                     GetSelectedMutationMethod(),
                                                     GetSelectedSelectionMethod(),
                                                     GetSelectedTerminationMethod(),
                                                     GetSelectedReinsertionMethod(), 
                                                     200);
                    fSolution.CrossoverProbability = 0.6f;
                    fSolution.MutationProbability = 0.6f;
                    fSolution.addbestChromEvent(fSolution.DefaultDraw);
                    fSolution.Start();
                    break;
                case "Genetic Programming":
                    // The genetic programming equivalent of the previous block will go here
                    break;
                default:
                    break;
            }
        }

        private void StopSolution(object sender, EventArgs e) { if (fSolution != null) fSolution.Stop(); }

        private void ResumeSolution(object sender, EventArgs e) { if (fSolution != null) fSolution.Resume(); }

        // Will be called at the generation ran event of the solution. Will update the results window with the best chromosome every N generations
        bool updateResults(Generation aCurrentGeneration)
        {
            if (aCurrentGeneration.Number % 10 == 0)
            {
                //resultsWindow.update(aCurrentGeneration.BestChromosome);
                return true;
            }
            return false;
        }

        // Depending on the selected solution, return the selected crossover
        private CrossoverBase GetSelectedCrossoverMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    switch (fCrossoverComboBox.SelectedValue.ToString())
                    {
                        case "Uniform":
                            return new UniformCrossover();
                        case "One Point":
                            return new OnePointCrossover();
                        case "Two Point":
                            return new TwoPointCrossover();
                        case "Three Point":
                            return new ThreeParentCrossover();
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fCrossoverComboBox.SelectedValue.ToString())
                    { // There are no programming crossovers
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected crossover
        private IFitness GetSelectedFitnessFuntion()
        {
            switch (fFitnessComboBox.SelectedValue.ToString())
            {
                case "Error Sum":
                    return new FitnessFunctions();
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected crossover
        private MutationBase GetSelectedMutationMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    switch (fMutationComboBox.SelectedValue.ToString())
                    {
                        case "Uniform":
                            return new UniformMutation();
                        case "Twors":
                            return new TworsMutation();
                        case "Reverse Sequence":
                            return new ReverseSequenceMutation();
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fMutationComboBox.SelectedValue.ToString())
                    { // These cases are commented as there are no programming specific mutations as of yet
                        case "Uniform":
                            //return new UniformMutation();
                        case "Branch":
                            //return new TworsMutation();
                        case "Reverse Sequence":
                            //return new ReverseSequenceMutation();
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected crossover
        private SelectionBase GetSelectedSelectionMethod()
        {
            switch (fSelectionComboBox.SelectedValue.ToString())
            {
                case "Elite":
                    return new EliteSelection();
                case "Stochastic":
                    return new StochasticSelection();
                case "Inverse Elite": // Doesn't exist?
                    //return new Inverse();
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected crossover
        private OrTermination GetSelectedTerminationMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    return new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(TimeSpan.FromSeconds(90)));
                case "Genetic Programming":
                    return new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(TimeSpan.FromSeconds(90)));
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected crossover
        private ReinsertionBase GetSelectedReinsertionMethod()
        {
            switch (fReinsertionComboBox.SelectedValue.ToString())
            {
                default:
                    return new ElitistReinsertion();
            }
        }
    }
}
