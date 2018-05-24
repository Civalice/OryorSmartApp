using UnityEngine;
using System.Collections;

public class PopScreen : MonoBehaviour {
	public GameObject SwapInPts;
	public GameObject SwapOutPts;
	private bool IsPopIn = false;

	public void SwapIn()
	{
		StopCoroutine("PopIn");
		StopCoroutine("PopOut");
		StartCoroutine("PopIn");
	}
	public void SwapOut()
	{
		StopCoroutine("PopIn");
		StopCoroutine("PopOut");
		StartCoroutine("PopOut");
	}

	IEnumerator PopIn()
	{
		Vector3 targetPos = SwapInPts.transform.localPosition;
		while (Vector3.Distance(transform.localPosition,targetPos)>=0.05f)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition,targetPos,Time.deltaTime*10);
			yield return null;
		}
		transform.localPosition = targetPos;
	}

	IEnumerator PopOut()
	{
		Vector3 targetPos = SwapOutPts.transform.localPosition;
		while (Vector3.Distance(transform.localPosition,targetPos)>=0.05f)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition,targetPos,Time.deltaTime*10);
			yield return null;
		}
		transform.localPosition = targetPos;
	}
	// Use this for initialization
	protected void Start () {
		this.transform.localPosition = SwapOutPts.transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
