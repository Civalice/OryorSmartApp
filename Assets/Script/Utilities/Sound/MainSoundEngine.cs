using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SoundObject
{
	public string key;
	public AudioClip clip;
}

[System.Serializable]
public class SoundObjectList
{
	public AudioClip this [string key]
	{
		get
		{
			if (key == null)
			{
				Debug.LogWarning("NullIndexException");
				return null;
			}
			for(int i = 0;i < keyList.Count;i++)
			{
				if (keyList[i].key == key)
				{
					return keyList[i].clip;
				}
			}
			Debug.LogWarning("NullKeyException");
			return null;
		}
	}
	[SerializeField]
	public List<SoundObject> keyList;

}

[RequireComponent(typeof(AudioSource))]
public class MainSoundEngine : MonoBehaviour {
	public static MainSoundEngine instance;

	public SoundObjectList BGMList;
	public SoundObjectList SFXList;

	public static void PlayBGM(string name)
	{
		instance.mPlayBGM(name);
	}

	public static void PlaySFX(string name)
	{
		instance.mPlaySFX(name);
	}

	public void mPlaySFX(string name)
	{
		AudioClip clip = SFXList[name];
		if (clip == null)
		{
			Debug.Log("No Sound To Play");
			return;
		}
		GetComponent<AudioSource>().PlayOneShot(clip);
	}

	public void mPlayBGM(string name)
	{
		AudioClip clip = BGMList[name];
		if (clip == null)
		{
			Debug.Log("No Sound To Play");
			return;
		}
		GetComponent<AudioSource>().PlayOneShot(clip);
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
		instance = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
