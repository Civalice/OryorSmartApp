using UnityEngine;
using System.Collections;
using TMPro;

public class NewsboardContentObject : MonoBehaviour {
	public NewsboardInfo nInfo;
	public static NewsboardContentObject pGlobal;
	public TextMeshPro DetailedText;
	public NewsboardDownloadSprite SpriteTex;
	public GameObject loading;
	
	private NewsboardDownloadTexture dTexture;
	private GameObject dtx;
	
	private Bounds textBoundery;
	public float height = 0;
	public float pictureHeight = 0;
	public float textHeight = 0;
	
	private int idx;
	
	public void Dispose()
	{
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
	}
	
	public void setupImg(string link,int i)
	{
		loading.SetActive (true);
		if (link == "") return;
		idx = i;
		dtx = new GameObject ("DownloadTex");
		dTexture = dtx.AddComponent<NewsboardDownloadTexture> ();
		SpriteTex.SetNewAnchor (0.5f, 1.0f);
		SpriteTex.AddDownloadTexture (dTexture);
		dtx.transform.parent = this.transform;
		dTexture.StartDownload (link);
		height += SpriteTex.size.y/100.0f + 0.5f;
//		Debug.Log ("ContentObject Picture Height : "+pictureHeight);
		height += pictureHeight + 0.8f;
	}
	public void setupDetailText(string text)
	{
		loading.SetActive (false);
		DetailedText.transform.localPosition = new Vector3 (0, -height, 0);
		DetailedText.text = StringUtil.ParseUnicodeEscapes(text);
		DetailedText.ForceMeshUpdate();
		textBoundery = DetailedText.bounds;
		height += textBoundery.size.y;
	}
	// Use this for initialization
	void Start () {
		pGlobal = this;
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void setpictureHeight(float picHeight){
		loading.SetActive (false);
//		pictureHeight = picHeight/100.0f;
//		DetailedText.transform.localPosition = new Vector3 (0, -picHeight, 0);
////		idx = i;
//		height += pictureHeight + 0.5f;
//		nInfo.pGlobal.setContentPosition (pictureHeight, idx);
	}
}
