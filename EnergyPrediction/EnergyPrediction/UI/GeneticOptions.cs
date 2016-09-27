using Eto.Forms;

using System.Threading;
using System.Collections.Generic;

using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Reinsertions;
using System.Security.Permissions;

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
        ComboBox fFitnessComboBox;

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

        // This thread class will run the solutions once they have been configured. This allows solutions to be cancelled, and does not freeze the UI
        Thread solutionRunner = new Thread(StartSolution);

        public GeneticOptions()
        {
            // Initialise all of the combo boxes
            fSolutionComboBox = new ComboBox
            {
                DataStore = fSolutionOptions
            };

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
            fFitnessComboBox = new ComboBox
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
            fStartSolutionButton.MouseDown += new System.EventHandler<Eto.Forms.MouseEventArgs>(CreateSolution);
            fStartSolutionButton.Text = "Start Solution";

            // Make sure that when the desired solution type is changed, the UI can trigger an update
            fSolutionComboBox.SelectedValueChanged += new System.EventHandler<System.EventArgs>(SolutionValueChanged);

            // AFTER the combo boxes load, select an option to present the UI correctly
            this.LoadComplete += new System.EventHandler<System.EventArgs>(SelectOnLoadComplete);

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
            Items.Add(fFitnessComboBox);

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
            
        }

        private void SelectOnLoadComplete(object sender, System.EventArgs e)
        {
            fSolutionComboBox.SelectedIndex = 0;
        }

        // Whenever the solution combo box value is changed, this method will be called
        // This will show/hide the appropriate combo boxes for the currently selected solution method
        private void SolutionValueChanged(object sender, System.EventArgs e)
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    fCrossoversAlgoComboBox.Visible = true;
                    fMutationAlgoComboBox.Visible = true;
                    fCrossoversProgComboBox.Visible = false;
                    fMutationProgComboBox.Visible = false;
                    break;
                case "Genetic Programming":
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
        private void CreateSolution(object sender, System.EventArgs e)
        {

            //DataIO.LoadMin(StateType.VIC, DateTime.Parse("1/9/16"), DateTime.Parse("1/9/16"));
            if (solutionRunner.IsAlive)
            {
                StopSolution();
            }
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    var GeneticAlgorithm = new GeneticAlgoController(new GeneticAlgoChromosome(1000, 4),
                                                     GetSelectedCrossoverMethod(),
                                                     GetSelectedFitnessFuntion(),
                                                     GetSelectedMutationMethod(),
                                                     GetSelectedSelectionMethod(),
                                                     GetSelectedTerminationMethod(),
                                                     GetSelectedReinsertionMethod(), 
                                                     200);
                    GeneticAlgorithm.CrossoverProbability = 0.6f;
                    GeneticAlgorithm.MutationProbability = 0.6f;
                    GeneticAlgorithm.addEventFunction(GeneticAlgorithm.DefaultDraw);
                    GeneticAlgorithm.Start();
                    solutionRunner = new Thread(StartSolution);
                    solutionRunner.Start(GeneticAlgorithm);
                    break;
                case "Genetic Programming":
                    // The genetic programming equivalent of the previous block will go here
                    break;
                default:
                    break;
            }
        }

        // The following methods concern threading, and starting a solution in a new thread. If I don't do this, the UI freezes
        // if you try to interact with it, and the solution is still running. As of writing this, the threading I have implemented doesn't
        // stop the freezing either. There will need to be changes made to the base controller class, so that it accepts a bool, which will
        // indicate whether it should stop or not (Say, upon each generation, if the thread has been signalled to stop, clean up and stop)

        // The method that shall be passed to a new thread. Accepts a genetic controller as a parameter, and runs that controller
        private static void StartSolution(object aControllerToRun)
        {
            // Try to cast to an algo controller
            var lAlgoController = aControllerToRun as GeneticAlgoController;

            if (lAlgoController != null)
            {
                lAlgoController.Start();
            }

            // If that failed, it must be a prog controller
            var lProgController = aControllerToRun as GeneticProgController;

            if (lProgController != null)
            {
                lProgController.Start();
            }
            // If THAT failed, something has gone horribly wrong
            else throw new System.InvalidCastException("New controller could not be cast to either algo or prog - GeneticOptions, StartSolution");
        }

        private void StopSolution()
        {
            // In here will exist a reference to a class scoped boolean that will tell a thread to stop executing
        }

        // Depending on the selected solution, return the selected crossover
        private CrossoverBase GetSelectedCrossoverMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    switch (fCrossoversAlgoComboBox.SelectedValue.ToString())
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
                    switch (fCrossoversProgComboBox.SelectedValue.ToString())
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
                    switch (fMutationAlgoComboBox.SelectedValue.ToString())
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
                    switch (fMutationProgComboBox.SelectedValue.ToString())
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
                    return new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(System.TimeSpan.FromSeconds(90)));
                case "Genetic Programming":
                    return new OrTermination(new FitnessThresholdTermination(0), new TimeEvolvingTermination(System.TimeSpan.FromSeconds(90)));
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
