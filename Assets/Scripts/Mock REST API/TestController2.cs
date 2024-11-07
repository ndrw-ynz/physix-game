using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestController2 : MonoBehaviour
{
    public TextMeshProUGUI emailText; // For displaying any field value
    public TextMeshProUGUI roleText; // Another example text display
    public TextMeshProUGUI dateCreatedText; // Another example text display
    public string documentId = "0Ukey8HtL2jhZchWvXV9";

    void Start()
    {
    }

    public void OnClick()
    {
        StartCoroutine(LoadUserData());
    }

    private IEnumerator LoadUserData()
    {
        yield return FirebaseManager2.Instance.GetUserData(documentId, DisplayData);
    }

    private void DisplayData(Dictionary<string, string> userData)
    {
        if (userData != null)
        {
            emailText.text = $"Email: {userData["email"]}";
            roleText.text = $"Role: {userData["role"]}";
            dateCreatedText.text = $"Date Created: {userData["dateCreated"]}";
        }
        else
        {
            Debug.LogError("Failed to load user data.");
        }
    }
}
