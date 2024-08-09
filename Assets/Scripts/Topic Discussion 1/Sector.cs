using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sector : MonoBehaviour
{
    [Header("Sector Properties")]
    public string sectorTitle;
    public List<Page> pages;
}