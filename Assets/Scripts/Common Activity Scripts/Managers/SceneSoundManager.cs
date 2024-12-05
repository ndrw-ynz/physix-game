using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneSoundManager : MonoBehaviour
{
	public static SceneSoundManager Instance { get; private set; }

	[SerializeField] private AudioListener mainCameraListener;
	[SerializeField] private AudioListener backupListener;
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;
	[SerializeField] private AudioLibrary audioLibrary;
	[SerializeField] private AudioMixer audioMixer;
    private void Start()
    {
        // Load and apply the music volume
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        }
        else
        {
            audioMixer.SetFloat("Music", 0); // Default value
        }

        // Load and apply the SFX volume
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        }
        else
        {
            audioMixer.SetFloat("SFX", 0); // Default value
        }
    }

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
		if (musicClip != null && (musicClip != musicSource.clip || !musicSource.isPlaying))
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
