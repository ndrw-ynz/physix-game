using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variance : MonoBehaviour, IInteractable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewVarianceUI;
    public void Interact()
    {
        Debug.Log("Activated variance panel!");
        _inputReader.SetUI();
        viewVarianceUI.SetActive(true);
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
