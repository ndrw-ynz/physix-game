using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitVarianceAnswerButton : MonoBehaviour
{
    public AnswerDropHandler answerDropHandler;
    public GameManager gameManager;
    private Bounds _ejectionAreaOne;
	public void Start()
	{
		_ejectionAreaOne = GameObject.Find("Ejection Area One").GetComponent<Renderer>().bounds;

	}
	public void OnClick()
    {

        double varianceAnswer = Math.Round(CalcVariance(), 4);
        double submittedAnswer = answerDropHandler.answerValue;
        
        Debug.Log("Variance answer: " + varianceAnswer);
        Debug.Log("Submitted answer: " + submittedAnswer);

        bool isApproximatelyCorrect = Mathf.Abs((float) (varianceAnswer - submittedAnswer)) <= 0.0001;
        Debug.Log("Is approximately correct: " + isApproximatelyCorrect);
    }

    private double CalcVariance()
    {
        List<float> distanceValues = gameManager.GetBoxDistanceValues(_ejectionAreaOne, gameManager.ejectionAreaOneBoxContainers);
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
