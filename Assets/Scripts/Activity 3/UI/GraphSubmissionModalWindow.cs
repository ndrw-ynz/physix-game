using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphSubmissionModalWindow : MonoBehaviour
{
	public static event Action InitiateNextSubActivity;
	public static event Action RetryGraphSubmission;
	
    [Header("Images")]
    public Image modalWindowBorderImage;
    public Image positionVsTimeGraphBorderImage;
	public Image velocityVsTimeGraphBorderImage;
	public Image accelerationVsTimeGraphBorderImage;
	[Header("Display Text")]
    public TextMeshProUGUI statusText;
    [Header("Buttons")]
    public Button nextButton;
    public Button tryAgainButton;

    public void SetDisplayFromSubmissionResult(bool isPositionVsTimeGraphCorrect, bool isVelocityVsTimeGraphCorrect, bool isAccelerationVsTimeGraphCorrect)
    {
        bool isAllCorrect = isPositionVsTimeGraphCorrect && isVelocityVsTimeGraphCorrect && isAccelerationVsTimeGraphCorrect;
        
        // Set modal window border image color, status text, and buttons
        if (isAllCorrect)
        {
            modalWindowBorderImage.color = new Color32(175, 255, 155, 255);
            statusText.text = "Graph conversion is correct!";
            nextButton.gameObject.SetActive(true);
			tryAgainButton.gameObject.SetActive(false);
		}
		else
        {
			modalWindowBorderImage.color = new Color32(200, 75, 55, 255);
			statusText.text = "Graph conversion is incorrect!";
			nextButton.gameObject.SetActive(false);
			tryAgainButton.gameObject.SetActive(true);
		}

        // Set graph border images
        positionVsTimeGraphBorderImage.color = isPositionVsTimeGraphCorrect ? new Color32(12, 255, 0, 255) : new Color32(255, 90, 80, 255);
		velocityVsTimeGraphBorderImage.color = isVelocityVsTimeGraphCorrect ? new Color32(12, 255, 0, 255) : new Color32(255, 90, 80, 255);
		accelerationVsTimeGraphBorderImage.color = isAccelerationVsTimeGraphCorrect ? new Color32(12, 255, 0, 255) : new Color32(255, 90, 80, 255);
	}

	private void OnEnable()
	{
		nextButton.onClick.AddListener(() => InitiateNextSubActivity?.Invoke());
		tryAgainButton.onClick.AddListener(() => RetryGraphSubmission?.Invoke());
	}

	private void OnDisable()
	{
		nextButton.onClick.RemoveAllListeners();
		nextButton.onClick.RemoveAllListeners();
	}
}