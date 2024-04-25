using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private ScientificNotationSO level1;
    [SerializeField] private List<BoxContainer> boxContainers;
    private Bounds _APFloorBounds;

    private BoxContainer _currentBoxContainer;
    private int _correctAnswer;

    void Start()
    {
        _input.PauseEvent += HandlePause;
        _input.ResumeEvent += HandleResume;
        BoxContainer.BoxInteractEvent += OnSelectBox;
        ViewScientificNotation.OpenViewEvent += OnOpenViewSN;
        ViewScientificNotation.SubmitAnswerEvent += CheckSubmittedAnswer;
        ViewAccuracyPrecision.OpenViewEvent += OnOpenViewAP;
        ViewAccuracyPrecision.SubmitAPEvent += CheckAPAnswer;

        // TODO: handle event for randomizing contents of box containers
        RandomlyGenerateBoxValues();

        // Gets boundaries for AP Floor.
        _APFloorBounds = GameObject.Find("AP Floor").GetComponent<Renderer>().bounds;
        // Display the bounds properties
        Debug.Log("Center: " + _APFloorBounds.center);
        Debug.Log("Size: " + _APFloorBounds.size);
        Debug.Log("Extents: " + _APFloorBounds.extents);
        Debug.Log("Min: " + _APFloorBounds.min);
        Debug.Log("Max: " + _APFloorBounds.max);

        // Calculate the width, length, and height
        float width = _APFloorBounds.size.x;
        float length = _APFloorBounds.size.z;
        float height = _APFloorBounds.size.y;

        // Display the width, length, and height
        Debug.Log("Width: " + width);
        Debug.Log("Length: " + length);
        Debug.Log("Height: " + height);
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
        foreach (BoxContainer box in boxContainers)
        {
            box.SetValues(level1);
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

    private void CheckSubmittedAnswer(string answer)
    {
        // with contents of _currentboxcontainer, convert to proper answer, and compare with answer.
        Debug.Log($"Checking answer: {answer}");
        string correctAnswer = GetCorrectAnswer(_currentBoxContainer.numericalValue, _currentBoxContainer.unitOfMeasurement);
        Debug.Log($"Desired answer: {correctAnswer}");
        bool isCorrect = answer.Equals(correctAnswer);
        Debug.Log("Result: " + isCorrect);
        // Handle random position for box 
        if (isCorrect)
        {
            _correctAnswer += 1;

            Vector3 center = _APFloorBounds.center;
            Vector3 extents = _APFloorBounds.extents;
            Vector3 randomPosition = _APFloorBounds.center;
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
            }
            _currentBoxContainer.transform.position = randomPosition;
        }
    }

    private string GetCorrectAnswer(int numericalValue, string unitOfMeasurement)
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
    private bool IsAccurate()
    {
        Debug.Log("Determining accuracy!");
		Vector3 center = _APFloorBounds.center;
		Vector3 extents = _APFloorBounds.extents;

        float sum = 0;

		foreach (BoxContainer box in boxContainers)
        {
            Bounds boxBounds = box.GetComponent<Renderer>().bounds;
            float dx = Math.Abs(boxBounds.center.x - center.x);
            float dy = Math.Abs(boxBounds.center.y - center.y);
            float distance = Vector3.Distance(boxBounds.center, center);
            Debug.Log("Distance of a box: " + distance);
            sum += distance;
        }


        Debug.Log("average: " + sum/4);
        Debug.Log("acceptable avg: " + extents/2);
        return false;
    }

    // This method determines precision of boxes, the standard
    // being that the measure of sd is within 1 sd.
    private bool IsPrecise()
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
        sd /= 4;
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
        bool actualAccuracy = IsAccurate();
		bool actualPrecision = IsPrecise();
        if (actualAccuracy == accuracySubmission && actualPrecision == precisionSubmission)
        {
            Debug.Log("AP Answer is correct.");
        } else
        {
            Debug.Log("AP Answer is incorrect.");
        }
		
    }
}
