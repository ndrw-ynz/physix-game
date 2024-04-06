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
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
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
