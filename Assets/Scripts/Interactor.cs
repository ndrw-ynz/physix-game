using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    public Transform interactorSource;
    public float interactRange = 3.0f;

    void Start()
    {
        _input.InteractEvent += HandleInteract;
    }

    void Update()
    {
        Interact();
    }

    private void HandleInteract()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray r = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(r, out RaycastHit hit, interactRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }

    private void Interact()
    {

    }
}
