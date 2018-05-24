using UnityEngine;
using System.Collections;
using TMPro;

public class ContentObject : MonoBehaviour {
	public TextMeshPro DetailedText;
	public DownloadSprite SpriteTex;

	private DownloadedTexture dTexture;

	private Bounds textBoundery;
	public float height = 0;

	public void Dispose()
	{
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	public void setupImg(string link)
	{
		if (link == "") return;
		GameObject dtx = new GameObject ("DownloadTex");
		dTexture = dtx.AddComponent<DownloadedTexture> ();
		SpriteTex.SetNewAnchor (0.5f, 1.0f);
		SpriteTex.AddDownloadTexture (dTexture,"Detail");
//		SpriteTex.AddDownloadTexture (dTexture);
		dtx.transform.parent = this.transform;
		dTexture.StartDownload (link);
		Debug.Log ("setupImg height : "+SpriteTex.size.y);
		height += SpriteTex.size.y/100.0f + 0.5f;
	}
	public void setupDetailText(string text)
	{
		DetailedText.transform.localPosition = new Vector3 (0, -height, 0);
		DetailedText.text = StringUtil.ParseUnicodeEscapes(text);
		DetailedText.ForceMeshUpdate();
		textBoundery = DetailedText.bounds;
		height += textBoundery.size.y;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
