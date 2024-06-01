using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Errors : IInteractable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewErrorsUI;
	[SerializeField] private ActivityOneManager activityOneManager;
	public override void Interact()
    {
        if (activityOneManager.isVarianceFinished == true && activityOneManager.isErrorsFinished == false)
        {
            _inputReader.SetUI();
            viewErrorsUI.SetActive(true);
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
