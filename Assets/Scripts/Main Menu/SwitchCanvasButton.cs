using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCanvasButton : MonoBehaviour
{
    public Canvas canvasToActivate;
    public Canvas canvasToDeactivate;

    public void OnClick()
    {
        canvasToActivate.gameObject.SetActive(true);
        canvasToDeactivate.gameObject.SetActive(false);
    }
}
