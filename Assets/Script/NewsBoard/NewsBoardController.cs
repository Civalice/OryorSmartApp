using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewsBoardController : MonoBehaviour {
	public GameObject newsIssue;
	public GameObject pragardIssue;
	public GameObject Loading;
	public GameObject News;

	public ButtonObject closeButton;

	public NewsBoardDownloader xmlData;
	private bool IsDone=false;
	public Animator loadControl;
	
	static public NewsBoardController pGlobal;

	private List<GameObject> newsList = new List<GameObject>();
	private int contentCount=0;
	private int countPragard=0;
	private int countNews=0;
	
	// Use this for initialization
	void Start () {
		pGlobal = this;
//		loadControl.Play (0);
		xmlData.postDownloaded += FinishDownload;
		closeButton.OnReleased += CloseLayer;
		loadXML();
	}

	void CloseLayer()
	{
		MainMenuGlobal.SetNewsObject(false);
	}

	// Update is called once per frame
	void Update () {
	
	}
	public void loadXML(){
		Debug.Log(": Start Load :");

		xmlData.StopDownload();
		xmlData.StartDownload();
		
	}
	public void FinishDownload(){
		Debug.Log("Finish Download");
		
		Loading.SetActive(false);

		IsDone = true;

		clearContentData();
		contentCount = xmlData.contentList.Length;


		GameObject pragardHead = (GameObject)GameObject.Instantiate(pragardIssue);
		pragardHead.SetActive(true);
		pragardHead.transform.parent = this.transform;
		pragardHead.transform.localPosition = new Vector3 (0, -3.6f, 0);
		newsList.Add (pragardHead);

		for(int i=0;i<contentCount;i++){
			createPragardContentData(xmlData.contentList[i]);
		}

		GameObject newsHead = (GameObject)GameObject.Instantiate(newsIssue);
		newsHead.SetActive(true);
		newsHead.transform.parent = this.transform;
		newsHead.transform.localPosition = new Vector3 (0, -1.6f - countPragard * 0.83f, 0);
		newsList.Add (newsHead);
		
		for(int i=0;i<contentCount;i++){
			createNewsContentData(xmlData.contentList[i]);
		}
	}

	void clearContentData(){
		if(newsList!=null){
			newsList.Clear ();
		}
		else{
			return;
		}
	}
	void createPragardContentData(NewsboardDetail nDetail){
		if(nDetail.pin=="1"){
			GameObject itemDetail = (GameObject)GameObject.Instantiate(News);
			itemDetail.SetActive(true);
			itemDetail.transform.parent = this.transform;
			itemDetail.transform.localPosition = new Vector3 (0, -0.7f - countPragard * 0.83f, 0);
			
			NewsboardContent idt = itemDetail.GetComponent<NewsboardContent>();
			idt.setContent(nDetail);
			newsList.Add (itemDetail);
			countPragard++;
		}
	}
	void createNewsContentData(NewsboardDetail nDetail){
		if(nDetail.pin!="1"){
			GameObject itemDetail = (GameObject)GameObject.Instantiate(News);
			itemDetail.SetActive(true);
			itemDetail.transform.parent = this.transform;
			itemDetail.transform.localPosition = new Vector3 (0, -1.8f - (countNews+countPragard) * 0.83f, 0);
			
			NewsboardContent idt = itemDetail.GetComponent<NewsboardContent>();
			idt.setContent(nDetail);
			newsList.Add (itemDetail);
			countNews++;
		}
	}
}
