using UnityEngine;
using UnityEngine.UI;

public class CenterOfMassView : MonoBehaviour
{
	[Header("Calculation Displays")]
	[SerializeField] GameObject MassTimesXCoords;
	[SerializeField] GameObject MassTimesYCoords;
	[SerializeField] GameObject CenterOfMassXDir;
	[SerializeField] GameObject CenterOfMassYDir;
	[Header("Interactive Buttons")]
    [SerializeField] Button leftPageButton;
	[SerializeField] Button rightPageButton;

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		MassTimesXCoords.gameObject.SetActive(true);
		CenterOfMassXDir.gameObject.SetActive(true);

		MassTimesYCoords.gameObject.SetActive(false);
		CenterOfMassYDir.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		MassTimesXCoords.gameObject.SetActive(false);
		CenterOfMassXDir.gameObject.SetActive(false);

		MassTimesYCoords.gameObject.SetActive(true);
		CenterOfMassYDir.gameObject.SetActive(true);
	}
	#endregion
}