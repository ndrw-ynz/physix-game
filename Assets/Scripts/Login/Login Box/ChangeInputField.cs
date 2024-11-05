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
        // Switch between input fields using tab or tab + lshift
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)){
            Selectable previous = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if(next != null)
            {
                next.Select();
            }
        }
    }
}
