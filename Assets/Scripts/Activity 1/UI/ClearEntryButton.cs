using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEntryButton : MonoBehaviour
{
    public ComputationDropHandler dropHandler;
    public void OnClick()
    {
        dropHandler.ClearEntry();
    }
}
