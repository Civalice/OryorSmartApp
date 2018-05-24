using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class MainSoundSrc : MonoBehaviour {
	public static MainSoundSrc pGlobal;

	public AudioClip[] audioList;

	public static void PlaySound(string name)
	{
		foreach(AudioClip clip in pGlobal.audioList)
		{
			if (clip.name == name)
			{
				pGlobal.Play(clip);
				break;
			}
		}
	}

	public void Play(AudioClip clip)
	{
		GetComponent<AudioSource>().PlayOneShot(clip);
	}

	void Awake()
	{
		pGlobal = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
