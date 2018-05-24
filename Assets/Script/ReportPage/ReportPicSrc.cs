using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ReportPicSrc : MonoBehaviour {
//	public Collider2D CloseBtn;
	public ButtonObject CloseButton;
	public Vector2 size = new Vector2(100,100);
	public SpriteRenderer SpriteSrc;
	public byte[] rawData;
	public int idx = 0;
	private Vector2 scaleSize = new Vector2(1,1);
	private Vector2 anchor = new Vector2(0.5f,0.5f);

	public ReportMainSound mSound;

	// Use this for initialization
	void Start () {
		CloseButton.OnReleased += RemovePicture;
	}

	void RemovePicture()
	{
		ReportCamera.PictureList.Remove(this);
		//Rearrange PictureList
		int i = 0;
		foreach(ReportPicSrc src in ReportCamera.PictureList)
		{
			src.transform.localPosition = new Vector3(i * 1.5f,0,0);
			i++;
		}
		mSound = GetComponent<ReportMainSound>();
		if(mSound != null){
			mSound.playReportSound("click");
		}
		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void SetImg(Texture2D tex)
	{
		if (SpriteSrc == null) return;
		Sprite sprite = new Sprite();
		sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),anchor,100.0f);
		float scale = (size.x/tex.width > size.y/tex.height)?size.y/tex.height:size.x/tex.width;
		scaleSize = new Vector2(scale,scale);
		SpriteSrc.sprite = sprite;
		SpriteSrc.transform.localScale = scaleSize;
	}
}
