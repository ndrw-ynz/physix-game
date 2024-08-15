using TMPro;
using UnityEngine;

/// <summary>
/// This class contains the display for the given variables
/// for a bolt to compute the torque. This contains the force,
/// distance vector, and direction of the bolt's torque.
/// </summary>
public class BoltInfoDisplay : MonoBehaviour
{
	[Header("Header Text")]
	[SerializeField] private TextMeshProUGUI boltDescriptonText;
    [Header("Given Variable Displays")]
    [SerializeField] private GivenVariableDisplay forceDisplay;
	[SerializeField] private GivenVariableDisplay distanceVectorDisplay;
	[SerializeField] private GivenVariableDisplay directionDisplay;

	/// <summary>
	/// Sets up the display of variables for <c>BoltInfoDisplay</c>
	/// based from <c>TorqueData</c>.
	/// </summary>
	/// <param name="data"></param>
	public void SetupBoltInfoDisplay(TorqueData data, int boltNumber)
	{
		boltDescriptonText.text = $"Bolt No. {boltNumber}:";

		forceDisplay.SetupGivenVariableDisplay("Force: ", $"{data.force}");
		distanceVectorDisplay.SetupGivenVariableDisplay("Distance Vector: ", $"{data.distanceVector}");
		string direction = data.torqueDirection == TorqueDirection.Upward ? "Loosen" : "Tighten";
		directionDisplay.SetupGivenVariableDisplay("Direction: ", direction);
	}
}