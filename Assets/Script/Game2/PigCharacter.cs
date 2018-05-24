using UnityEngine;
using System.Collections;

public class PigCharacter : MonoBehaviour {
	public bool IsSick = false;
	public AudioClip[] EatSound;
	public AudioClip SickSound;
	public AudioClip ExerciseSound;
	public AudioClip AngrySound;
	public GameObject HPNortify;

	public void Eating()
	{
		Animator anim = GetComponent<Animator> ();
		GetComponent<AudioSource>().PlayOneShot(EatSound[Random.Range(0,EatSound.Length)]);
		anim.Play ("PigEating",-1,0.0f);
	}
	public void Sick()
	{
		Animator anim = GetComponent<Animator> ();
		anim.Play ("PigSick",-1,0.0f);
		GetComponent<AudioSource>().PlayOneShot(SickSound);
		Game2Global.SetSick(true);
		GameObject hpObj = GameObject.Instantiate(HPNortify) as GameObject;
		hpObj.SetActive(true);
		hpObj.transform.position = this.transform.position;
		hpObj.GetComponent<HPNotification>().DecreaseLife(1);
		IsSick = true;
	}
	public void Angry()
	{
		GetComponent<AudioSource>().PlayOneShot(AngrySound);
		GameObject hpObj = GameObject.Instantiate(HPNortify) as GameObject;
		hpObj.SetActive(true);
		hpObj.transform.position = this.transform.position;
		hpObj.GetComponent<HPNotification>().DecreaseLife(1);
		Animator anim = GetComponent<Animator> ();
		anim.Play ("PigAngry", -1, 0.0f);
	}
	public void Exercise()
	{
		GetComponent<AudioSource>().PlayOneShot(ExerciseSound);

		Animator anim = GetComponent<Animator> ();
		anim.Play ("PigExercise",-1,0.0f);
	}

	public void Pause()
	{
		Animator anim = GetComponent<Animator> ();
		anim.StartPlayback();
	}

	public void UnPause()
	{
		Animator anim = GetComponent<Animator> ();
		anim.StopPlayback();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (IsSick) {
			Animator anim = GetComponent<Animator> ();
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("PigIdle"))
			{
				IsSick = false;
				Game2Global.SetSick (false);
			}
		}
	}
}
