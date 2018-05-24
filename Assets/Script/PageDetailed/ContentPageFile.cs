using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;


[System.Serializable]
[XmlRoot("ContentRoot")]
public class ContentPageFileXML {
	[XmlArray("LikeIdList")]
	[XmlArrayItem("Id")]
	public int[] LikeIdList;
	[XmlArray("ItemList")]
	[XmlArrayItem("Item")]
	public ContentData[] ContentList;
	
	public void Save(string path)
	{
		Debug.Log("Saving : "+path);
		XmlSerializer serializer = new XmlSerializer(typeof(ContentPageFileXML));
		#if UNITY_WP8
		
		XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
		
		serializer.Serialize(writer,this);
		
		#else
		
		using(XmlWriter writer = XmlTextWriter.Create(path))
		{
			serializer.Serialize(writer,this);
		}
		
		
		#endif

	}
	
	public static ContentPageFileXML Load (string path)
	{
		Debug.Log("Loading : "+path);
		if (File.Exists(path))
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ContentPageFileXML));
			using (XmlReader reader = XmlReader.Create(path))
			{
				return serializer.Deserialize(reader) as ContentPageFileXML;
			}
		}
		else
		{
			return new ContentPageFileXML();
		}
	}
};

public class ContentPageFile : MonoBehaviour {
	public string filePath = "";
	public int maxContent = 20;
	public ContentPageFileXML fileXml;
	public List<int> likeList;
	public List<ContentData> contentList;
	public static List<DownloadedTexture> downloadTextureList = new List<DownloadedTexture> ();
	public static List<DownloadedTexture> downloadingList = new List<DownloadedTexture>();
	public static DownloadedFile RawFile = null;

	public static ContentPageFile gPageFile;

	public static float GetProgress()
	{
		int fileCount = 0;
		float currentProgress = 0;
		fileCount += downloadingList.Count;
		foreach(DownloadedTexture tex in downloadingList)
		{
			currentProgress += tex.progress;
		}
		if (RawFile != null)
		{
			currentProgress += RawFile.progress;
			fileCount ++;
		}
		return currentProgress/fileCount;
	}

	public static DownloadedTexture CreateLoadTexture()
	{
		Debug.Log("Create Download Texture");
		GameObject dtx = new GameObject ("DownloadTex");
		DownloadedTexture dTexture = dtx.AddComponent<DownloadedTexture> ();
		dtx.transform.parent = gPageFile.transform;
		downloadTextureList.Add (dTexture);
		return dTexture;
	}
	public static void ClearLoadTexture()
	{
		Debug.Log("downloadTextureList : "+downloadTextureList.Count);
		foreach (DownloadedTexture dtx in downloadTextureList) {
			dtx.StopDownload();
			Destroy(dtx.gameObject);
		}
		downloadTextureList.Clear ();
	}

	public static void SaveContentFile()
	{
		gPageFile.fileXml.ContentList = gPageFile.contentList.ToArray ();
		gPageFile.fileXml.LikeIdList = gPageFile.likeList.ToArray();
		gPageFile.fileXml.Save(gPageFile.filePath);
	}

	public bool IsFavourite(int id)
	{
		foreach (ContentData data in contentList) {
			if (id == data.dataid)
			{
				//duplicate item
				return true;
			}
		}
		return false;
	}

	public bool AddContentToFile(ContentData cData, bool IsDownloadAll)
	{
		if (contentList.Count > maxContent) {
			//Max Item
			return false;
		}
		foreach (ContentData data in contentList) {
			if (cData.dataid == data.dataid)
			{
				//duplicate item
				return false;
			}
		}
		DownloadTextureFile (cData.imgthumbnail);
		DownloadTextureFile (cData.img);
		if (IsDownloadAll)
		{
			//add another download file here
			if (cData.pdfurl != "")
			{
				DownloadFile(cData.pdfurl);
			}
			else if (cData.vdourl != "")
			{
				DownloadFile(cData.vdourl);
			}

		}

//		StartCoroutine ("NowSaving",cData,IsDownloadAll);
		StartCoroutine(NowSaving(cData,IsDownloadAll));
		return true;
	}

	IEnumerator NowSaving(ContentData cData,bool IsDownloadAll)
	{
		while (downloadingList.Count > 0) {
			yield return null;
		}
		//modified contentList at here first
		ContentData toSave = ModifiedDataForSave(cData,IsDownloadAll);
		contentList.Add (toSave);
		cData.IsFav = true;
		fileXml.ContentList = contentList.ToArray ();
		fileXml.LikeIdList = likeList.ToArray();
		fileXml.Save (filePath);
		Debug.Log ("Saved Content");
	}

	public void RemoveContent(ContentData cData)
	{
		for (int i = 0; i < contentList.Count; i++) {
			if (contentList[i].dataid == cData.dataid)
			{
				contentList.Remove(contentList[i]);
				//remove Downloadtexture from child
				Transform thumb = transform.Find(GetFileName( cData.imgthumbnail));
				Destroy(thumb.gameObject);
				break;
			}
		}
		fileXml.ContentList = contentList.ToArray ();
		fileXml.LikeIdList = likeList.ToArray();
		fileXml.Save (filePath);
	}

	static public ContentData ModifiedDataForSave (ContentData cData,bool IsDownloadAll)
	{	
	
		ContentData toSave = ContentData.Clone(cData);

		toSave.imgthumbnail = GetFileName(cData.imgthumbnail);
		if (toSave.img != null)
		{
			for (int i = 0; i < toSave.img.Length; i++) {
					toSave.img [i] = GetFileName(cData.img[i]);
			}
		}
		if (IsDownloadAll)
		{
			if (cData.pdfurl != "")
				toSave.pdfurl = "file://"+System.IO.Path.Combine(Application.persistentDataPath,GetFileName(cData.pdfurl));
			if (cData.vdourl != "")
				toSave.vdourl = "file://"+System.IO.Path.Combine(Application.persistentDataPath,GetFileName(cData.vdourl));
		}
		return toSave;
	}
	static public ContentData ModifiedDataForLoad (ContentData cData)
	{
		ContentData toLoad = ContentData.Clone(cData);
		toLoad.imgthumbnail = System.IO.Path.Combine(Application.persistentDataPath,cData.imgthumbnail);
		for (int i = 0;(toLoad.img != null)&&( i < toLoad.img.Length); i++) {
			toLoad.img [i] = System.IO.Path.Combine(Application.persistentDataPath,cData.img[i]);
		}
		return toLoad;
	}

	public static string GetFileName(string url)
	{
		string[] strSplit = url.Split("/".ToCharArray(),System.StringSplitOptions.None);
		return strSplit [strSplit.Length - 1];
	}



	void DownloadTextureFile(string[] imgList)
	{
		if (imgList.Length <= 0) 
		{
			Debug.Log("No ImgList Downloaded");
			return;
		}
		foreach (string img in imgList) {
			if (img.Length <= 0)
			{
				Debug.Log("No Img Downloaded");
			}
			else
			{
				GameObject dtx = new GameObject (GetFileName(img));
				DownloadedTexture dTexture = dtx.AddComponent<DownloadedTexture> ();
				dTexture.postDownloaded += saveFileTexture;
				dtx.transform.parent = this.transform;
				dTexture.StartDownload (img);
				downloadingList.Add(dTexture);
			}
		}
	}
	void DownloadTextureFile(string img)
	{
		if (img.Length <= 0) 
		{
			Debug.Log("No Img Downloaded");
			return;
		}
		GameObject dtx = new GameObject (GetFileName(img));
		DownloadedTexture dTexture = dtx.AddComponent<DownloadedTexture> ();
		dTexture.postDownloaded += saveFileTexture;
		dtx.transform.parent = this.transform;
		dTexture.StartDownload (img);
		downloadingList.Add(dTexture);
	}

	void DownloadFile(string file)
	{
		if (file.Length <= 0) 
		{
			Debug.Log("No Img Downloaded");
			return;
		}
		GameObject dtx = new GameObject (GetFileName(file));
		RawFile = dtx.AddComponent<DownloadedFile> ();
		RawFile.postDownloaded += saveFileData;
		dtx.transform.parent = this.transform;
		RawFile.StartDownload (file);
	}

	public void ResetFile()
	{
		filePath = Application.persistentDataPath + "/" + PageDetailGlobal.type.ToString() + ".xml";
		//switch type from PageDetailGlobal
		fileXml = ContentPageFileXML.Load (filePath);
		if (fileXml.ContentList == null)
			contentList = new List<ContentData> ();
		else
			contentList = new List<ContentData> (fileXml.ContentList);
		if (fileXml.LikeIdList == null)
			likeList = new List<int>();
		else
			likeList = new List<int>(fileXml.LikeIdList);
		fileXml.Save (filePath);
	}

	// Use this for initialization
	void Start () {
		gPageFile = this;
	}
	
	// Update is called once per frame
	void Update () {
			
	}
	void saveFileData(byte[] rawData,DownloadedFile fDownload)
	{
		string fileName = GetFileName (fDownload.www.url);
		
		byte[] bytes= rawData;
		using (FileStream file = new FileStream(Application.persistentDataPath + "/"+fileName,FileMode.Create)) {
			var binary = new BinaryWriter (file);
			binary.Write (bytes);
		}
		Destroy(fDownload.gameObject);
		RawFile = null;
	}
	void saveFileTexture(Texture2D tex,DownloadedTexture dtx)
	{
		string fileName = GetFileName (dtx.www.url);

		byte[] bytes= tex.EncodeToPNG();
		using (FileStream file = new FileStream(Application.persistentDataPath + "/"+fileName,FileMode.Create)) {
				var binary = new BinaryWriter (file);
				binary.Write (bytes);
		}
		downloadingList.Remove (dtx);
		Debug.Log("Downloading Count : "+downloadingList.Count);
		Destroy(dtx.gameObject);
	}
}
