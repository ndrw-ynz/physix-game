using UnityEngine;

public class ScreenNavigationButton : MonoBehaviour
{
    public GameObject screenToActivate;
    public GameObject screenToDeactivate;

    public void OnClick()
    {
        screenToActivate.gameObject.SetActive(true);
        screenToDeactivate.gameObject.SetActive(false);
    }
}
