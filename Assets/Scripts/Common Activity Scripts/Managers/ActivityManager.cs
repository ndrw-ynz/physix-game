using UnityEngine;

public abstract class ActivityManager : MonoBehaviour
{
	[SerializeField] protected InputReader inputReader;
	[SerializeField] protected ActivityPauseMenuUI activityPauseMenuUI;
	[SerializeField] protected MissionObjectiveDisplayUI missionObjectiveDisplayUI;

	protected virtual void Start()
	{
		inputReader.PauseGameplayEvent += HandleGameplayPause;
		inputReader.PauseGameplayUIEvent += HandleGameplayPause;
		inputReader.ResumeGameplayEvent += HandleGameplayResume;
	}

	protected virtual void HandleGameplayPause()
	{
		activityPauseMenuUI.gameObject.SetActive(true);
	}

	protected virtual void HandleGameplayResume()
	{
		activityPauseMenuUI.gameObject.SetActive(false);
	}

	public void SetMissionObjectiveDisplay(bool isActive)
	{
		missionObjectiveDisplayUI.gameObject.SetActive(isActive);
	}

	public abstract void DisplayPerformanceView();
}