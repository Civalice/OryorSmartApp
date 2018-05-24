using UnityEngine;
using System.Collections;

public class DownloadedFile : MonoBehaviour {
	public delegate void textureCallback(byte[] rawData,DownloadedFile file);
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
		Debug.Log ("StartDownload File : " + url);
		StartCoroutine ("DownloadFile", url);
	}
	
	public void StopDownload()
	{
		if (www != null) {
			Debug.Log("XMLDownloader Dispose");
			www.Dispose ();
		}
		www = null;
		StopCoroutine ("DownloadFile");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public IEnumerator DownloadFile(string url) 
	{
		Debug.Log ("Start Download File : " + url);
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
		} else if((www.bytes!=null)&&(www.bytes.Length <= 0)) {
			Debug.LogWarning("LOCAL FILE ERROR: TEXTURE NULL");
		} else {
			IsFinish = true;
			if (postDownloaded != null)
				postDownloaded(www.bytes,this);
		}
	}
}
