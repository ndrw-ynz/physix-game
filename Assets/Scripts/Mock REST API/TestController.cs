using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

public class TestController : MonoBehaviour
{
    public TextMeshProUGUI emailText; // For displaying any field value
    public TextMeshProUGUI roleText; // Another example text display
    public TextMeshProUGUI dateCreatedText; // Another example text display

    public Button button;

    private void Start()
    {
        // Example: Read data from Firestore (use your collection and document ID)
        //StartCoroutine(FirebaseManager.Instance.ReadData("players", "player1", OnDataReceived));
    }

    public void OnClick()
    {
        // WRITING OF DATA
        //Dictionary<string, object> dataToWrite = new Dictionary<string, object>
        //    {
        //        { "name", "John Doe" },
        //        { "age", 30 },
        //        { "isActive", true },
        //        { "lastLogin", DateTime.Now } // This will be stored as a timestamp
        //    };
        //
        //string randomDocumentId = GenerateRandomDocumentId();
        //StartCoroutine(FirebaseManager.Instance.WriteData("users", randomDocumentId, dataToWrite, (success) =>
        //{
        //    if (success)
        //    {
        //        Debug.Log("Data successfully written to Firestore.");
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to write data to Firestore.");
        //    }
        //}));

        // READING OF DATA
        StartCoroutine(FirebaseManager.Instance.ReadData("users", "uZwldeKjhKyKx89WEpIZ", OnDataReceived));

        // UPDATING OF DATA
        //DateTime date = DateTime.Now;
        //int value = 30302402;
        //
        //List<object> list = new List<object>() {3, "ho"};
        //
        //StartCoroutine(FirebaseManager.Instance.UpdateData("users", "uZwldeKjhKyKx89WEpIZ", "email", list, OnUpdateComplete));
    }

    private string GenerateRandomDocumentId(int length = 20)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        System.Random random = new System.Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }

    void OnUpdateComplete(bool success)
    {
        if (success)
        {
            Debug.Log("Data update was successful!");
        }
        else
        {
            Debug.LogError("Data update failed.");
        }
    }

    // Callback function to handle the data received
    private void OnDataReceived(Dictionary<string, object> data)
    {
        Debug.Log("OnDataReceived called");
        Debug.Log(data);

        Debug.Log(data.Count);
        // Example: Display all the fields in the dictionary
        foreach (var entry in data)
        {
            Debug.Log($"Field: {entry.Key}, Value: {entry.Value}");
            
            // You can check for specific fields or just display them
            if (entry.Key == "email")
            {
                // Assuming entry.Value is a string or something that can be converted to string
                emailText.text = "Email: " + entry.Value.ToString();
            }
            else if (entry.Key == "role")
            {
                // Assuming entry.Value is a string or something that can be converted to string
                roleText.text = "Role: " + entry.Value.ToString();
            }
            else if (entry.Key == "dateCreated")
            {
                // If entry.Value is a DateTime, convert it to a readable format
                if (entry.Value is DateTime dateCreated)
                {
                    dateCreatedText.text = "Date Created: " + dateCreated.ToString("MMMM/dd/yyyy");
                }
                else
                {
                    dateCreatedText.text = "Date Created: " + entry.Value.ToString();
                }
            }
        }
    }
}
