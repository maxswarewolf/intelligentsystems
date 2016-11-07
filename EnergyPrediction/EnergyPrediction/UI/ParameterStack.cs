using Eto.Forms;
using System;

using System.Collections.Generic;

using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using System.Threading;
using GeneticSharp.Domain.Populations;

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
        private List<string> fPredictionUnits = new List<string>() { "Hours", "Days", "Weeks" };
        private ComboBox fPredictionUnitsComboBox;

        // The number of a particular unit of time that will be predicted eg 5 fDaySteps will predict 5 days into the future
        private List<string> fHourSteps = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
        private List<string> fDaySteps = new List<string>() { "1", "2", "3", "4", "5", "6", "7" };
        private List<string> fWeekSteps = new List<string>() { "1", "2", "3", "4" };
        private ComboBox fPredictionStepsComboBox;

        // How much real data is to be used for the prediction
        private List<string> fHistoricalHours = new List<string>() { "24", "48", "72", "96", "120", "144", "168" };
        private List<string> fHistoricalDays = new List<string>() { "1", "2", "3", "4", "5", "6", "7" };
        private List<string> fHistoricalWeeks = new List<string>() { "1", "2", "3", "4" };
        private ComboBox fHistoricalUnitsComboBox;

        // The granuality of data to aggregate from for power usage levels
        private List<string> fGranularities = new List<string>() { "Appliance", "Household", "Region" };
        private ComboBox fGranularityComboBox;

        // The appliancce to predict. Disabled if not predicting appliances
        private List<string> fAppliances = new List<string>() { "TV", "Fan", "Fridge", "Laptop", "Heater", "Oven", "Washing Machine", "Microwave", "Toaster", "Wall Socket", "Cooker" };
        private ComboBox fApplianceComboBox;

        private List<string> fStates = new List<string>() { "VIC", "NSW", "QLD", "ACT", "SA", "TAS" };
        private ComboBox fStatesComboBox;

        // Will use an events to trigger
        private Button fStart;
        private Button fStop;
        // And this will hide/show real data
        private Button fToggleReal;
        private StackLayout fButtonStack;

        // The start date for all real data 7/9/2015
        private DateTime dataStartDate = new DateTime(2015, 9, 7);

        // Contains the currently running solution
        private ControllerBase fSolution = null;

        public Func<IChromosome, bool> updateResultsDelegate { get; set; }

        public Func<string, bool> updateAxisTitleDelegate { get; set; }

        public Func<string, bool> updatePredictionStepsDelegate { get; set; }

        public Func<string, bool> toggleRealDataDelegate { get; set; }

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
            fPredictionUnitsComboBox.SelectedValueChanged += delegate
            {
                updateAxisTitleDelegate(fPredictionUnitsComboBox.SelectedValue.ToString());
            };

            fPredictionStepsComboBox = new ComboBox
            {
                DataStore = fHourSteps,
                SelectedIndex = 0
            };

            fPredictionStepsComboBox.SelectedValueChanged += delegate
            {
                updatePredictionStepsDelegate(fPredictionStepsComboBox.SelectedValue.ToString());
            };

            fHistoricalUnitsComboBox = new ComboBox
            {
                DataStore = fHistoricalHours,
                SelectedIndex = 0
            };

            fHistoricalUnitsComboBox.SelectedValueChanged += new EventHandler<EventArgs>(PredictionUnitChanged);

            fGranularityComboBox = new ComboBox
            {
                DataStore = fGranularities,
                SelectedIndex = 0
            };

            fGranularityComboBox.SelectedValueChanged += new EventHandler<EventArgs>(GranualityChanged);

            fApplianceComboBox = new ComboBox
            {
                DataStore = fAppliances,
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
            fStop.Enabled = false;

            fToggleReal = new Button();
            fToggleReal.MouseDown += delegate
            {
                toggleRealDataDelegate("This string will not be used, but is necessary for a delegate");
            };
            fToggleReal.Text = "Toggle Real Data";
            fToggleReal.Enabled = false;

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
                Text = "Granularity"
            });
            Items.Add(fGranularityComboBox);

            Items.Add(new Label
            {
                Text = "Appliance to predict"
            });
            Items.Add(fApplianceComboBox);

            Items.Add(new Label
            {
                Text = "Start/Stop the prediction"
            });

            // The control buttons will sit in a horizontal stack
            fButtonStack = new StackLayout();
            fButtonStack.Items.Add(fStart);
            fButtonStack.Items.Add(fStop);
            fButtonStack.Items.Add(fToggleReal);
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
            ComboBox senderBox = sender as ComboBox;
            if (senderBox != null)
            {
                if (senderBox == fPredictionUnitsComboBox)
                {
                    switch (fPredictionUnitsComboBox.SelectedValue.ToString())
                    {
                        case "Hours":
                            fPredictionStepsComboBox.DataStore = fHourSteps;
                            fPredictionStepsComboBox.SelectedIndex = 0;
                            break;
                        case "Days":
                            fPredictionStepsComboBox.DataStore = fDaySteps;
                            fPredictionStepsComboBox.SelectedIndex = 0;
                            break;
                        case "Weeks":
                            fPredictionStepsComboBox.DataStore = fWeekSteps;
                            fPredictionStepsComboBox.SelectedIndex = 0;
                            break;
                        default:
                            break;
                    }
                }
                else if (senderBox == fHistoricalUnitsComboBox)
                {
                    switch (fPredictionUnitsComboBox.SelectedValue.ToString())
                    {
                        case "Hours":
                            fHistoricalUnitsComboBox.DataStore = fHistoricalHours;
                            fHistoricalUnitsComboBox.SelectedIndex = 0;
                            break;
                        case "Days":
                            fHistoricalUnitsComboBox.DataStore = fHistoricalDays;
                            fHistoricalUnitsComboBox.SelectedIndex = 0;
                            break;
                        case "Weeks":
                            fHistoricalUnitsComboBox.DataStore = fHistoricalWeeks;
                            fHistoricalUnitsComboBox.SelectedIndex = 0;
                            break;
                        default:
                            break;
                    }
                }
                else throw new InvalidOperationException("SenderBox in ParameterStack is neither a fPredictionStepsComboBox or a fHistoricalUnitsComboBox - This should never happen");
            }
            else throw new InvalidCastException("Sender in ParameterStack PredictionUnitChanged is not a ComboBox - This should never happen");
        }

        private void GranualityChanged(object sender, EventArgs e)
        {
            switch (fGranularityComboBox.SelectedValue.ToString())
            {
                case "Appliance":
                    fApplianceComboBox.Enabled = true;
                    break;
                default:
                    fApplianceComboBox.Enabled = false;
                    break;
            }
        }

        // Triggered once the fStart button is clicked
        private void StartSolution(object sender, EventArgs e)
        {
            if (fSolution != null)
            {
                if (fSolution.State == GeneticAlgorithmState.Stopped)
                {
                    CreateSolution();
                    new Thread(() => { fSolution.Start(); }).Start();
                    ToggleUIOnStart();
                }
            }
            else
            {
                CreateSolution();
                new Thread(() => { fSolution.Start(); }).Start();
                ToggleUIOnStart();
            }
        }

        private void CreateSolution()
        {
            LoadData();

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
                    fSolution.addEventFunction(updateResultsDelegate);
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
                    fSolution.addEventFunction(updateResultsDelegate);
                    break;
                default:
                    break;
            }
        }

        private void LoadData()
        {
            StateType selectedState = StateType.VIC;

            switch (fStatesComboBox.SelectedValue.ToString())
            {
                case "VIC":
                    selectedState = StateType.VIC;
                    break;
                case "NSW":
                    selectedState = StateType.NSW;
                    break;
                case "QLD":
                    selectedState = StateType.QLD;
                    break;
                case "ACT":
                    selectedState = StateType.ACT;
                    break;
                case "SA":
                    selectedState = StateType.SA;
                    break;
                case "TAS":
                    selectedState = StateType.TAS;
                    break;
                default:
                    break;
            }

            int aggregationInterval = 12;

            switch (fPredictionUnitsComboBox.SelectedValue.ToString())
            {
                case "Hours":
                    // 12 5 minute clusters an hour
                    aggregationInterval = 12;
                    break;
                case "Days":
                    // 288 5 minute clusters a day
                    aggregationInterval = 288;
                    break;
                case "Weeks":
                    // 2016 5 minute clusters a week
                    aggregationInterval = 2016;
                    break;
            }

            DateTime endDate = new DateTime();

            switch (fPredictionUnitsComboBox.SelectedValue.ToString())
            {
                case "Hours":
                    int hourSteps = int.Parse(fPredictionStepsComboBox.SelectedValue.ToString());
                    endDate = dataStartDate.AddHours(hourSteps);
                    break;
                case "Days":
                    int daySteps = int.Parse(fPredictionStepsComboBox.SelectedValue.ToString());
                    endDate = dataStartDate.AddDays(daySteps);
                    break;
                case "Weeks":
                    int weekSteps = int.Parse(fPredictionStepsComboBox.SelectedValue.ToString()) * 7;
                    endDate = dataStartDate.AddDays(weekSteps);
                    break;
            }

            DataIO.AggregateData(selectedState, dataStartDate, endDate, aggregationInterval);
        }

        private void StopSolution(object sender, EventArgs e)
        {
            if (fSolution != null)
            {
                if (fSolution.State == GeneticAlgorithmState.Started || fSolution.State == GeneticAlgorithmState.Resumed) fSolution.Stop(); ToggleUIOnStop();
            }
        }

        // Used to toggle the Enabled field of all of the UI elements. This will prevent weird behaviour while
        // the solution is running
        private void ToggleUIOnStart()
        {
            fSolutionComboBox       .Enabled = !fSolutionComboBox.Enabled;
            fCrossoverComboBox      .Enabled = !fCrossoverComboBox.Enabled;
            fFitnessComboBox        .Enabled = !fFitnessComboBox.Enabled;
            fMutationComboBox       .Enabled = !fMutationComboBox.Enabled;
            fSelectionComboBox      .Enabled = !fSelectionComboBox.Enabled;
            fReinsertionComboBox    .Enabled = !fReinsertionComboBox.Enabled;
            fStatesComboBox         .Enabled = !fStatesComboBox.Enabled;
            fPredictionUnitsComboBox.Enabled = !fPredictionUnitsComboBox.Enabled;
            fPredictionStepsComboBox.Enabled = !fPredictionStepsComboBox.Enabled;
            fGranularityComboBox    .Enabled = !fGranularityComboBox.Enabled;
            if (fApplianceComboBox.Enabled) fApplianceComboBox.Enabled = false;
            fStart                  .Enabled = !fStart.Enabled;
            fStop                   .Enabled = !fStop.Enabled;
            fToggleReal             .Enabled = !fToggleReal.Enabled;
        }

        private void ToggleUIOnStop()
        {
            fSolutionComboBox.Enabled = !fSolutionComboBox.Enabled;
            fCrossoverComboBox.Enabled = !fCrossoverComboBox.Enabled;
            fFitnessComboBox.Enabled = !fFitnessComboBox.Enabled;
            fMutationComboBox.Enabled = !fMutationComboBox.Enabled;
            fSelectionComboBox.Enabled = !fSelectionComboBox.Enabled;
            fReinsertionComboBox.Enabled = !fReinsertionComboBox.Enabled;
            fStatesComboBox.Enabled = !fStatesComboBox.Enabled;
            fPredictionUnitsComboBox.Enabled = !fPredictionUnitsComboBox.Enabled;
            fPredictionStepsComboBox.Enabled = !fPredictionStepsComboBox.Enabled;
            fGranularityComboBox.Enabled = !fGranularityComboBox.Enabled;
            switch (fGranularityComboBox.SelectedValue.ToString())
            {
                case "Appliance":
                    fApplianceComboBox.Enabled = true;
                    break;
                default:
                    fApplianceComboBox.Enabled = false;
                    break;
            }
            fStart.Enabled = !fStart.Enabled;
            fStop.Enabled = !fStop.Enabled;
            fToggleReal.Enabled = !fToggleReal.Enabled;
        }

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
                    switch (fReinsertionComboBox.SelectedValue.ToString())
                    {
                        case "Combined Reinsertion":
                            return new CombinedReinsertion();
                        default:
                            return null;
                    }
                case "Genetic Programming":
                    switch (fReinsertionComboBox.SelectedValue.ToString())
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
