using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActivityOneManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
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
        _input.PauseEvent += HandlePause;
        _input.ResumeEvent += HandleResume;
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

    private void HandlePause()
    {
        //pauseMenu.SetActive(true);
    }

    private void HandleResume()
    {
        //pauseMenu.SetActive(false);
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
    }
}
