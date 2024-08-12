using System;
using TMPro;
using UnityEngine;

public class MomentOfInertiaFormulaDisplay : MonoBehaviour
{
    [Header("Number Input Fields")]
    [SerializeField] private TMP_InputField massInputField;
	[SerializeField] private TMP_InputField lengthInputField; // For rods and cylinders
	[SerializeField] private TMP_InputField plateLengthAInputField; // For plates
	[SerializeField] private TMP_InputField plateLengthBInputField; // For plates
	[SerializeField] private TMP_InputField radiusInputField; // For disks, cylinders, spheres
	[SerializeField] private TMP_InputField innerRadiusInputField; // For hollow cylinders
	[SerializeField] private TMP_InputField outerRadiusInputField; // For hollow cylinders
	[Header("Result Field")]
    [SerializeField] private TMP_InputField resultField;
    [Header("Inertia Object Type")]
    public InertiaObjectType inertiaObjectType;

	public float? resultValue { get; private set; }

    public void OnValueChange()
    {
        switch (inertiaObjectType)
        {
            case InertiaObjectType.SlenderRodCenter:
				EvaluateSlenderRodCenterEquation();
                break;
			case InertiaObjectType.SlenderRodEnd:
				EvaluateSlenderRodEndEquation();
				break;
			case InertiaObjectType.RectangularPlateCenter:
				EvaluateRectangularPlateCenterEquation();
				break;
			case InertiaObjectType.RectangularPlateEdge:
				EvaluateRectangularPlateEdgeEquation();
				break;
			case InertiaObjectType.HollowCylinder:
				EvaluateHollowCylinderEquation();
				break;
			case InertiaObjectType.SolidCylinder:
				EvaluateSolidCylinderEquation();
				break;
			case InertiaObjectType.ThinWalledHollowCylinder:
				EvaluateThinWalledHollowCylinderEquation();
				break;
			case InertiaObjectType.SolidSphere:
				EvaluateSolidSphereEquation();
				break;
			case InertiaObjectType.ThinWalledHollowSphere:
				EvaluateThinWalledHollowSphereEquation();
				break;
			case InertiaObjectType.SolidDisk:
				EvaluateSolidDiskEquation();
				break;
		}
    }

	/// <summary>
	/// Evaluates Moment of Inertia Equation for Inertia Object of type <c>SlenderRodCenter</c>.
	/// <br/>
	/// Equation: <c>I = 1/12 ML^2</c>
	/// </summary>
	private void EvaluateSlenderRodCenterEquation()
	{
		// Empty/Null inputs, resulting to N/A
		if (
			string.IsNullOrEmpty(massInputField.text) || 
			string.IsNullOrEmpty(lengthInputField.text)
			)
		{
			resultField.text = "N/A";
			resultValue = null;
			return;
		}

		string equationText = $"1/12 * ({massInputField.text}) * ({lengthInputField.text})^2";

		bool canEvaluate = ExpressionEvaluator.Evaluate(equationText, out float result);
		if (canEvaluate)
		{
			resultValue = (float)Math.Round(result, 4);
			resultField.text = $"{result}";
		}
		else
		{
			resultValue = null;
			resultField.text = "N/A";
		}
	}

	/// <summary>
	/// Evaluates Moment of Inertia Equation for Inertia Object of type <c>SlenderRodEnd</c>.
	/// <br/>
	/// Equation: <c>I = 1/3 ML^2</c>
	/// </summary>
	private void EvaluateSlenderRodEndEquation()
	{
		Debug.Log("change");
		// Empty/Null inputs, resulting to N/A
		if (
			string.IsNullOrEmpty(massInputField.text) ||
			string.IsNullOrEmpty(lengthInputField.text)
			)
		{
			resultField.text = "N/A";
			resultValue = null;
			return;
		}

		string equationText = $"1/3 * ({massInputField.text}) * ({lengthInputField.text})^2";

		bool canEvaluate = ExpressionEvaluator.Evaluate(equationText, out float result);
		if (canEvaluate)
		{
			resultValue = (float)Math.Round(result, 4);
			resultField.text = $"{result}";
		}
		else
		{
			resultValue = null;
			resultField.text = "N/A";
		}
	}

	private void EvaluateRectangularPlateCenterEquation()
	{

	}

	private void EvaluateRectangularPlateEdgeEquation()
	{

	}

	private void EvaluateHollowCylinderEquation()
	{

	}

	private void EvaluateSolidCylinderEquation()
	{

	}

	private void EvaluateThinWalledHollowCylinderEquation()
	{

	}

	private void EvaluateSolidSphereEquation()
	{

	}

	private void EvaluateThinWalledHollowSphereEquation()
	{

	}

	private void EvaluateSolidDiskEquation()
	{

	}
}