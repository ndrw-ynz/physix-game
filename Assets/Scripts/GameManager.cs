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

            return $"{formattedCoefficient} × 10^{exponent}";
        }
        else
        {
            Debug.LogError("Unit of measurement not found!");
            return null;
        }
    }
}
