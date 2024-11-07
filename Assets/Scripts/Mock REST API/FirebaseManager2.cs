using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class FirebaseManager2 : MonoBehaviour
{
    [Header("Firebase Configuration")]
    private string projectId = "practice-509db";
    private string apiKey = "AIzaSyBz6c1RUy8ggDLC46PAKEi9Qp-0FicGOso";
    private string collectionName = "users";

    private string baseUrl;

    public static FirebaseManager2 Instance { get; private set; }

    void Awake()
    {
        baseUrl = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/{collectionName}";

        // Singleton pattern to ensure only one instance of FirebaseManager2 exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Public IEnumerator to fetch data, so it can be called from other scripts
    public IEnumerator GetUserData(string documentId, System.Action<Dictionary<string, string>> onUserDataFetched)
    {
        string url = $"{baseUrl}/{documentId}?key={apiKey}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Dictionary<string, string> userData = ParseUserData(request.downloadHandler.text);
                onUserDataFetched?.Invoke(userData);
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
                onUserDataFetched?.Invoke(null);
            }
        }
    }

    // Parse JSON data into a dictionary for display
    private Dictionary<string, string> ParseUserData(string jsonData)
    {
        Dictionary<string, string> userData = new Dictionary<string, string>();

        JObject json = JObject.Parse(jsonData);
        JToken fields = json["fields"];

        if (fields != null)
        {
            userData["email"] = fields["email"]?["stringValue"]?.ToString();
            userData["role"] = fields["role"]?["stringValue"]?.ToString();
            userData["dateCreated"] = fields["dateCreated"]?["timestampValue"]?.ToString();
        }

        return userData;
    }
}
