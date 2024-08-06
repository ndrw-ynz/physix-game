using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscussionActivityButton : MonoBehaviour
{
    public int activityNumber;

    public void OnClick()
    {
        SceneManager.LoadScene($"Activity {activityNumber}");
    }
}
