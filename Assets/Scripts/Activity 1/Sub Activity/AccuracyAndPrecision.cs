using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyAndPrecision : IInteractable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewAccuracyAndPrecisionUI;
    [SerializeField] private ActivityOneManager activityOneManager;
    public override void Interact()
    {
        if (activityOneManager.isScientificNotationFinished == true && activityOneManager.isAccuracyAndPrecisionFinished == false)
        {
            _inputReader.SetUI();
            viewAccuracyAndPrecisionUI.SetActive(true);
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
