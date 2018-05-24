using UnityEngine;
using System.Collections;

//Other than smarttip
//http://www.oryor.com/newweb/webservice_oryor/service_year2v1.php?task=updateInfo&view=1&like=1&dataId=1
//Smart Tips
//http://www.oryor.com/oryor_smart_app_year2/ws_client_year2v1.php?task=updateInfo&type=2&id=39&view=1&like=1

public class LikeAPI : MonoBehaviour {
	public string LikeURL;
	public void SendLike()
	{
		StopCoroutine("SendLikeProcess");
		StartCoroutine("SendLikeProcess");
	}
	
	IEnumerator SendLikeProcess()
	{
		string url = LikeURL;
		Debug.Log("Like URL = "+url);
		WWW www = new WWW(url);
		while (!www.isDone)
		{
			//waiting
			yield return null;
		}
		Debug.Log("Like API : "+www.text);
		Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
