using UnityEngine;

public enum SubActivityType
{
	Quantities,
	CartesianComponents,
	VectorAddition,
	End
}

public class InteractablePanel : IInteractable
{
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private GameObject view;
	[SerializeField] private ActivityTwoManager activityTwoManager;
	public SubActivityType subActivityType;

	public override void Interact()
	{
		switch (subActivityType)
		{
			case SubActivityType.Quantities:
				if (activityTwoManager.isQuantitiesSubActivityFinished == true) return;
				break;
			case SubActivityType.CartesianComponents:
				if (activityTwoManager.isQuantitiesSubActivityFinished == false || activityTwoManager.isComponentsSubActivityFinished == true) return;
				break;
			case SubActivityType.VectorAddition:
				if (activityTwoManager.isComponentsSubActivityFinished == false || activityTwoManager.isVectorAdditionSubActivityFinished == true) return;
				break;
			case SubActivityType.End:
				if (activityTwoManager.isVectorAdditionSubActivityFinished == false) return;
				break;
		}
		_inputReader.SetUI();
		view.gameObject.SetActive(true);
	}
}
