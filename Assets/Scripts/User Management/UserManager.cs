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

	public FirestoreField() { }
}

public class UserManager : MonoBehaviour
{
	public static UserManager Instance { get; private set; }

	private const string FirebaseAuthURL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyBXBcpUw0dNTpqdx6uT4q67qFt45c1_Ds0";
	private const string FirestoreBaseURL = "https://firestore.googleapis.com/v1/projects/physix-9c8bd/databases/(default)/documents/students/";

	public FirebaseAuthResponse CurrentUser { get; private set; }
	public FirestoreDocument UserData { get; private set; }

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
					Debug.Log("User signed in successfully.");
					callback(true);
				}
				else
				{
					Debug.LogError("Sign-in failed: " + request.downloadHandler.text);
					CurrentUser = null;
					callback(false);
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
}