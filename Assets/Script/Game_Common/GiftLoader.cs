using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

[XmlRoot("GiftSendSaveFile")]
public class GiftSendSaveFile
{
	[XmlArray("ItemList")]
	[XmlArrayItem("Item")]
	public GiftSend[] SendItem;
	
	public void Save(string path)
	{
		
		
		
		
		#if UNITY_WINRT
		
		//		XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
		//
		//		serializer.Serialize(writer,this);
		
		byte [] bytes = UnityPluginForWindowsPhone.FileIO.SerializeObject(this);
		//			
		////			File.WriteAllBytes(file, bytes);
		string textData = Convert.ToBase64String (bytes);
		
		//	string  textData=	Newtonsoft.Json.JsonConvert.SerializeObject(RecordFood);
		writeFile(path , textData);
		
		#else
		XmlSerializer serializer = new XmlSerializer(typeof(GiftSendSaveFile));
		
		using(XmlWriter writer = XmlTextWriter.Create(path))
		{
			serializer.Serialize(writer,this);
		} 
		
		
		#endif
		
		
	}
	
	public static GiftSendSaveFile Load (string path)
	{
		if (File.Exists(path))
		{
			
			#if UNITY_WINRT
			string line = readFile(path );
			
			
			
			GiftSendSaveFile	data = UnityPluginForWindowsPhone.FileIO.DeserializeObject<GiftSendSaveFile>(line);
			if(data == null){
				
				data = new GiftSendSaveFile();
				
			}
			return data;
			#else
			
			XmlSerializer serializer = new XmlSerializer(typeof(GiftSendSaveFile));
			
			using (XmlReader reader = XmlReader.Create(path))
			{
				return serializer.Deserialize(reader) as GiftSendSaveFile;
			}
			#endif
		}
		else
		{
			return new GiftSendSaveFile();
		}
	}
	
	
	private static void writeFile(string path , string data){
		
		using (StreamWriter sw = new StreamWriter(path)) 
		{
			// Add some text to the file.
			sw.Write(data , 0 , data.Length);
			
		}
	}
	private static string readFile(string path ){
		
		try 
		{
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader(path)) 
			{
				string line;
				#if UNITY_WINRT
				line = sr.ReadToEnd();
				return line;
				#else
				while ((line = sr.ReadLine()) != null) 
				{
					Console.WriteLine(line);
				}
				
				#endif
				// Read and display lines from the file until the end of 
				// the file is reached.
				
				return "";
			}
		}
		catch (Exception e) 
		{
			// Let the user know what went wrong.
			//			Console.WriteLine("The file could not be read:");
			//			Console.WriteLine(e.Message);
		}
		return "";
	}
	
}



[System.Serializable]
public class GiftSend
{
	[XmlAttribute]
	public string user_id;
	[XmlAttribute]
	public string tick;
}

public class GiftSentGlobal
{
	public static GiftSentGlobal pGlobal;
	public static long GiftCountdown = (System.TimeSpan.TicksPerMinute*3);
	
	public GiftSendSaveFile file;
	public List<GiftSend> sentList;
	
	string filePath;
	
	public static GiftSentGlobal Instance()
	{
		if (pGlobal == null)
			pGlobal = new GiftSentGlobal();
		return pGlobal;
	}

	#if !UNITY_WEBGL && !DISABLE_WEBVIEW
	public static void Initialize()
	#else
	public static void Initialize(string message)
	#endif
	{
		pGlobal = GiftSentGlobal.Instance();
		pGlobal.filePath = Application.persistentDataPath + "/" + "GiftList.xml";
		pGlobal.file = GiftSendSaveFile.Load(pGlobal.filePath);
		if (pGlobal.file.SendItem!=null)
			pGlobal.sentList = new List<GiftSend>(pGlobal.file.SendItem);
		else
			pGlobal.sentList = new List<GiftSend>();
	}
	
	public static void AddGift(GiftSend item)
	{
		foreach(GiftSend gs in pGlobal.sentList)
		{
			if (gs.user_id == item.user_id)
			{
				gs.tick = item.tick;
				UpdateGift();
				return;
			}
		}
		pGlobal.sentList.Add(item);
		UpdateGift();
	}
	
	public static void UpdateGift()
	{
		List<GiftSend> list = pGlobal.sentList;
		//loop check
		for(int i = 0;i < list.Count;i++)
		{
			GiftSend item = list[i];
			Debug.Log("item username = "+item.user_id);
			long countTick = System.DateTime.UtcNow.Ticks-long.Parse(item.tick);
			long CountdownTick = GiftCountdown-countTick;
			if (CountdownTick < 0)
			{
				//remove
				list.Remove(item);
				i--;
			}
		}
		pGlobal.file.SendItem = list.ToArray();
		pGlobal.file.Save(pGlobal.filePath);
	}
	
	public static long CalculateCountDownTick(GiftSend item)
	{
		return GiftCountdown-(System.DateTime.UtcNow.Ticks-long.Parse(item.tick));
	}
	
	public static GiftSend getGift(string user_id)
	{
		foreach(GiftSend item in pGlobal.sentList)
		{
			if (item.user_id == user_id)
			{
				GiftSend ret = new GiftSend();
				ret.tick = item.tick;
				ret.user_id = item.user_id;
				return ret;
			}
		}
		return null;
	}
}

public class GiftLoader : MonoBehaviour {
	public static GiftLoader pGlobal;
	
	public string ShowGiftURL;
	public string SentGiftURL;
	public string ReceiveGiftURL;
	WWW loader;
	public bool isFinish = false;
	public delegate void eventCallback();
	public event eventCallback postDownloaded;
	
	public GiftItemData[] giftList;
	
	void Awake ()
	{
		pGlobal = this;
	}
	
	public void LoadGift()
	{
		StartCoroutine("Loading");
	}
	
	IEnumerator Loading()
	{
		WWWForm form = new WWWForm();
		//check ID = 7 for test
		form.AddField("user_receive_id",UserCommonData.pGlobal.user.user_id);
		loader = new WWW(ShowGiftURL,form);
		yield return loader;
		if (loader.error != null)
		{
			Debug.Log("HTTP ERROR");
			LoadingScript.HideLoading();
			yield break;
		}
		Debug.Log(loader.text);
		if (loader.text == "") 
		{
			LoadingScript.HideLoading();
			yield break;
		}
		yield return new WaitForSeconds(3.0f);
		JSONObject json = new JSONObject(loader.text);
		//Casting Data
		if (json["msg"].str == "OK")
		{
			//get Gift list
			JSONObject arr = json["gift"];
			if (arr.list != null)
				giftList = new GiftItemData[arr.list.Count];
			else
				giftList = null;
			int i = 0;
			foreach(JSONObject content in arr.list){
				GiftItemData data = new GiftItemData();
				data.GiftKey = content["user_gift_id"].str;
				data.name = StringUtil.ParseUnicodeEscapes(content["user_name"].str);
				data.id = int.Parse(content["GiftId"].str);
				data.giftName = StringUtil.ParseUnicodeEscapes(content["GiftName"].str);
				giftList[i] = data;
				i++;
			}
		}
		isFinish = true;
		if (postDownloaded != null)
			postDownloaded();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
