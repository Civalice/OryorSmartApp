using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class ReadMoreButton : MonoBehaviour {
	public Collider2D ButtonCollider;
	public AudioClip sound;
	public ContentData cData;
	private bool IsEnable = false;
	private bool IsFavourite = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
			return;
		if (!IsEnable)
			return;
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if (ButtonCollider.OverlapPoint(touchPos))
			{
				//show button
				GetComponent<AudioSource>().PlayOneShot(sound);
//				mSound.playContentSound("click");
				if (IsFavourite)
					PageDetailGlobal.PopFavouriteContentPage(cData);
				else
					PageDetailGlobal.PopContentPage(cData);
			}
		}
	}
	public void SetContentData(ContentData data,bool FavFlag)
	{
		IsFavourite = FavFlag;
		cData = data;
	}

	public void SetButtonEnable(bool flag)
	{
		IsEnable = flag;
	}
}
