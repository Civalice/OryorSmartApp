using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(SpriteRenderer))]
public class DownloadSprite : MonoBehaviour {
	public Vector2 size = new Vector2(100,100);
	public GameObject Indecator = null;
	private Vector2 scaleSize = new Vector2(1,1);
	private Vector2 anchor = new Vector2(0.5f,0.5f);
	private DownloadedTexture mTex;	
	private Sprite sprite;
	private bool IsSpriteCreate = false;
	private string op = "Content";
	public void AddDownloadTexture(DownloadedTexture dtx, string _op="Content")
	{
//		IsSpriteCreate = false;
		mTex = dtx;
		op = _op;

		if(op == "Content"){
			size.y = 155;
		}
		else if(op == "FavThumb"){
			size.y = 120;
		}
		else if (PageDetailGlobal.GetHeaderIndex ((int)PageDetailGlobal.type - 1) == "รายชื่อผลิตภัณฑ์" && op == "Detail") {
			size.y = 900;
		}
		else if(PageDetailGlobal.GetHeaderIndex ((int)PageDetailGlobal.type - 1) == "Infographic" && op == "Detail"){
			size.y = 900;
		}else {
			size.y = 450;
		}

		Debug.Log ("AddDownloadTexture : "+op);
		if (Indecator != null) {
			Indecator.SetActive (true);
		}
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

	void CreateSprite(Texture2D tex,DownloadedTexture dtx)
	{
			if (Indecator != null) {
				Indecator.SetActive (false);
			}
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),anchor,100.0f);

		float sizeheight = tex.height / size.x * tex.width;
		Debug.Log ("Download Image Page : "+op+" :"+PageDetailGlobal.GetHeaderIndex ((int)PageDetailGlobal.type - 1));
		scaleSize = new Vector2(size.x/tex.width,size.y/tex.height);

		renderer.sprite = sprite;
		this.transform.localScale = scaleSize;
		IsSpriteCreate = true;

	}
}
