using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileMotionAnswerSubmission {

	public float? maximumHeight { get; private set; }
	public float? horizontalRange { get; private set; }
	public float? timeOfFlight { get; private set; }

	public ProjectileMotionAnswerSubmission(
		float? maximumHeight,
		float? horizontalRange,
		float? timeOfFlight
		)
	{
		this.maximumHeight = maximumHeight;
		this.horizontalRange = horizontalRange;
		this.timeOfFlight = timeOfFlight;
	}
}

public class ProjectileMotionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<ProjectileMotionAnswerSubmission> SubmitAnswerEvent;

	[Header("Text Displays")]
	[SerializeField] private TextMeshProUGUI testCountText;

	[Header("Given Value Fields")]
	[SerializeField] private TMP_InputField givenInitialVelocity;
	[SerializeField] private TMP_InputField givenInitialHeight;
	[SerializeField] private TMP_InputField givenAngleMeasure;

	[Header("Formula Displays")]
	[SerializeField] private MaximumHeightFormulaDisplay maximumHeightFormulaDisplay;
	[SerializeField] private HorizontalRangeFormulaDisplay horizontalRangeFormulaDisplay;
	[SerializeField] private TimeOfFlightFormulaDisplay timeOfFlightFormulaDisplay;

	[Header("Calculation Page List")]
	[SerializeField] private List<GameObject> calculationPages;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	private int currentPageIndex;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void UpdateTestCountTextDisplay(int currentNumTests, int totalNumTests)
	{
		testCountText.text = $"<color=yellow>Number of Tests Solved: {currentNumTests} / {totalNumTests}</color>";
	}

	public void SetupProjectileMotionView(ProjectileMotionCalculationData data)
	{
		// Reset display state
		ResetViewDisplayState();

		// Update given value text fields
		givenInitialVelocity.text = $"{data.initialVelocity} m/s";
		givenInitialHeight.text = $"{data.initialHeight} m";
		givenAngleMeasure.text = $"{data.angleMeasure} �";

		// Reset state of formula displays
		maximumHeightFormulaDisplay.ResetState();
		horizontalRangeFormulaDisplay.ResetState();
		timeOfFlightFormulaDisplay.ResetState();
	}

	private void ResetViewDisplayState()
	{
		calculationPages[0].gameObject.SetActive(true);
		for (int i = 1; i < calculationPages.Count; i++)
		{
			calculationPages[i].gameObject.SetActive(false);
		}
		currentPageIndex = 0;

		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);
	}

	public void OnLeftPageButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		calculationPages[currentPageIndex].gameObject.SetActive(false);
		currentPageIndex--;
		calculationPages[currentPageIndex].gameObject.SetActive(true);

		if (currentPageIndex <= 0)
		{
			leftPageButton.gameObject.SetActive(false);
		}
		rightPageButton.gameObject.SetActive(true);
	}

	public void OnRightPageButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		calculationPages[currentPageIndex].gameObject.SetActive(false);
		currentPageIndex++;
		calculationPages[currentPageIndex].gameObject.SetActive(true);

		if (currentPageIndex >= calculationPages.Count - 1)
		{
			rightPageButton.gameObject.SetActive(false);
		}
		leftPageButton.gameObject.SetActive(true);
	}

	public void OnSubmitButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		ProjectileMotionAnswerSubmission submission = new ProjectileMotionAnswerSubmission(
			maximumHeight: maximumHeightFormulaDisplay.resultValue,
			horizontalRange: horizontalRangeFormulaDisplay.resultValue,
			timeOfFlight: timeOfFlightFormulaDisplay.resultValue
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
