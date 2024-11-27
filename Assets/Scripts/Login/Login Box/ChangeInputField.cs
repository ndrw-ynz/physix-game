using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeInputField : MonoBehaviour
{
    // Event system for tab, tab+lshift operations
    EventSystem eventSystem;

    [Header("First Input Field")]
    [SerializeField] Selectable firstInput;
    void Start()
    {
        // Assign event system and select the first input field to be typed on
        eventSystem = EventSystem.current;
        firstInput.Select();
    }

    void Update()
    {
        // Ensure there's a currently selected GameObject
        if (eventSystem.currentSelectedGameObject == null)
        {
            firstInput.Select(); // Select the first input if nothing is selected
            return;
        }

        // Get the current selectable
        Selectable currentSelectable = eventSystem.currentSelectedGameObject.GetComponent<Selectable>();

        if (currentSelectable == null)
        {
            return; // Skip if the selected object is not a Selectable
        }

        // Switch between input fields using Tab or Tab + LeftShift
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable previous = currentSelectable.FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = currentSelectable.FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }

}
