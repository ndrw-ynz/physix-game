using UnityEngine;

[System.Serializable]
public class Page : MonoBehaviour
{
    [Header("Page Properties")]
    public GameObject page;
    public bool isMarkedRead;
    public CanvasGroup canvasGroup;
}