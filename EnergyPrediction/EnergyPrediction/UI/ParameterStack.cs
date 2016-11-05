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
using GeneticSharp.Domain.Chromosomes;

namespace EnergyPrediction.UI
{
    /// <summary>
    /// ParameterStack is a Stack Layout derived from Eto.Forms that contains all of the elements
    /// used to define what solution shall be run, and with what parameters. This is done in the form 
    /// of combo boxes, each of which are labeled. Each of the critical combo boxes will also affect 
    /// the UI through events and event handlers e.g The very first combo box contains options as to 
    /// which solution shall be used, either a Genetic Algorithm, or Genetic Programming.
    /// </summary>
    class ParameterStack : StackLayout
    {
        #region Class Variables
        // Each of the following is a list of options that may be selected, followed by the combo box that will contain it

        private List<string> fSolutionOptions = new List<string>() { "Genetic Algorithm", "Genetic Programming" };
        private ComboBox fSolutionComboBox;

        private List<string> fCrossoversAlgo = new List<string>() { "One Point" };
        private List<string> fCrossoversProg = new List<string>() { "Branch" };
        private ComboBox fCrossoverComboBox;

        private List<string> fFitnesses = new List<string>() { "Error Squared" };
        private ComboBox fFitnessComboBox;

        private List<string> fMutationsAlgo = new List<string>() { "Uniform", "Twors" };
        private List<string> fMutationsProg = new List<string>() { "Uniform" };
        private ComboBox fMutationComboBox;

        private List<string> fSelections = new List<string>() { "Tournament", "Elite" };
        private ComboBox fSelectionComboBox;

        private List<string> fReinsertions = new List<string>() { "Combined Reinsertion" };
        private ComboBox fReinsertionComboBox;

        // Will change the xAxis title, and the aggregation of the source data for time steps
        private List<string> fPredictionUnits = new List<string>() { "Days", "Weeks", "Months" };
        private ComboBox fPredictionUnitsComboBox;

        // The number of a particular unit of time that will be predicted eg 5 fDaySteps will predict 5 days into the future
        private List<string> fDaySteps = new List<string>() { "1", "2", "3", "4", "5", "6", "7" };
        private List<string> fWeekSteps = new List<string>() { "1", "2", "3", "4" };
        private List<string> fMonthSteps = new List<string>() { "1", "2", "3", "4", "5", "6" };
        private ComboBox fPredictionStepsComboBox;

        // The granuality of data to aggregate from for power usage levels
        private List<string> fGranualities = new List<string>() { "Appliance", "Household", "Region" };
        private ComboBox fGranualityComboBox;

        private List<string> fStates = new List<string>() { "VIC", "NSW", "QLD", "ACT", "NT", "SA", "WA", "TAS" };
        private ComboBox fStatesComboBox;

        // Will use an events to trigger
        private Button fStart;
        private Button fStop;
        private Button fResume;
        private StackLayout fButtonStack;

        // Contains the currently running solution
        private ControllerBase fSolution = null;

        public Func<IChromosome, bool> resultsDelegate { get; set; }

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
            fSolutionComboBox.SelectedValueChanged += new EventHandler<EventArgs>(SolutionChanged);

            // To start, Algorithm is selected by default. This means the initial data store for both
            // Crossovers and Mutations will also be for Algorithm
            fCrossoverComboBox = new ComboBox
            {
                DataStore = fCrossoversAlgo,
                SelectedIndex = 0
            };
            fFitnessComboBox = new ComboBox
            {
                DataStore = fFitnesses,
                SelectedIndex = 0
            };
            fMutationComboBox = new ComboBox
            {
                DataStore = fMutationsAlgo,
                SelectedIndex = 0
            };
            fSelectionComboBox = new ComboBox
            {
                DataStore = fSelections,
                SelectedIndex = 0
            };
            fReinsertionComboBox = new ComboBox
            {
                DataStore = fReinsertions,
                SelectedIndex = 0
            };
            fPredictionUnitsComboBox = new ComboBox
            {
                DataStore = fPredictionUnits,
                SelectedIndex = 0
            };

            fPredictionUnitsComboBox.SelectedValueChanged += new EventHandler<EventArgs>(PredictionUnitChanged);

            fPredictionStepsComboBox = new ComboBox
            {
                DataStore = fDaySteps,
                SelectedIndex = 0
            };
            fGranualityComboBox = new ComboBox
            {
                DataStore = fGranualities,
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

        // Initialise the list of items contained in the StackLayout
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
                Text = "Predict from State"
            });
            Items.Add(fStatesComboBox);

            Items.Add(new Label
            {
                Text = "Predict by following unit"
            });
            Items.Add(fPredictionUnitsComboBox);

            Items.Add(new Label
            {
                Text = "Prediction for N units"
            });
            Items.Add(fPredictionStepsComboBox);

            Items.Add(new Label
            {
                Text = "Granuality"
            });
            Items.Add(fGranualityComboBox);

            Items.Add(new Label
            {
                Text = "Start/Stop/Resume the prediction"
            });

            // The control buttons will sit in a horizontal stack
            fButtonStack = new StackLayout();
            fButtonStack.Items.Add(fStart);
            fButtonStack.Items.Add(fStop);
            fButtonStack.Items.Add(fResume);
            fButtonStack.Orientation = Orientation.Horizontal;

            Items.Add(fButtonStack);
        }

        // Whenever the solution combo box value is changed, this method will be called
        // This will switch the data store of the appropriate combo boxes for the currently selected solution method
        private void SolutionChanged(object sender, EventArgs e)
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    fCrossoverComboBox.DataStore = fCrossoversAlgo;
                    fCrossoverComboBox.SelectedIndex = 0;

                    fMutationComboBox.DataStore = fMutationsAlgo;
                    fMutationComboBox.SelectedIndex = 0;
                    break;
                case "Genetic Programming":
                    fCrossoverComboBox.DataStore = fCrossoversProg;
                    fCrossoverComboBox.SelectedIndex = 0;

                    fMutationComboBox.DataStore = fMutationsProg;
                    fMutationComboBox.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        // Whenever the prediction units combo box value is changed, this method will be called
        // This will switch the data store of the appropriate combo boxes for the currently selected solution method
        private void PredictionUnitChanged(object sender, EventArgs e)
        {
            switch (fPredictionUnitsComboBox.SelectedValue.ToString())
            {
                case "Days":
                    fPredictionStepsComboBox.DataStore = fDaySteps;
                    fPredictionStepsComboBox.SelectedIndex = 0;
                    break;
                case "Weeks":
                    fPredictionStepsComboBox.DataStore = fWeekSteps;
                    fPredictionStepsComboBox.SelectedIndex = 0;
                    break;
                case "Months":
                    fPredictionStepsComboBox.DataStore = fMonthSteps;
                    fPredictionStepsComboBox.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        // Triggered once the fStart button is clicked
        private void StartSolution(object sender, EventArgs e)
        {
            if (fSolution != null)
                if (fSolution.State == GeneticAlgorithmState.Started || fSolution.State == GeneticAlgorithmState.Resumed)
                    fSolution.Stop();

            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    fSolution = new GeneticAlgoController(new GeneticAlgoChromosome(10, 10),
                                                     CrossoverMethod(),
                                                     FitnessFuntion(),
                                                     MutationMethod(),
                                                     SelectionMethod(),
                                                     0,
                                                     20000,
                                                     10,
                                                     ReinsertionMethod(),
                                                     1000);
                    fSolution.CrossoverProbability = 0.7f;
                    fSolution.MutationProbability = 0.1f;
                    fSolution.addEventFunction(fSolution.DefaultDrawChromosome);
                    fSolution.addEventFunction(resultsDelegate);
                    fSolution.Start();
                    break;
                case "Genetic Programming":
                    fSolution = new GeneticProgController(new GeneticProgChromosome(10, 10),
                                                     CrossoverMethod(),
                                                     FitnessFuntion(),
                                                     MutationMethod(),
                                                     SelectionMethod(),
                                                     0,
                                                     20000,
                                                     10,
                                                     ReinsertionMethod(),
                                                     1000);
                    fSolution.CrossoverProbability = 0.7f;
                    fSolution.MutationProbability = 0.1f;
                    fSolution.addEventFunction(fSolution.DefaultDrawChromosome);
                    fSolution.addEventFunction(resultsDelegate);
                    fSolution.Start();
                    break;
                default:
                    break;
            }
        }

        private void StopSolution(object sender, EventArgs e) { if (fSolution != null && fSolution.State == GeneticAlgorithmState.Started || fSolution.State == GeneticAlgorithmState.Resumed) fSolution.Stop(); }

        private void ResumeSolution(object sender, EventArgs e) { if (fSolution != null && fSolution.State == GeneticAlgorithmState.Stopped) fSolution.Resume(); }

        // Depending on the selected solution, return the selected crossover
        private CrossoverBase CrossoverMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    switch (fCrossoverComboBox.SelectedValue.ToString())
                    {
                        case "One Point":
                            return new AlgoOnePointCrossover();
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fCrossoverComboBox.SelectedValue.ToString())
                    {
                        case "Branch":
                            return new BranchCrossover();
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected fitness function
        private IFitness FitnessFuntion()
        {
            switch (fFitnessComboBox.SelectedValue.ToString())
            {
                case "Error Squared":
                    return new FitnessFunctions();
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected mutation method
        private MutationBase MutationMethod()
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
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fMutationComboBox.SelectedValue.ToString())
                    {
                        case "Uniform":
                          return new UniformTreeMutation();
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected selection method
        private SelectionBase SelectionMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    switch (fSelectionComboBox.SelectedValue.ToString())
                    {
                        case "Tournament":
                            return new TournamentSelection();
                        case "Elite":
                            return new EliteSelection();
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fSelectionComboBox.SelectedValue.ToString())
                    {
                        case "Tournament":
                            return new TournamentSelection();
                        case "Elite":
                            return new EliteSelection();
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        // Depending on the selected solution, return the selected reinsertion method
        private IReinsertion ReinsertionMethod()
        {
            switch (fSolutionComboBox.SelectedValue.ToString())
            {
                case "Genetic Algorithm":
                    switch (fSelectionComboBox.SelectedValue.ToString())
                    {
                        case "Combined Reinsertion":
                            return new CombinedReinsertion();
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fSelectionComboBox.SelectedValue.ToString())
                    {
                        case "Combined Reinsertion":
                            return new CombinedReinsertion();
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }
    }
}
