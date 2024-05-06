using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEntryButton : MonoBehaviour
{
    public DropHandler dropHandler;
    public void OnClick()
    {
        dropHandler.ClearEntry();
    }
}
