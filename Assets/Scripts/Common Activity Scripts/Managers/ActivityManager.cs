using UnityEngine;

public abstract class ActivityManager : MonoBehaviour
{
	[SerializeField] protected InputReader inputReader;
	[SerializeField] protected ActivityPauseMenuUI activityPauseMenuUI;
	[SerializeField] protected MissionObjectiveDisplayUI missionObjectiveDisplayUI;

    [Header("New Level Unlocked Screen")]
    [SerializeField] protected NewLevelUnlockedScreen newLevelUnlockedScreen;

    protected bool hasDisplayedPerformance = false;

	protected virtual void Start()
	{
		inputReader.PauseGameplayEvent += HandleGameplayPause;
		inputReader.PauseGameplayUIEvent += HandleGameplayPause;
		inputReader.ResumeGameplayEvent += HandleGameplayResume;
	}

    private void OnDestroy()
    {
        inputReader.PauseGameplayEvent -= HandleGameplayPause;
        inputReader.PauseGameplayUIEvent -= HandleGameplayPause;
        inputReader.ResumeGameplayEvent -= HandleGameplayResume;
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

	protected abstract void AddAttemptRecord();

	protected abstract void SetNextLevelButtonState();

	public virtual void DisplayPerformanceView()
	{
		if (hasDisplayedPerformance) return;
		hasDisplayedPerformance = true;

		AddAttemptRecord();
		SetNextLevelButtonState();
    }
}