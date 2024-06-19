using TMPro;
using UnityEngine;

public class VectorComponentDisplay : MonoBehaviour
{
	[Header("Vector Label")]
	public TextMeshProUGUI vectorLabel;
	[Header("Vector Component Fields")]
    public TMP_InputField xComponentField;
	public TMP_InputField yComponentField;
	[Header("Private variables")]
	private Vector2 vectorComponent;

	public void SetupVectorComponentDisplay(VectorInfo vectorInfo)
	{
		vectorLabel.text = $"{vectorInfo.magnitudeValue}m {vectorInfo.directionValue}°";
		vectorComponent = vectorInfo.vectorComponent;
		xComponentField.text = vectorComponent.x.ToString();
		yComponentField.text = vectorComponent.y.ToString();
	}
}
