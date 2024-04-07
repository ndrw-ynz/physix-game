using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyAndPrecision : MonoBehaviour, IInteractable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewAccuracyAndPrecisionUI;
    public void Interact()
    {
        Debug.Log("Activated accuracy and precision panel!");
        _inputReader.SetUI();
        viewAccuracyAndPrecisionUI.SetActive(true);
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
