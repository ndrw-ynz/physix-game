using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio")]
public class AudioLibrary : ScriptableObject
{
	public List<AudioClip> backgroundMusicClips;
	public List<AudioClip> sfxClips;

	private Dictionary<string, AudioClip> backgroundMusicLookup;
	private Dictionary<string, AudioClip> sfxLookup;

	private void OnEnable()
	{
		backgroundMusicLookup = new Dictionary<string, AudioClip>();
		foreach (var audio in backgroundMusicClips)
		{
			backgroundMusicLookup[audio.name] = audio;
		}

		sfxLookup = new Dictionary<string, AudioClip>();
		foreach (var audio in sfxClips)
		{
			sfxLookup[audio.name] = audio;
		}
	}

	public AudioClip GetMusic(string musicName)
	{
		backgroundMusicLookup.TryGetValue(musicName, out var clip);
		return clip;
	}

	public AudioClip GetSFX(string sfxName)
	{
		sfxLookup.TryGetValue(sfxName, out var clip);
		return clip;
	}
}
