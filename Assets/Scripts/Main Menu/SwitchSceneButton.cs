using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour
{
    public string sceneName;
    public void OnClick()
    {
        if (!sceneName.IsUnityNull())
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
