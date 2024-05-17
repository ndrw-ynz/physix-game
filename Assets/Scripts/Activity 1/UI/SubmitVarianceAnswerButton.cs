using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitVarianceAnswerButton : MonoBehaviour
{
    public AnswerDropHandler answerDropHandler;
    public GameManager gameManager;
    public void OnClick()
    {

        double varianceAnswer = Math.Round(CalcVariance(), 4);
        double submittedAnswer = answerDropHandler.answerValue;
        
        Debug.Log("Variance answer: " + varianceAnswer);
        Debug.Log("Submitted answer: " + submittedAnswer);

        bool isApproximatelyCorrect = Mathf.Approximately((float)varianceAnswer, (float)submittedAnswer);
        Debug.Log("Is approximately correct: " + isApproximatelyCorrect);
    }

    private double CalcVariance()
    {
        List<float> distanceValues = gameManager.GetBoxDistanceValues();
        // Calculate average of values
        double avg = 0;
        foreach (float d in distanceValues)
        {
            avg += d;
        }
        avg = Math.Round(avg/4, 4);
        // Calculate variance
        double variance = 0;
        foreach (float d in distanceValues)
        {
            variance += Math.Pow(d-avg, 2);
        }
        variance = Math.Round(variance/3, 4);

        return variance;
    }
}