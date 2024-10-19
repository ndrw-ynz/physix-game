using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityOneManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action SNRoomClearEvent;
	public event Action SNCorrectAnswerEvent;
	public event Action VarianceRoomClearEvent;
	public event Action APRoomClearEvent;
	public event Action errorsRoomClearEvent;

	[Header("Level Data - Scientific Notation (SN)")]
	[SerializeField] private ScientificNotationSubActivitySO SNLevelOne;
	[SerializeField] private ScientificNotationSubActivitySO SNLevelTwo;
	[SerializeField] private ScientificNotationSubActivitySO SNLevelThree;
	private ScientificNotationSubActivitySO currentSNLevel;

    [Header("Managers")]
    [SerializeField] private APGraphManager APGraphManager;

    [Header("Handlers")]
    [SerializeField] private ContainerSelectionHandler containerSelectionHandler;

	[Header("Views")]
	[SerializeField] private ContainerPickerView containerPickerView;
    [SerializeField] private ScientificNotationView scientificNotationView;
    [SerializeField] private VarianceView varianceView;
    [SerializeField] private AccuracyPrecisionView accuracyPrecisionView;
    [SerializeField] private ErrorsView errorsView;
    [SerializeField] private ActivityOnePerformanceView performanceView;

	[Header("Submission Status Displays")]
	[SerializeField] private SNSubmissionStatusDisplay scientificNotationSubmissionStatusDisplay;
    [SerializeField] private VarianceSubmissionStatusDisplay varianceSubmissionStatusDisplay;
    [SerializeField] private APSubmissionStatusDisplay accuracyPrecisionSubmissionStatusDisplay;
    [SerializeField] private ErrorsSubmissionStatusDisplay errorsSubmissionStatusDisplay;

	// Variables for keeping track of current number of tests
	private int currentNumSNTests;

	private List<float> numericalContainerValues;

	// Variables for tracking which view is currently active
	private bool isSNViewActive;
	private bool isVarianceViewActive;
	private bool isAPViewActive;
	private bool isErrorsViewActive;

	// Gameplay performance metrics variables
	// Scientific Notation Sub Activity
	private float SNGameplayDuration;
	private bool isSNSubActivityFinished;
	private int numIncorrectSNSubmission;
	private int numCorrectSNSubmission;
	// Variance Sub Activity
	private float varianceGameplayDuration;
	private bool isVarianceSubActivityFinished;
	private int numIncorrectVarianceSubmission;
	private int numCorrectVarianceSubmission;
	// Accuracy Precision Sub Activity
	private float APGameplayDuration;
	private bool isAPSubActivityFinished;
	private int numIncorrectAPSubmission;
	private int numCorrectAPSubmission;
	// Errors Sub Activity
	private float errorsGameplayDuration;
	private bool isErrorsSubActivityFinished;
	private int numIncorrectErrorsSubmission;
	private int numCorrectErrorsSubmission;

	protected override void Start()
	{
		base.Start();

		ConfigureLevelData(Difficulty.Easy);

        SubscribeViewAndDisplayEvents();

		// Initialize given values
		containerSelectionHandler.SetupContainerValues(currentSNLevel);
        numericalContainerValues = new List<float>();

		// Setting number of tests
		currentNumSNTests = currentSNLevel.numberOfTests;

        // Setup views
        scientificNotationView.UpdateNumberOfContainersTextDisplay(0, currentNumSNTests);
	}

	private void Update()
	{
		if (isSNViewActive && !isSNSubActivityFinished) SNGameplayDuration += Time.deltaTime;
		if (isVarianceViewActive && !isVarianceSubActivityFinished) varianceGameplayDuration += Time.deltaTime;
		if (isAPViewActive && !isAPSubActivityFinished) APGameplayDuration += Time.deltaTime;
		if (isErrorsViewActive && !isErrorsSubActivityFinished) errorsGameplayDuration += Time.deltaTime;
		if (isSNSubActivityFinished && isVarianceSubActivityFinished && isAPSubActivityFinished && isErrorsSubActivityFinished) DisplayPerformanceView();
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentSNLevel = SNLevelOne;
				break;
			case Difficulty.Medium:
				currentSNLevel = SNLevelTwo;
				break;
			case Difficulty.Hard:
				currentSNLevel = SNLevelThree;
				break;
		}
	}

    private void SubscribeViewAndDisplayEvents()
    {
        // Scientific Notation Sub Activity Related Events
		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => containerPickerView.UpdateContainerDisplay(boxContainer);
		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => scientificNotationView.UpdateScientificNotationView(boxContainer);
		scientificNotationView.OpenViewEvent += () => isSNViewActive = true;
		scientificNotationView.QuitViewEvent += () => isSNViewActive = false;
		scientificNotationView.SubmitAnswerEvent += CheckScientificNotationAnswer;
		scientificNotationSubmissionStatusDisplay.ProceedEvent += UpdateSNViewState;

		// Variance Sub Activity Related Events
		varianceView.OpenViewEvent += () => isVarianceViewActive = true;
		varianceView.QuitViewEvent += () => isVarianceViewActive = false;
		varianceView.SubmitAnswerEvent += CheckVarianceAnswer;
		varianceSubmissionStatusDisplay.ProceedEvent += UpdateVarianceViewState;

		// Accuracy Precision Sub Activity Related Events
		accuracyPrecisionView.OpenViewEvent += () => isAPViewActive = true;
		accuracyPrecisionView.QuitViewEvent += () => isAPViewActive = false;
		accuracyPrecisionView.SubmitAnswerEvent += CheckAccuracyPrecisionAnswer;
		accuracyPrecisionSubmissionStatusDisplay.ProceedEvent += UpdateAPViewState;

		// Errors Sub Activity Related Events
		errorsView.OpenViewEvent += () => isErrorsViewActive = true;
		errorsView.QuitViewEvent += () => isErrorsViewActive = false;
		errorsView.SubmitAnswerEvent += CheckErrorsAnswer;
        errorsSubmissionStatusDisplay.ProceedEvent += UpdateErrorsViewState;
	}

	#region Scientific Notation

	private void CheckScientificNotationAnswer(ScientificNotationAnswerSubmission answer)
    {
        BoxContainer selectedContainer = containerSelectionHandler.GetSelectedContainer();
		ScientificNotationAnswerSubmissionResults results = ActivityOneUtilities.ValidateScientificNotationSubmission(answer, selectedContainer.numericalValue, selectedContainer.unitOfMeasurement);

		if (results.isAllCorrect())
		{
			numCorrectSNSubmission++;
			currentNumSNTests--;
            numericalContainerValues.Add(selectedContainer.numericalValue);
		}
		else
		{
			numIncorrectSNSubmission++;
		}

		DisplaySNSubmissionResults(results);
	}

	private void DisplaySNSubmissionResults(ScientificNotationAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			scientificNotationSubmissionStatusDisplay.SetSubmissionStatus(true, "Great job! Your calculations are correct. Making containers for ejection.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Select storage containers and determine its value in standard scientific notation ({currentNumSNTests}/{currentSNLevel.numberOfTests})");
		}
		else
		{
			scientificNotationSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		scientificNotationSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		scientificNotationSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateSNViewState()
	{
		SNCorrectAnswerEvent.Invoke();
		if (currentNumSNTests > 0)
		{
            scientificNotationView.UpdateNumberOfContainersTextDisplay(currentNumSNTests, currentSNLevel.numberOfTests);
			scientificNotationView.UpdateScientificNotationView(containerSelectionHandler.GetSelectedContainer());
		}
		else
		{
            isSNSubActivityFinished = true;
            SNRoomClearEvent?.Invoke();
            varianceView.SetupVarianceView(numericalContainerValues);
			missionObjectiveDisplayUI.ClearMissionObjective(0);
		}
		containerPickerView.UpdateContainerDisplay(null);
		scientificNotationView.gameObject.SetActive(false);
	}

	#endregion

	#region Variance
    private void CheckVarianceAnswer(VarianceAnswerSubmission answer)
    {
		VarianceAnswerSubmissionResults results = ActivityOneUtilities.ValidateVarianceSubmission(answer, numericalContainerValues);

		if (results.isAllCorrect())
		{
			numCorrectVarianceSubmission++;
		}
		else
		{
			numIncorrectVarianceSubmission++;
		}

        DisplayVarianceSubmissionResults(results);
	}

	private void DisplayVarianceSubmissionResults(VarianceAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			varianceSubmissionStatusDisplay.SetSubmissionStatus(true, "Amazing work! Container spread verified. Now placing containers on the ejecton site.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the variance of the selected storage containers (1/1)");

		}
		else
		{
			varianceSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		varianceSubmissionStatusDisplay.gameObject.SetActive(true);

		varianceSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);
	}

	private void UpdateVarianceViewState()
	{
		isVarianceSubActivityFinished = true;
		varianceView.gameObject.SetActive(false);
        VarianceRoomClearEvent?.Invoke();
		missionObjectiveDisplayUI.ClearMissionObjective(1);
	}

	#endregion

	#region Accuracy and Precision
	private void CheckAccuracyPrecisionAnswer(APGraphType? answer) 
    {
        APGraphType correctGraphType = APGraphManager.GetGraphTypeFromGraphs(1);
        bool result = answer == correctGraphType;

		if (result)
		{
			numCorrectAPSubmission++;
		}
		else
		{
			numIncorrectAPSubmission++;
		}

        DisplayAccuracyPrecisionSubmissionResult(result);
	}

    private void DisplayAccuracyPrecisionSubmissionResult(bool result)
    {
        if (result)
        {
			accuracyPrecisionSubmissionStatusDisplay.SetSubmissionStatus(true, "Great work! Containers are prepared and ready for ejection. Head to the last panel.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Determine the accuracy & precision of the placement of the selected containers (1/1)");
		}
		else
        {
			accuracyPrecisionSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		accuracyPrecisionSubmissionStatusDisplay.gameObject.SetActive(true);

		accuracyPrecisionSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);
	}

    private void UpdateAPViewState()
    {
		isAPSubActivityFinished = true;
		accuracyPrecisionView.gameObject.SetActive(false);
		APRoomClearEvent?.Invoke();
		missionObjectiveDisplayUI.ClearMissionObjective(2);
	}

	#endregion

	#region Errors
	private void CheckErrorsAnswer(ErrorType? answer)
    {
        APGraphType[] givenGraphTypes =
        {
            APGraphManager.GetGraphTypeFromGraphs(1),
            APGraphManager.GetGraphTypeFromGraphs(2),
            APGraphManager.GetGraphTypeFromGraphs(3)
        };

		bool result = ActivityOneUtilities.ValidateErrorsSubmission(answer, givenGraphTypes);

		if (result)
		{
			numCorrectErrorsSubmission++;
		}
		else
		{
			numIncorrectErrorsSubmission++;
		}

		DisplayErrorsSubmissionResult(result);
	}

    private void DisplayErrorsSubmissionResult(bool result)
    {
		if (result)
		{
			errorsSubmissionStatusDisplay.SetSubmissionStatus(true, "Nicely done! You may now eject the containers.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(3, $"Determine the type of error from the positions of the selected containers (1/1)");
		}
		else
		{
			errorsSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		errorsSubmissionStatusDisplay.gameObject.SetActive(true);

		errorsSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);
	}

    private void UpdateErrorsViewState()
    {
		isErrorsSubActivityFinished = true;
		errorsView.gameObject.SetActive(false);
		errorsRoomClearEvent?.Invoke();
		missionObjectiveDisplayUI.ClearMissionObjective(3);
	}
	#endregion

	public override void DisplayPerformanceView()
	{
		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(SNGameplayDuration + varianceGameplayDuration + APGameplayDuration + errorsGameplayDuration);

		performanceView.SetScientificNotationMetricsDisplay(
			isAccomplished: isSNSubActivityFinished,
			numIncorrectSubmission: numIncorrectSNSubmission,
			duration: SNGameplayDuration
			);

		performanceView.SetVarianceMetricsDisplay(
			isAccomplished: isVarianceSubActivityFinished,
			numIncorrectSubmission: numIncorrectVarianceSubmission,
			duration: varianceGameplayDuration
			);

		performanceView.SetAccuracyPrecisionMetricsDisplay(
			isAccomplished: isAPSubActivityFinished,
			numIncorrectSubmission: numIncorrectAPSubmission,
			duration: APGameplayDuration
			);

		performanceView.SetErrorsMetricsDisplay(
			isAccomplished: isErrorsSubActivityFinished,
			numIncorrectSubmission: numIncorrectErrorsSubmission,
			duration: errorsGameplayDuration
			);

		// Update its activity feedback display (four args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "scientific notation",
				isSubActivityFinished: isSNSubActivityFinished,
				numIncorrectAnswers: numIncorrectSNSubmission,
				numCorrectAnswers: numCorrectSNSubmission,
				badScoreThreshold: 6,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "variance",
				isSubActivityFinished: isVarianceSubActivityFinished,
				numIncorrectAnswers: numIncorrectVarianceSubmission,
				numCorrectAnswers: numCorrectVarianceSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "accuracy & precision",
				isSubActivityFinished: isAPSubActivityFinished,
				numIncorrectAnswers: numIncorrectAPSubmission,
				numCorrectAnswers: numCorrectAPSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "errors",
				isSubActivityFinished: isErrorsSubActivityFinished,
				numIncorrectAnswers: numIncorrectErrorsSubmission,
				numCorrectAnswers: numCorrectErrorsSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				)
			);
	}

	protected override void HandleGameplayPause()
	{
		base.HandleGameplayPause();
		// Update content of activity pause menu UI
		List<string> taskText = new List<string>();
		if (!isSNSubActivityFinished)
		{
			taskText.Add($"- Select storage containers and determine its value in standard scientific notation.");
		}
		if (!isVarianceSubActivityFinished)
		{
			taskText.Add("- Calculate the variance of the selected storage containers.");
		}
		if (!isAPSubActivityFinished)
		{
			taskText.Add("- Determine the accuracy & precision of the placement of the selected containers.");
		}
		if (!isErrorsSubActivityFinished)
		{
			taskText.Add("- Determine the type of error from the positions of the selected containers.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Eject containers from the space rocket. ");

		activityPauseMenuUI.UpdateContent("Lesson 1 - Activity 1", taskText, objectiveText);
	}

	// --  OLD CODE --
	// -- REMOVE AFTER REFACTORING WHOLE SCRIPT -- 

	/*[SerializeField] private InputReader _input;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private ScientificNotationSO level1;

    public List<BoxContainer> ejectionAreaOneBoxContainers;
    private List<BoxContainer> _ejectionAreaTwoBoxContainers;
    private List<BoxContainer> _ejectionAreaThreeBoxContainers;

    private Bounds _ejectionAreaOne;
    private Bounds _ejectionAreaTwo;
    private Bounds _ejectionAreaThree;

    private BoxContainer _currentBoxContainer;
    private int _correctAnswer;

    public GameObject boxContainerPrefab;

    // Gameplay performance metrics

    // For Scientific Notation activity
    public bool isScientificNotationFinished;
    private int numIncorrectSNSubmission;

    // For Accuracy and Precision activity
    public bool isAccuracyAndPrecisionFinished;
    private int numIncorrectAPSubmission;

    // For Variance activity
    public bool isVarianceFinished;
    private int numIncorrectVarianceSubmission;

    // For Errors activity
    public bool isErrorsFinished;
    private int numIncorrectErrorsSubmission;

    void Start()
    {
        BoxContainer.BoxInteractEvent += OnSelectBox;
        ViewScientificNotation.OpenViewEvent += OnOpenViewSN;
        ViewScientificNotation.SubmitAnswerEvent += CheckSubmittedSNAnswer;
        ViewAccuracyPrecision.OpenViewEvent += OnOpenViewAP;
        ViewAccuracyPrecision.SubmitAPEvent += CheckAPAnswer;
        ViewVariance.OpenVarianceEvent += OnOpenViewVariance;
        ViewVariance.SubmitVarianceEvent += CheckVarianceAnswer;
        ViewErrors.SubmitErrorsEvent += CheckErrorsAnswer;
        ViewActivityOnePerformance.OpenViewEvent += OnOpenViewActivityOnePerformance;

        // Handle event for randomizing contents of box containers.
        RandomlyGenerateBoxValues();

        // Gets boundaries for ejecton areas.
        _ejectionAreaOne = GameObject.Find("Ejection Area One").GetComponent<Renderer>().bounds;
		_ejectionAreaTwo = GameObject.Find("Ejection Area Two").GetComponent<Renderer>().bounds;
		_ejectionAreaThree = GameObject.Find("Ejection Area Three").GetComponent<Renderer>().bounds;

        // Initialize box containers
        _ejectionAreaTwoBoxContainers = new List<BoxContainer>();
        _ejectionAreaThreeBoxContainers = new List<BoxContainer>();

		// Fill up empty ejection areas.
		SetupEjectionArea(_ejectionAreaTwo, _ejectionAreaTwoBoxContainers);
        SetupEjectionArea(_ejectionAreaThree, _ejectionAreaThreeBoxContainers);

		// Set bool values for panels.
		isScientificNotationFinished = false;
        isAccuracyAndPrecisionFinished = false;
        isVarianceFinished = false;
        isErrorsFinished = false;
    }

    private void RandomlyGenerateBoxValues()
    {
        foreach (BoxContainer box in ejectionAreaOneBoxContainers)
        {
            box.SetValues(level1);
        }
    }
    private void SetupEjectionArea(Bounds ejectionArea, List<BoxContainer> ejectionAreaBoxes)
    {
		Vector3 center = ejectionArea.center;
		Vector3 extents = ejectionArea.extents;
        for (int quadrant = 0; quadrant < 4; quadrant++)
        {

            GameObject instantiatedBoxContainer = Instantiate(boxContainerPrefab);
            BoxContainer boxContainer = instantiatedBoxContainer.GetComponentInChildren<BoxContainer>();
            
		    Vector3 randomPosition = ejectionArea.center;

		    if (quadrant == 1)
		    {
			    randomPosition.x = Random.Range(center.x - extents.x, center.x);
			    randomPosition.z = Random.Range(center.z, center.z + extents.z);
		    }
		    if (quadrant == 2)
		    {
			    randomPosition.x = Random.Range(center.x, center.x + extents.x);
			    randomPosition.z = Random.Range(center.z, center.z + extents.z);
		    }
		    if (quadrant == 3)
		    {
			    randomPosition.x = Random.Range(center.x - extents.x, center.x);
			    randomPosition.z = Random.Range(center.z, center.z - extents.z);
		    }
		    if (quadrant == 4)
		    {
			    randomPosition.x = Random.Range(center.x, center.x + extents.x);
			    randomPosition.z = Random.Range(center.z, center.z - extents.z);
		    }
		    boxContainer.transform.position = randomPosition;

			ejectionAreaBoxes.Add(boxContainer);
        }   
	}
    private void OnSelectBox(BoxContainer container)
    {
        _currentBoxContainer = container;
    }

    private void OnOpenViewSN(ViewScientificNotation view)
    {
        if (_currentBoxContainer != null)
        {
            view.measurementText.text = _currentBoxContainer.measurementText.text;
        }
    }
    
    private void OnOpenViewAP(ViewAccuracyPrecision view)
    {
        
    }

    private void OnOpenViewVariance(ViewVariance view)
    {
        for (int i = 0; i < view.givenNumbers.Count; i++)
        {
            float d = Vector3.Distance(ejectionAreaOneBoxContainers[i].transform.position, _ejectionAreaOne.center);
            view.givenNumbers[i].SetValue((float) Math.Round(d, 2));
        }
    }

    private void OnOpenViewActivityOnePerformance(ViewActivityOnePerformance view)
    {
        view.SNStatusText.text += isScientificNotationFinished ? "Accomplished" : "Not accomplished";
        view.SNIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectSNSubmission}";

		view.APStatusText.text += isAccuracyAndPrecisionFinished ? "Accomplished" : "Not accomplished";
		view.APIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectAPSubmission}";

		view.VarianceStatusText.text += isVarianceFinished ? "Accomplished" : "Not accomplished";
		view.VarianceIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectVarianceSubmission}";

		view.ErrorsStatusText.text += isErrorsFinished ? "Accomplished" : "Not accomplished";
		view.ErrorsIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectErrorsSubmission}";
	}

    private void CheckSubmittedSNAnswer(string answer)
    {
        // with contents of _currentboxcontainer, convert to proper answer, and compare with answer.
        Debug.Log($"Checking answer: {answer}");
        string correctAnswer = GetCorrectSNAnswer(_currentBoxContainer.numericalValue, _currentBoxContainer.unitOfMeasurement);
        Debug.Log($"Desired answer: {correctAnswer}");
        bool isCorrect = answer.Equals(correctAnswer);
        Debug.Log("Result: " + isCorrect);
        // Handle random position for box 
        if (isCorrect)
        {
            _correctAnswer += 1;

            Vector3 center = _ejectionAreaOne.center;
            Vector3 extents = _ejectionAreaOne.extents;
            Vector3 randomPosition = _ejectionAreaOne.center;
            // Multiply extents, only alter x and z
            if (_correctAnswer == 1)
            {
                randomPosition.x = Random.Range(center.x - extents.x, center.x);
                randomPosition.z = Random.Range(center.z, center.z + extents.z);
            }
            if (_correctAnswer == 2)
            {
                randomPosition.x = Random.Range(center.x, center.x + extents.x);
                randomPosition.z = Random.Range(center.z, center.z + extents.z);
            }
            if (_correctAnswer == 3)
            {
                randomPosition.x = Random.Range(center.x - extents.x, center.x);
                randomPosition.z = Random.Range(center.z, center.z - extents.z);
            }
            if (_correctAnswer == 4)
            {
                randomPosition.x = Random.Range(center.x, center.x + extents.x);
                randomPosition.z = Random.Range(center.z, center.z - extents.z);

                
                isScientificNotationFinished = true;
            }
            _currentBoxContainer.transform.position = randomPosition;
        } else
        {
            numIncorrectSNSubmission++;
        }
    }

    private string GetCorrectSNAnswer(int numericalValue, string unitOfMeasurement)
    {
        Dictionary<string, int> unitPowers = new Dictionary<string, int>()
        {
            { "Kilometer", 3 },
            { "Hectometer", 2 },
            { "Dekameter", 1 },
            { "Decimeter", -1 },
            { "Centimeter", -2 },
            { "Millimeter", -3 }
        };

        if (unitPowers.TryGetValue(unitOfMeasurement, out int power))
        {
            double baseForm = numericalValue * Math.Pow(10, power);
            int exponent = (int)Math.Floor(Math.Log10(Math.Abs(baseForm)));

            double coefficient = baseForm / Math.Pow(10, exponent);

            string valueString = baseForm.ToString();

            valueString = valueString.Replace(".", "");

            valueString = valueString.TrimStart('0').TrimEnd('0');

            int significantFigures = valueString.Length == 1 ? valueString.Length : valueString.Length-1;

            string formattedCoefficient = coefficient.ToString($"F{significantFigures}");

            return $"{formattedCoefficient} x 10^{exponent}";
        }
        else
        {
            Debug.LogError("Unit of measurement not found!");
            return null;
        }
    }

    // method for determining accuracy
    private bool IsAccurate(Bounds ejectionArea, List<BoxContainer> boxContainers)
    {
        Debug.Log("Determining accuracy!");
		Vector3 center = ejectionArea.center;
		Vector3 extents = ejectionArea.extents;

        float sum = 0;

		foreach (BoxContainer box in boxContainers)
        {
            Bounds boxBounds = box.GetComponent<Renderer>().bounds;
            float dx = Math.Abs(boxBounds.center.x - center.x);
            float dy = Math.Abs(boxBounds.center.y - center.y);
            float distance = Vector3.Distance(boxBounds.center, center);
            //Debug.Log("Distance of a box: " + distance);
            sum += distance;
        }
        float avg = sum/4;

        float acceptableAvg = extents.x / 2;

        //Debug.Log("Average: " + avg);
        //Debug.Log("Acceptable avg: " + acceptableAvg);

        return avg <= acceptableAvg ;
    }

    // This method determines precision of boxes, the standard
    // being that the measure of sd is within 1 sd.
    private bool IsPrecise(List<BoxContainer> boxContainers)
    {
        List<float> distanceValues = new List<float>();

        // Compute centroid of boxes
        Vector3 centroid = new Vector3();
        foreach (BoxContainer box in boxContainers)
        {
			Bounds boxBounds = box.GetComponent<Renderer>().bounds;
            
            centroid += boxBounds.center;
        }
        centroid /= 4;

        // Calculate average distance to centroid
        float avgDistance = 0;
        foreach (BoxContainer box in boxContainers)
        {
			Bounds boxBounds = box.GetComponent<Renderer>().bounds;
            float boxDistance = Vector3.Distance(centroid, boxBounds.center);

            distanceValues.Add(boxDistance);
			avgDistance += boxDistance;
		}
        avgDistance /= 4;

        // Calculate standard deviation
        double sd = 0;
        foreach (float distanceValue in distanceValues)
        {
            sd += Math.Pow(distanceValue-avgDistance, 2);
		}
        sd /= 3;
        sd = Math.Sqrt(sd);

        // Compare standard deviation to be within 1 sd
        if (sd < 1)
        {
            Debug.Log("Precise!");
        } else
        {
            Debug.Log("Not precise!");
        }

		return sd < 1;
    }

    private void CheckAPAnswer(bool accuracySubmission, bool precisionSubmission)
    {
        bool actualAccuracy = IsAccurate(_ejectionAreaOne, ejectionAreaOneBoxContainers);
		bool actualPrecision = IsPrecise(ejectionAreaOneBoxContainers);
        if (actualAccuracy == accuracySubmission && actualPrecision == precisionSubmission)
        {
            Debug.Log("AP Answer is correct.");
            
            isAccuracyAndPrecisionFinished = true;
        } else
        {
            Debug.Log("AP Answer is incorrect.");

            numIncorrectAPSubmission++;
        }
    }

	private double CalculateVariance()
	{
		List<float> distanceValues = GetBoxDistanceValues(_ejectionAreaOne, ejectionAreaOneBoxContainers);
		// Calculate average of values
		double avg = 0;
		foreach (float d in distanceValues)
		{
			avg += d;
		}
		avg = Math.Round(avg / 4, 4);
		// Calculate variance
		double variance = 0;
		foreach (float d in distanceValues)
		{
			variance += Math.Pow(d - avg, 2);
		}
		variance = Math.Round(variance / 3, 4);

		return variance;
	}
	private List<float> GetBoxDistanceValues(Bounds ejectionArea, List<BoxContainer> boxContainers)
	{
		List<float> values = new List<float>();
		foreach (BoxContainer box in boxContainers)
		{
			float d = Vector3.Distance(box.transform.position, ejectionArea.center);
			values.Add((float)Math.Round(d, 2));
		}
		return values;
	}

    private void CheckVarianceAnswer(float submittedAnswer)
    {
		double varianceAnswer = Math.Round(CalculateVariance(), 4);

		Debug.Log("Variance answer: " + varianceAnswer);
		Debug.Log("Submitted answer: " + submittedAnswer);

		bool isApproximatelyCorrect = Mathf.Abs((float)(varianceAnswer - submittedAnswer)) <= 0.0001;
		Debug.Log("Is approximately correct: " + isApproximatelyCorrect);

        if (isApproximatelyCorrect)
        {
            isVarianceFinished = true;
        } else
        {
            numIncorrectVarianceSubmission++;
        }
	}

	public bool IsSystematicError()
    {
        int numOfAccurate = 0;
        numOfAccurate += IsAccurate(_ejectionAreaOne, ejectionAreaOneBoxContainers) ? 1 : 0;
		numOfAccurate += IsAccurate(_ejectionAreaTwo, _ejectionAreaTwoBoxContainers) ? 1 : 0;
		numOfAccurate += IsAccurate(_ejectionAreaThree, _ejectionAreaThreeBoxContainers) ? 1 : 0;

		return numOfAccurate <= 1;
    }

    public bool IsRandomError()
    {
        int numOfPrecise = 0;
        numOfPrecise += IsPrecise(ejectionAreaOneBoxContainers) ? 1 : 0;
		numOfPrecise += IsPrecise(_ejectionAreaTwoBoxContainers) ? 1 : 0;
		numOfPrecise += IsPrecise(_ejectionAreaThreeBoxContainers) ? 1 : 0;

		return numOfPrecise <= 1;
    }

    public void CheckErrorsAnswer(bool systematicErrorSubmission, bool randomErrorSubmission)
    {
        bool actualSystematicError = IsSystematicError();
        bool actualRandomError = IsRandomError();
        Debug.Log($"Submitted Errors Answers: S={systematicErrorSubmission} R={randomErrorSubmission}");
        Debug.Log($"Actual Errors Answers: S={actualSystematicError} R={actualRandomError}");
        if (actualSystematicError == systematicErrorSubmission && actualRandomError == randomErrorSubmission)
        {
            Debug.Log("Errors answer is correct.");
            isErrorsFinished = true;
        } else
        {
            Debug.Log("Errors answer is incorrect.");
            numIncorrectErrorsSubmission++;
        }
    }*/
}
