using UnityEngine;
using System.Collections;

public class SnailFace : MonoBehaviour {
	public Sprite NormalFace;
	public Sprite HappyFace;
	public Sprite BoredFace;
	public Sprite ShockFace;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HappySnail()
	{
		StopCoroutine ("ChangeFace");
		StartCoroutine ("ChangeFace", HappyFace);
	}
	public void BoredSnail()
	{
		StopCoroutine ("ChangeFace");
		StartCoroutine ("ChangeFace", BoredFace);
	}
	public void ShockSnail()
	{
		StopCoroutine ("ChangeFace");
		StartCoroutine ("ChangeFace", ShockFace);
	}

	IEnumerator ChangeFace(Sprite sp)
	{
		SpriteRenderer spr = GetComponent<SpriteRenderer> ();
		spr.sprite = sp;
		yield return new WaitForSeconds(2.0f);
		spr.sprite = NormalFace;
	}
}
