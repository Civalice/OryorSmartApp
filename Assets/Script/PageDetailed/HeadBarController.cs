using UnityEngine;
using System.Collections;

public class HeadBarController : MonoBehaviour {
	public Collider2D NewReleaseCollider;
	public GroupButton NewReleaseButton;

	public Collider2D MostViewCollider;
	public GroupButton MostViewButton;

	public Collider2D MostLikeCollider;
	public GroupButton MostLikeButton;

	public ContentMainSound mSound;

	public int FirstButton = 1;
	private int CurrentButton = 1;
	// Use this for initialization
	void Start () {
		NewReleaseButton.OnPressed += NewReleasePressed;
		MostViewButton.OnPressed += MostViewPressed;
		MostLikeButton.OnPressed += MostLikePressed;
		CurrentButton = FirstButton;
		switch (FirstButton) {
		case 1:
			NewReleaseButton.Press ();
			MostViewButton.Release ();
			MostLikeButton.Release ();
			break;
		case 2:
			NewReleaseButton.Release ();
			MostViewButton.Press ();
			MostLikeButton.Release ();
			break;
		case 3:
			NewReleaseButton.Release ();
			MostViewButton.Release ();
			MostLikeButton.Press ();
			break;
				}
	}
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != DetailState.DS_LIST)
						return;
				bool touchedDown = TouchInterface.GetTouchDown ();
				bool touchedUp = TouchInterface.GetTouchUp ();
				Vector2 touchPos = TouchInterface.GetTouchPosition ();
				RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
				if (touchedDown) {
						if (NewReleaseCollider.OverlapPoint(touchPos)) {
				mSound.playContentSound("click");
				NewReleaseButton.Press ();
								MostViewButton.Release ();
								MostLikeButton.Release ();
						}
						if (MostViewCollider.OverlapPoint(touchPos)) {
				mSound.playContentSound("click");
				NewReleaseButton.Release ();
								MostViewButton.Press ();
								MostLikeButton.Release ();
						}
						if (MostLikeCollider.OverlapPoint(touchPos)) {
				mSound.playContentSound("click");
				NewReleaseButton.Release ();
								MostViewButton.Release ();
								MostLikeButton.Press ();
						}
				}
		}


	void MostLikePressed()
	{
//		PageDetailGlobal.xmlDownload (0);
		PageDetailGlobal.DownloadNewSort ("like");
	}

	void NewReleasePressed()
	{
		PageDetailGlobal.DownloadNewSort ("date");
	}

	void MostViewPressed()
	{
		PageDetailGlobal.DownloadNewSort ("view");
	}
}
