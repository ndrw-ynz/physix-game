using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    private const string FirestoreApiUrl = "https://firestore.googleapis.com/v1/projects/{0}/databases/(default)/documents";
    private const string ApiKey = "AIzaSyBz6c1RUy8ggDLC46PAKEi9Qp-0FicGOso";
    private const string ProjectId = "practice-509db";  // Firebase project ID

    public static FirebaseManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of FirebaseManager exists
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

    public IEnumerator WriteData(string collectionName, string documentId, Dictionary<string, object> data, Action<bool> onComplete)
    {
        string url = string.Format(FirestoreApiUrl, ProjectId) + "/" + collectionName + "/" + documentId + "?key=" + ApiKey;

        Debug.Log("URL:" + url);

        // Prepare the data in the format Firestore expects
        JObject jsonData = new JObject
        {
            ["fields"] = new JObject()
        };

        // Convert the provided data dictionary into Firestore-compatible fields
        foreach (var entry in data)
        {
            var fieldKey = entry.Key;
            var fieldValue = entry.Value;

            JObject field = new JObject();

            if (fieldValue is string)
            {
                field["stringValue"] = (string)fieldValue;
            }
            else if (fieldValue is int)
            {
                field["integerValue"] = (int)fieldValue;
            }
            else if (fieldValue is bool)
            {
                field["booleanValue"] = (bool)fieldValue;
            }
            else if (fieldValue is double)
            {
                field["doubleValue"] = (double)fieldValue;
            }
            else if (fieldValue is DateTime)
            {
                field["timestampValue"] = ((DateTime)fieldValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else
            {
                // Handle other data types as necessary, possibly storing them as strings or null
                field["stringValue"] = fieldValue.ToString();
            }

            jsonData["fields"][fieldKey] = field;
        }

        // Convert the JObject to a JSON string for the request body
        string jsonString = jsonData.ToString();

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Successfully updated Firestore document.");
            onComplete?.Invoke(true);  // Notify success
        }
        else
        {
            Debug.LogError($"Error writing data to Firestore: {request.error}");
            onComplete?.Invoke(false);  // Notify failure
        }
    }

    public IEnumerator ReadData(string collectionName, string documentId, Action<Dictionary<string, object>> onComplete)
    {
        string url = string.Format(FirestoreApiUrl, ProjectId) + "/" + collectionName + "/" + documentId + "?key=" + ApiKey;

        Debug.Log("URL:" + url);

        UnityWebRequest request = UnityWebRequest.Get(url);

        // Set headers if necessary (e.g., for authentication)
        // request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                // Parse the JSON response using JObject
                JObject jsonResponse = JObject.Parse(request.downloadHandler.text);
                Debug.Log("Firestore Response: " + request.downloadHandler.text);  // Log the entire response for debugging

                // Prepare a dictionary to store the parsed data
                Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

                // Process the JSON object and populate the dictionary
                foreach (var property in jsonResponse["fields"])
                {
                    var key = property.Path.Split('.')[1]; // Get the field name
                    var field = property.First as JObject;

                    if (field != null)
                    {
                        // Check for different field types and extract the raw value
                        // Main code to parse fields and add to `dataDictionary`
                        if (field["stringValue"] != null)
                        {
                            dataDictionary[key] = field["stringValue"].ToString(); // Extract string value
                        }
                        else if (field["integerValue"] != null)
                        {
                            dataDictionary[key] = Convert.ToInt32(field["integerValue"].ToString()); // Extract integer value
                        }
                        else if (field["booleanValue"] != null)
                        {
                            dataDictionary[key] = Convert.ToBoolean(field["booleanValue"].ToString()); // Extract boolean value
                        }
                        else if (field["doubleValue"] != null)
                        {
                            dataDictionary[key] = Convert.ToDouble(field["doubleValue"].ToString()); // Extract double value
                        }
                        else if (field["timestampValue"] != null)
                        {
                            DateTime parsedTimestamp = DateTime.Parse(field["timestampValue"].ToString());
                            dataDictionary[key] = parsedTimestamp;
                        }
                        else if (field["mapValue"] != null)
                        {
                            // Handle map values by converting to a Dictionary<string, object>
                            var mapData = new Dictionary<string, object>();
                            foreach (var mapField in field["mapValue"]["fields"])
                            {
                                string mapKey = mapField.Path.Split('.').Last();
                                JToken mapFieldValue = mapField.First;

                                if (mapFieldValue["stringValue"] != null)
                                {
                                    mapData[mapKey] = mapFieldValue["stringValue"].ToString();
                                }
                                else if (mapFieldValue["integerValue"] != null)
                                {
                                    mapData[mapKey] = Convert.ToInt32(mapFieldValue["integerValue"].ToString());
                                }
                                else if (mapFieldValue["booleanValue"] != null)
                                {
                                    mapData[mapKey] = Convert.ToBoolean(mapFieldValue["booleanValue"].ToString());
                                }
                                else if (mapFieldValue["doubleValue"] != null)
                                {
                                    mapData[mapKey] = Convert.ToDouble(mapFieldValue["doubleValue"].ToString());
                                }
                                else if (mapFieldValue["timestampValue"] != null)
                                {
                                    mapData[mapKey] = DateTime.Parse(mapFieldValue["timestampValue"].ToString());
                                }
                                // Add any other types if necessary
                                else
                                {
                                    mapData[mapKey] = null;
                                }
                            }
                            dataDictionary[key] = mapData;
                        }
                        else if (field["arrayValue"] != null)
                        {
                            // Handle array values by converting to a List<object>
                            var arrayData = new List<object>();
                            foreach (var arrayElement in field["arrayValue"]["values"])
                            {
                                if (arrayElement["stringValue"] != null)
                                {
                                    arrayData.Add(arrayElement["stringValue"].ToString());
                                }
                                else if (arrayElement["integerValue"] != null)
                                {
                                    arrayData.Add(Convert.ToInt32(arrayElement["integerValue"].ToString()));
                                }
                                else if (arrayElement["booleanValue"] != null)
                                {
                                    arrayData.Add(Convert.ToBoolean(arrayElement["booleanValue"].ToString()));
                                }
                                else if (arrayElement["doubleValue"] != null)
                                {
                                    arrayData.Add(Convert.ToDouble(arrayElement["doubleValue"].ToString()));
                                }
                                else if (arrayElement["timestampValue"] != null)
                                {
                                    arrayData.Add(DateTime.Parse(arrayElement["timestampValue"].ToString()));
                                }
                                // Add any other types if necessary
                                else
                                {
                                    arrayData.Add(null);
                                }
                            }
                            dataDictionary[key] = arrayData;
                        }
                        else
                        {
                            // If no recognized type, add a null value
                            dataDictionary[key] = null;
                        }

                    }
                }
                // Invoke the callback with the populated dictionary
                onComplete?.Invoke(dataDictionary);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error processing JSON response: {e.Message}");
            }
        }
        else
        {
            Debug.LogError($"Error fetching data: {request.error}");
        }
    }

    public IEnumerator UpdateData(string collectionName, string documentId, string keyToUpdate, object newValue, Action<bool> onComplete)
    {
        // Construct the Firestore URL to target the specific document, adding `updateMask` to preserve other fields
        string url = string.Format(FirestoreApiUrl, ProjectId) + "/" + collectionName + "/" + documentId + "?key=" + ApiKey + "&updateMask.fieldPaths=" + keyToUpdate;
        Debug.Log("URL: " + url);

        // Prepare the JSON data for the specific field to update
        JObject jsonData = new JObject
        {
            ["fields"] = new JObject
            {
                [keyToUpdate] = new JObject()
            }
        };

        // Set the new value for `keyToUpdate` in the appropriate Firestore format
        if (newValue is string)
        {
            jsonData["fields"][keyToUpdate]["stringValue"] = (string)newValue;
        }
        else if (newValue is int)
        {
            jsonData["fields"][keyToUpdate]["integerValue"] = newValue.ToString();
        }
        else if (newValue is bool)
        {
            jsonData["fields"][keyToUpdate]["booleanValue"] = newValue.ToString().ToLower();
        }
        else if (newValue is double)
        {
            jsonData["fields"][keyToUpdate]["doubleValue"] = newValue.ToString();
        }
        else if (newValue is DateTime)
        {
            jsonData["fields"][keyToUpdate]["timestampValue"] = ((DateTime)newValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        else if (newValue is Dictionary<string, object> mapValue)
        {
            // Handle Map by converting the Dictionary into a nested JSON structure
            JObject mapObject = new JObject();
            foreach (var entry in mapValue)
            {
                JObject field = new JObject();

                if (entry.Value is string)
                {
                    field["stringValue"] = (string)entry.Value;
                }
                else if (entry.Value is int)
                {
                    field["integerValue"] = entry.Value.ToString();
                }
                else if (entry.Value is bool)
                {
                    field["booleanValue"] = entry.Value.ToString().ToLower();
                }
                else if (entry.Value is double)
                {
                    field["doubleValue"] = entry.Value.ToString();
                }
                else if (entry.Value is DateTime)
                {
                    field["timestampValue"] = ((DateTime)entry.Value).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    Debug.LogWarning("Unsupported map field value type.");
                    onComplete?.Invoke(false);
                    yield break;
                }

                mapObject[entry.Key] = field;
            }

            jsonData["fields"][keyToUpdate]["mapValue"] = new JObject { ["fields"] = mapObject };
        }
        else if (newValue is List<object> arrayValue)
        {
            // Handle Array by converting the list into Firestore-compatible JSON array format
            JArray arrayObject = new JArray();
            foreach (var item in arrayValue)
            {
                JObject arrayItem = new JObject();

                if (item is string)
                {
                    arrayItem["stringValue"] = (string)item;
                }
                else if (item is int)
                {
                    arrayItem["integerValue"] = item.ToString();
                }
                else if (item is bool)
                {
                    arrayItem["booleanValue"] = item.ToString().ToLower();
                }
                else if (item is double)
                {
                    arrayItem["doubleValue"] = item.ToString();
                }
                else if (item is DateTime)
                {
                    arrayItem["timestampValue"] = ((DateTime)item).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    Debug.LogWarning("Unsupported array item type.");
                    onComplete?.Invoke(false);
                    yield break;
                }

                arrayObject.Add(arrayItem);
            }

            jsonData["fields"][keyToUpdate]["arrayValue"] = new JObject { ["values"] = arrayObject };
        }
        else
        {
            Debug.LogWarning("Unsupported value type for update.");
            onComplete?.Invoke(false);
            yield break;
        }


        // Convert JSON to string for request
        string jsonString = jsonData.ToString();

        // Setup PATCH request to update only the specified field
        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Field updated successfully in Firestore.");
            onComplete?.Invoke(true);  // Success
        }
        else
        {
            Debug.LogError($"Failed to update field in Firestore: {request.error}");
            onComplete?.Invoke(false);  // Failure
        }
    }


}
