using System;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSelectionHandler : MonoBehaviour
{
	public event Action<BoxContainer> UpdateSelectedContainerEvent;

	[SerializeField] private Camera containerAreaCamera;

	[SerializeField] private List<BoxContainer> containerList;
	
	private Transform highlightedContainer;
	private Transform selectedContainer;
	private RaycastHit raycastHit;

	void Update()
	{
		// White - available
		// Yellow - hovering
		// Green - selected

		if (highlightedContainer != null && highlightedContainer != selectedContainer)
		{
			highlightedContainer.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
			highlightedContainer = null;
		}
		
		Ray ray = containerAreaCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out raycastHit))
		{
			highlightedContainer = raycastHit.transform;

			if (highlightedContainer.CompareTag("Selectable")) //&& highlight != selection
			{
				if (highlightedContainer != selectedContainer)
				highlightedContainer.gameObject.GetComponent<Outline>().OutlineColor = Color.yellow;
			}
			else
			{
				highlightedContainer = null;
			}

		}

		// Select/Deselect container
		if (Input.GetMouseButtonDown(0))
		{
			// Hovering on a highlighted object
			if (highlightedContainer)
			{
				// If there is already a selected object, reset it to white.
				if (selectedContainer != null)
				{
					selectedContainer.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
				}

				if (highlightedContainer == selectedContainer)
				{
					// If the selected object is already selected, deselect object.
					selectedContainer.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
					selectedContainer = null;
				} else
				{
					// If the object is not selected, make it the selected object.
					selectedContainer = raycastHit.transform;
					selectedContainer.gameObject.GetComponent<Outline>().OutlineColor = Color.green;
				}

				UpdateSelectedContainerEvent?.Invoke(selectedContainer != null ? selectedContainer.gameObject.GetComponent<BoxContainer>() : null);

				highlightedContainer = null;
			}
		}
	}

	public void SetupContainerValues(ScientificNotationSubActivitySO scientificNotationSO)
	{
		foreach (BoxContainer box in containerList)
		{
			box.SetValues(scientificNotationSO);
		}
	}

	public BoxContainer GetSelectedContainer()
	{
		if (selectedContainer != null)
		{
			return selectedContainer.gameObject.GetComponent<BoxContainer>();
		}
		return null;
	}
}
