using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private ScientificNotationSO level1;
    [SerializeField] private List<BoxContainer> boxContainers;
    private BoxContainer _currentBoxContainer;

    void Start()
    {
        _input.PauseEvent += HandlePause;
        _input.ResumeEvent += HandleResume;
        BoxContainer.BoxInteractEvent += OnSelectBox;
        ViewScientificNotation.OpenViewEvent += OnOpenViewSN;
        ViewScientificNotation.SubmitAnswerEvent += CheckSubmittedAnswer;

        // TODO: handle event for randomizing contents of box containers
        RandomlyGenerateBoxValues();
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
        foreach (BoxContainer box in boxContainers) {
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

    private void CheckSubmittedAnswer(string answer)
    {
        // with contents of _currentboxcontainer, convert to proper answer, and compare with answer.
        Debug.Log($"Checking answer: {answer}!");
        string correctAnswer = GetCorrectAnswer(_currentBoxContainer.numericalValue, _currentBoxContainer.unitOfMeasurement);
        Debug.Log($"Desired answer: {correctAnswer}");
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

        // convert unit of measurement to number of zeroes.
        if (unitPowers.TryGetValue(unitOfMeasurement, out int power))
        {
            // Multiply the numerical value by 10 raised to the power associated with the unit
            double result = numericalValue * Mathf.Pow(10, power);
            // Convert the number to absolute value for manipulation
            double absoluteValue = Math.Abs(result);

            // Calculate the exponent
            int exponent = (int)Math.Floor(Math.Log10(absoluteValue));

            // Extract the leading digit (nonzero)
            double leadingDigit = absoluteValue / Math.Pow(10, exponent - 1);

            // Format the value in scientific notation
            string formattedValue = string.Format("{0:F2}x10^{1}", absoluteValue / leadingDigit, exponent);

            return formattedValue;
        } else
        {
            Debug.LogError("Unit of measurement not found!");
            return null;
        }
    }
}
