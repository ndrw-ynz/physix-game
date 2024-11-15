using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class FirebaseAuthResponse
{
	public string localId;
	public string idToken;
	public string refreshToken;
	public string expiresIn;
}

[System.Serializable]
public class FirestoreDocument
{
	public string name;
	public Dictionary<string, FirestoreField> fields;
	public string createTime;
	public string updateTime;
}

[System.Serializable]
public class FirestoreField
{
#nullable enable
	public string? stringValue;
	public int? integerValue;
	public bool? booleanValue;
	public string? timestampValue;
	public Dictionary<string, object>? mapValue;
	public List<object>? arrayValue; // New array field
#nullable disable

	public FirestoreField(string value)
	{
		stringValue = value;
	}

	public FirestoreField(int value)
	{
		integerValue = value;
	}

	public FirestoreField(bool value)
	{
		booleanValue = value;
	}

	public FirestoreField(DateTime value)
	{
		timestampValue = value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
	}

	public FirestoreField(Dictionary<string, object> map) => mapValue = map;

	public FirestoreField(List<object> array) => arrayValue = array;

	public FirestoreField() { }
}

public class UserManager : MonoBehaviour
{
	public static UserManager Instance { get; private set; }

	private const string FirebaseAuthURL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyBXBcpUw0dNTpqdx6uT4q67qFt45c1_Ds0";
	private const string FirestoreBaseURL = "https://firestore.googleapis.com/v1/projects/physix-9c8bd/databases/(default)/documents/students/";

	public FirebaseAuthResponse CurrentUser { get; private set; }
	public FirestoreDocument UserData { get; private set; }
	public FirestoreDocument UserSection { get; private set; }
	public FirestoreDocument DiscussionOneMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionTwoMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionThreeMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionFourMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionFiveMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionSixMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionSevenMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionEightMarkedPagesData { get; private set; }
    public FirestoreDocument DiscussionNineMarkedPagesData { get; private set; }

    private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject); // Persist between scenes
	}

	// Method to sign in
	public IEnumerator SignInWithEmailAndPassword(string email, string password, System.Action<bool> callback)
	{
		var requestData = new { email, password, returnSecureToken = true };
		string jsonBody = JsonConvert.SerializeObject(requestData);

		UnityWebRequest request = new UnityWebRequest(FirebaseAuthURL, "POST");
		request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			CurrentUser = JsonConvert.DeserializeObject<FirebaseAuthResponse>(request.downloadHandler.text);
			yield return StartCoroutine(GetDocument(CurrentUser.localId, (success) =>
			{
				if (success)
				{
					StartCoroutine(GetSectionDocument(UserData.fields["sectionId"].stringValue, (success) =>
					{
						if (success)
						{
                            Debug.Log("User signed in successfully.");
                            callback(true);
                        }
						else
						{
                            Debug.LogError("Sign-in failed: " + request.downloadHandler.text);
                            callback(false);
							UserSection = null;
                        }
					}));
					
				}
				else
				{
					CurrentUser = null;
				}
			}));
		}
		else
		{
			Debug.LogError("Sign-in failed: " + request.downloadHandler.text);
			callback(false);
		}
	}

	// Method to retrieve user document from Firestore
	public IEnumerator GetDocument(string documentId, System.Action<bool> callback)
	{
		string url = FirestoreBaseURL + documentId;
		UnityWebRequest request = UnityWebRequest.Get(url);
		request.SetRequestHeader("Authorization", "Bearer " + CurrentUser.idToken);

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			UserData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
			Debug.Log("Document retrieved successfully!");
			callback(true);
		}
		else
		{
			Debug.LogError("Failed to retrieve document: " + request.downloadHandler.text);
			callback(false);
		}
	}

	// Method to update user document
	public IEnumerator UpdateDocument(string documentId, Dictionary<string, FirestoreField> updatedFields)
	{
		string url = FirestoreBaseURL + documentId + "?updateMask.fieldPaths=" + string.Join(",", updatedFields.Keys);

		var updateData = new { fields = updatedFields };
		string jsonBody = JsonConvert.SerializeObject(updateData);

		UnityWebRequest request = new UnityWebRequest(url, "PATCH");
		request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		request.SetRequestHeader("Authorization", "Bearer " + CurrentUser.idToken);

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("Document updated successfully!");
		}
		else
		{
			Debug.LogError("Failed to update document: " + request.downloadHandler.text);
		}
	}

	public IEnumerator CreateAttemptDocument(Dictionary<string, FirestoreField> fields, string documentName)
	{
		string url = $"https://firestore.googleapis.com/v1/projects/physix-9c8bd/databases/(default)/documents/{documentName}";

		var requestBody = new { fields = fields };
		string jsonData = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
		}); 
		Debug.Log("BODY:\n: " + jsonData);

		// Set up UnityWebRequest for POST
		UnityWebRequest request = new UnityWebRequest(url, "POST");
		request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
		request.downloadHandler = new DownloadHandlerBuffer();

		// Set headers
		request.SetRequestHeader("Content-Type", "application/json");
		request.SetRequestHeader("Authorization", "Bearer " + CurrentUser.idToken);

		// Send the request and wait for a response
		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("Document created successfully: " + request.downloadHandler.text);
		}
		else
		{
			Debug.LogError("Failed to create document: " + request.downloadHandler.text);
		}
	}


    // Four functions that I added for logging out, main menu and topic discussion //
    // Method to get section name from student's section Id
    public IEnumerator LogoutCurrentUser()
    {
        CurrentUser = null;
        UserData = null;
        UserSection = null;
        DiscussionOneMarkedPagesData = null;
        DiscussionTwoMarkedPagesData = null;
        DiscussionThreeMarkedPagesData = null;
        DiscussionFourMarkedPagesData = null;
        DiscussionFiveMarkedPagesData = null;
        DiscussionSixMarkedPagesData = null;
        DiscussionSevenMarkedPagesData = null;
        DiscussionEightMarkedPagesData = null;
        DiscussionNineMarkedPagesData = null;

		Debug.Log("Logout Successful");
        yield break;
    }

    public IEnumerator GetSectionDocument(string documentId, System.Action<bool> callback)
	{
		string url = $"https://firestore.googleapis.com/v1/projects/physix-9c8bd/databases/(default)/documents/sections/{documentId}";
		UnityWebRequest request = UnityWebRequest.Get(url);
		request.SetRequestHeader("Authorization", "Bearer " + CurrentUser.idToken);

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			UserSection = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
			Debug.Log("Section document retrieved successfully!");
			callback(true);
		}
		else
		{
            Debug.LogError("Failed to retrieve section document: " + request.downloadHandler.text);
            callback(false);
        }
    }

    // Method to get discussion pages progress document of the current user
    public IEnumerator GetDiscussionPagesProgress(int topicDiscussionNumber, string collectionName, string documentId, System.Action<bool> callback)
    {
		string url = $"https://firestore.googleapis.com/v1/projects/physix-9c8bd/databases/(default)/documents/{collectionName}/{documentId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + CurrentUser.idToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
			switch (topicDiscussionNumber)
			{
				case 1:
                    DiscussionOneMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 2:
                    DiscussionTwoMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 3:
                    DiscussionThreeMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 4:
                    DiscussionFourMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 5:
                    DiscussionFiveMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 6:
                    DiscussionSixMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 7:
                    DiscussionSevenMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 8:
                    DiscussionEightMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;

                case 9:
                    DiscussionNineMarkedPagesData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
                    break;
            }
            Debug.Log("Discussion Pages retrieved successfully!");
            callback(true);
        }
        else
        {
            Debug.Log("Failed to retrieve discussion pages: " + request.downloadHandler.text);
            callback(false);
        }
    }

	// Method to update discussion pages progress document of the current user
    public IEnumerator UpdateDiscussionPageProgress(Dictionary<string, FirestoreField> updatedFields, string collectionName, string studentID)
	{
		string url = $"https://firestore.googleapis.com/v1/projects/physix-9c8bd/databases/(default)/documents/{collectionName}/{studentID}";

		var requestBody = new { fields = updatedFields };
        string jsonData = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + CurrentUser.idToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"{collectionName} document updated successfully!");
        }
        else
        {
            Debug.LogError($"Failed to update document {collectionName}: " + request.downloadHandler.text);
        }
    }
}