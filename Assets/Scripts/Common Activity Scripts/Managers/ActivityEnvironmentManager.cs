using UnityEngine;

public abstract class ActivityEnvironmentManager : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] private InputReader inputReader;


	[Header("Player")]
	[SerializeField] protected GameObject player;

	public void SetPlayerActivityState(bool isActive)
	{
		player.gameObject.SetActive(isActive);
		if (isActive)
		{
			SceneSoundManager.Instance.PlaySFX("UI_MenufadeOut_Stereo_01");
			inputReader.SetGameplay();
		} else
		{
			SceneSoundManager.Instance.PlaySFX("UI_MenufadeIn_Stereo_01");
			inputReader.SetUI();
		}
	}
}