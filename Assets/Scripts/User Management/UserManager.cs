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
	public string stringValue;
	public string timestampValue;
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
			Debug.Log("Sign-in successful!");
			yield return StartCoroutine(GetDocument(CurrentUser.localId, CurrentUser.idToken));
			callback(true);
		}
		else
		{
			Debug.LogError("Sign-in failed: " + request.downloadHandler.text);
			callback(false);
		}
	}

	// Method to retrieve user document from Firestore
	public IEnumerator GetDocument(string documentId, string idToken)
	{
		string url = FirestoreBaseURL + documentId;
		UnityWebRequest request = UnityWebRequest.Get(url);
		request.SetRequestHeader("Authorization", "Bearer " + idToken);

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			UserData = JsonConvert.DeserializeObject<FirestoreDocument>(request.downloadHandler.text);
			Debug.Log("Document retrieved successfully!");
		}
		else
		{
			Debug.LogError("Failed to retrieve document: " + request.downloadHandler.text);
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
}