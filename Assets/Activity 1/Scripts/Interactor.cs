using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightMaterial;
    public abstract void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    public Transform interactorSource;
    public float interactRange = 3.0f;

    private IInteractable _selectedInteractable;

    void Start()
    {
        _input.InteractEvent += HandleInteract;
    }

    void Update()
    {
        HandleHighlight();
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

    private void HandleHighlight()
    {
        if (_selectedInteractable != null)
        {
            _selectedInteractable.GetComponent<Renderer>().material = _selectedInteractable.defaultMaterial;
            _selectedInteractable = null;
        }

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray r = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(r, out RaycastHit hit, interactRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.GetComponent<Renderer>().material = interactObj.highlightMaterial;
                _selectedInteractable = interactObj;
            } 
        }
    }
}
