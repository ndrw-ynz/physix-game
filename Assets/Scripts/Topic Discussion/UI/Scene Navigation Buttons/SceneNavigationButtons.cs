using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneNavigationButtons : MonoBehaviour, IPointerEnterHandler
{
    [Header("Scene Navigation Buttons")]
    [SerializeField] private DiscussionMainMenuButton mainMenuButton;
    [SerializeField] private DiscussionActivityButton startActivityButton;
    [SerializeField] private DiscussionBackToActivityButton backToActivityButton;

    public void ActivateBackToGameButtonOnly()
    {
        //Deactivate main menu and start activity button
        //Then activate back to game button
        mainMenuButton.gameObject.SetActive(false);
        startActivityButton.gameObject.SetActive(false);
        backToActivityButton.gameObject.SetActive(true);
    }

    public void LoadMainMenu()
    {
		SceneSoundManager.Instance.PlaySFX("Click_2");
		// Load main menu scene
		SceneManager.LoadScene($"Main Menu");
    }

    public void LoadActivity(int activityNumber)
    {
		SceneSoundManager.Instance.PlaySFX("Click_2");
		// Load specified activity scene
		SceneManager.LoadScene($"Activity {activityNumber}");
    }

    public void CloseCurrentDiscussion(int activeDiscussionScene)
    {
		SceneSoundManager.Instance.PlaySFX("Click_2");
        // Commented as an altenative way to close current scene
        //Scene activeScene = SceneManager.GetActiveScene();
        //SceneManager.UnloadSceneAsync(activeScene.buildIndex);
        // Close current topic discussion scene asynchronously
        SceneManager.UnloadSceneAsync($"Topic Discussion {activeDiscussionScene}");
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}
