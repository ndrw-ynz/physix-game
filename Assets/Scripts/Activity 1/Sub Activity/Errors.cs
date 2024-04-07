using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Errors : MonoBehaviour, IInteractable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewErrorsUI;
    public void Interact()
    {
        Debug.Log("Activated errors panel!");
        _inputReader.SetUI();
        viewErrorsUI.SetActive(true);
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
