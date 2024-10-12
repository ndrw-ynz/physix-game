using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VarianceView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Given Numerical Value Display Container")]
	[SerializeField] private VerticalLayoutGroup numericalValueContainers;

	[Header("Prefabs")]
	[SerializeField] private GivenVariableDisplay givenVariableDisplayPrefab;

	public void SetupVarianceView(List<BoxContainer> boxContainerList)
	{
		for (int i = 0; i < boxContainerList.Count; i++)
		{
			GivenVariableDisplay numericalValueDisplay = Instantiate(givenVariableDisplayPrefab, numericalValueContainers.transform, false);
			numericalValueDisplay.SetupGivenVariableDisplay($"Container No. {i+1} : ", $"{boxContainerList[i].numericalValue}");
		}
	}

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}