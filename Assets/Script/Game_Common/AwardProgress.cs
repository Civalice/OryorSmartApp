using UnityEngine;
using System.Collections;
using TMPro;

public class AwardProgress : MonoBehaviour {
	public GameObject AwardProgressBar;
	public TextMeshPro textTitle;

	public bool SetUpAward(int progress,int max)
	{
		//set progress first
		if (AwardProgressBar != null)
		{
			if (progress > max) progress = max;
			float scale = progress/(float)max;
			string text = progress.ToString() + " / "+max.ToString();
			AwardProgressBar.transform.localScale = new Vector3(scale,1,1);
			textTitle.text = text;
		}
		return (progress == max);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
