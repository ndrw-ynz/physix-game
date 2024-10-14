using UnityEngine;

public class APGraphManager : MonoBehaviour
{
    [Header("Accuracy Precision Graphs")]
    [SerializeField] private APGraph graphOne;
    [SerializeField] private APGraph graphTwo;
    [SerializeField] private APGraph graphThree;

	private void Start()
	{
		graphOne.InitializeGraphContent();
		graphTwo.InitializeGraphContent();
		graphThree.InitializeGraphContent();
	}
}
