using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSoundManager : MonoBehaviour
{
	public static SceneSoundManager Instance { get; private set; }

	[SerializeField] private AudioListener mainCameraListener;
	[SerializeField] private AudioListener backupListener;
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;
	[SerializeField] private AudioLibrary audioLibrary;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		var camera = Camera.main;
		if (camera != null)
		{
			mainCameraListener = camera.GetComponent<AudioListener>();
		}

		if (mainCameraListener != null)
		{
			backupListener.enabled = !mainCameraListener.enabled;
		}
	}

	private void Update()
	{
		if (mainCameraListener != null)
		{
			backupListener.enabled = !mainCameraListener.isActiveAndEnabled;
		}
	}

	public void PlayMusic(string musicName)
	{
		AudioClip musicClip = audioLibrary.GetMusic(musicName);
		if (musicClip != null)
		{
			musicSource.clip = musicClip;
			musicSource.Play();
			Debug.Log("Music Played: " + musicClip.name);
		}
	}

	public void PlaySFX(string sfxName , float vol = 1)
	{
		AudioClip sfxClip = audioLibrary.GetSFX(sfxName);
		if (sfxClip != null)
		{
			sfxSource.PlayOneShot(sfxClip, vol);
			Debug.Log("SFX Played: " + sfxClip.name);
		}
	}
}
