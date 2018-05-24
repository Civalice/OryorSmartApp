using UnityEngine;
using System.Collections;
using TMPro;

public class NewsboardContent : MonoBehaviour {
	public NewsboardDetail NewsDetail;
	public NewboardDetailLayer nDetailLayer;
	public TextMeshPro TextTitle;
	//public TextMeshPro TextDesc;
	private NewsboardContent pGlobal;
	public Collider2D mbox;

	public GameObject hotIcon;
	public GameObject pragardIcon;
	public GameObject newsIcon;
	public AudioSource mSound;

	private bool IsDetail=false;
	private string linkweb="";

	void Awake() 
	{
		TextTitle.maxVisibleLines = 2;
	}

	// Use this for initialization
	void Start () {
		pGlobal = this;
		mSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
		if (touchedDown) {
			if (mbox.OverlapPoint(touchPos))
			{
				if(mSound!=null){mSound.Play();}
				IsDetail=true;
				if(linkweb!=""){nDetailLayer.PopWebPage(linkweb);}
				else{nDetailLayer.OpenDetail(NewsDetail);}

			}
		}
	}

	public void setContent(NewsboardDetail nDetail){
		NewsDetail = nDetail;

		TextTitle.text = nDetail.title;
		linkweb = nDetail.link_web;

		if(nDetail.pin=="1"){
			pragardIcon.SetActive(true);
		}
		else{
			if(nDetail.hot=="1"){
				hotIcon.SetActive(true);
			}
		}
	}
}
