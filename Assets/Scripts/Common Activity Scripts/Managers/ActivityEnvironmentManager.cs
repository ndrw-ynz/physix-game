using UnityEngine;

public abstract class ActivityEnvironmentManager : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] private InputReader inputReader;


	[Header("Player")]
	[SerializeField] private GameObject player;

	public void SetPlayerActivityState(bool isActive)
	{
		player.gameObject.SetActive(isActive);
		if (isActive)
		{
			inputReader.SetGameplay();
		} else
		{
			inputReader.SetUI();
		}
	}
}