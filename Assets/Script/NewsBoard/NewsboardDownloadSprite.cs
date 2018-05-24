using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(SpriteRenderer))]
public class NewsboardDownloadSprite : MonoBehaviour {
	public NewsboardContentObject cObject;
	public Vector2 size = new Vector2(100,100);
	public GameObject Indecator = null;
	private Vector2 scaleSize = new Vector2(1,1);
	private Vector2 anchor = new Vector2(0.5f,0.5f);
	private NewsboardDownloadTexture mTex;	
	private Sprite sprite;
	private bool IsSpriteCreate = false;

	public void AddDownloadTexture(NewsboardDownloadTexture dtx)
	{
		IsSpriteCreate = false;
		mTex = dtx;
		dtx.postDownloaded += CreateSprite;
	}
	
	public void SetNewAnchor(float x,float y)
	{
		anchor = new Vector2 (x, y);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void DisposeSprite()
	{
		if (IsSpriteCreate)
			Texture2D.Destroy(sprite.texture);
	}
	
	void CreateSprite(Texture2D tex,NewsboardDownloadTexture dtx)
	{
		if (Indecator != null) {
			Indecator.SetActive (false);
		}
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),anchor,100.0f);
		if (cObject != null) {
			scaleSize = new Vector2 (size.x / tex.width, size.x / tex.width);
			cObject.setpictureHeight(tex.height * size.x / tex.width);
		} else {
			scaleSize = new Vector2(size.x/tex.width,size.y/tex.height);
			cObject.setpictureHeight(size.y/tex.height);
		}
		
		renderer.sprite = sprite;
		this.transform.localScale = scaleSize;
		IsSpriteCreate = true;
		
	}
}
