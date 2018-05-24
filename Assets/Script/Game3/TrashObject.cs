using UnityEngine;
using System.Collections;

public class TrashObject : MonoBehaviour {
	SpriteRenderer sr;
	public Sprite OpenCan;
	public Sprite CloseCan;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TrashEnter()
	{
		StopCoroutine("StartAnim");
		StartCoroutine("StartAnim");
	}

	IEnumerator StartAnim()
	{
		sr.sprite = CloseCan;
		yield return new WaitForSeconds(1.0f);
		sr.sprite = OpenCan;
	}
}
