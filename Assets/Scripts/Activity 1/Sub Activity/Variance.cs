using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variance : IInteractable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewVarianceUI;
	[SerializeField] private ActivityOneManager activityOneManager;

	public override void Interact()
    {
        if (activityOneManager.isAccuracyAndPrecisionFinished == true && activityOneManager.isVarianceFinished == false)
        {
            _inputReader.SetUI();
            viewVarianceUI.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
