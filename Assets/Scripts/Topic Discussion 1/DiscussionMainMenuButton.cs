using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscussionMainMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
