using UnityEngine;
using System.Collections;

public class NewsboardDownloadTexture : MonoBehaviour {
	public delegate void textureCallback(Texture2D tex,NewsboardDownloadTexture dtx);
	public event textureCallback postDownloaded;
	public bool IsFinish = false;
	public WWW www = null;
	
	public float progress = 0.0f;
	// Use this for initialization
	void Start () {
	}
	
	public void StartDownload(string url)
	{
		IsFinish = false;
		progress = 0.0f;
		Debug.Log ("StartDownload Tex : " + url);
		StartCoroutine ("DownloadImage", url);
	}
	
	public void StopDownload()
	{
		if (www != null) {
			Debug.Log("XMLDownloader Dispose");
			www.Dispose ();
		}
		www = null;
		StopCoroutine ("DownloadImage");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public IEnumerator DownloadImage(string url) 
	{
		Debug.Log ("Start Download Image : " + url);
		www = new WWW(url);
		while(!www.isDone)
		{
			progress = www.progress;
			yield return null;
		}
		progress = www.progress;
		yield return www;
		/* EDIT: */
		if (!string.IsNullOrEmpty(www.error)){
			Debug.LogWarning("LOCAL FILE ERROR: "+www.error);
		} else if(www.texture == null) {
			Debug.LogWarning("LOCAL FILE ERROR: TEXTURE NULL");
		} else {
			IsFinish = true;
			if (postDownloaded != null)
				postDownloaded(www.texture,this);
		}
	}
}
