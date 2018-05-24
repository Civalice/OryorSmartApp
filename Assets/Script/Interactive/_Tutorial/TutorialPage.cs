using UnityEngine;
using System.Collections;

public class TutorialPage : MonoBehaviour {
	public MainMenuState state;
	public SpriteRenderer[] highlightList;
	private int[] OldLayerIDList;

	public void HighlightObject()
	{
		foreach(SpriteRenderer obj in highlightList)
		{
			//swap sortinglayerID
			Debug.Log(obj.name + " HighLight");
			obj.sortingLayerName = "Tutorial";
		}
		StopCoroutine("Animate");
		StartCoroutine("Animate");
	}

	public void UnHightlightObject()
	{
		Color white = new Color(1,1,1,1);

		for(int i = 0;i < highlightList.Length;i++)
		{
			highlightList[i].color = white;
			highlightList[i].sortingLayerID = OldLayerIDList[i];
		}
		StopCoroutine("Animate");
	}

	IEnumerator Animate()
	{
//		float alpha = 0.8f;
		Color alpha = new Color(1,1,1,0.5f);
		Color white = new Color(1,1,1,1);
		while (true)
		{
			Color currentColor = white;
			while (Mathf.Abs(currentColor.a - alpha.a) > 0.05f)
			{
				currentColor = Color.Lerp(currentColor,alpha,Time.deltaTime*5);
				foreach (SpriteRenderer obj in highlightList)
				{
					obj.color = currentColor;
				}
				yield return null;
			}
			currentColor = alpha;
			while (Mathf.Abs(currentColor.a - white.a) > 0.05f)
			{
				currentColor = Color.Lerp(currentColor,white,Time.deltaTime*5);
				foreach (SpriteRenderer obj in highlightList)
				{
					obj.color = currentColor;
				}
				yield return null;
			}
			currentColor = white;
		}
	}

	void Awake () {
		if (highlightList.Length > 0)
		{
			OldLayerIDList = new int[highlightList.Length];
			for(int i = 0;i < highlightList.Length;i++)
			{
				OldLayerIDList[i] = highlightList[i].sortingLayerID;
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}
}
