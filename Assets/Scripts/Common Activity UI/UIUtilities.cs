using UnityEngine;

public static class UIUtilities
{
	public static void CenterChildInParent(GameObject child, GameObject parent)
	{
		RectTransform childRectTransform = child.GetComponent<RectTransform>();
		RectTransform parentRectTransform = parent.GetComponent<RectTransform>();

		if (childRectTransform != null && parentRectTransform != null)
		{
			// Set the child as a child of the parent
			childRectTransform.SetParent(parentRectTransform);

			// Set the pivot of the child to the center
			childRectTransform.pivot = new Vector2(0.5f, 0.5f);

			// Set anchors to center
			childRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			childRectTransform.anchorMax = new Vector2(0.5f, 0.5f);

			// Set the child's anchored position to the center of the parent
			childRectTransform.anchoredPosition = Vector2.zero;
		}
		else
		{
			Debug.LogWarning("One or both of the provided GameObjects do not have a RectTransform component.");
		}
	}
}