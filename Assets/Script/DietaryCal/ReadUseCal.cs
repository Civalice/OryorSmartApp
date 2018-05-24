using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

[XmlRoot("kcal")]
public class UseCal{
	[XmlArray("list"),XmlArrayItem("act")]
	public act[] acts;

	public void Save(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(UseCal));
//		using(XmlWriter writer = XmlWriter.Create(path))
//		{
//			serializer.Serialize(writer,this);
//		}

		#if UNITY_WINRT
		
		XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
		
		serializer.Serialize(writer,this);
		
		#else
		
		using(XmlWriter writer = XmlTextWriter.Create(path))
		{
			serializer.Serialize(writer,this);
		}
		
		
		#endif

	}
	
	public static UseCal Load (TextAsset file)
	{
//		if (File.Exists(path))
		if (file != null)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(UseCal));
//			using (XmlReader reader = XmlReader.Create(path))
			StringReader strReader = new StringReader(file.ToString());
			using (	XmlReader reader = XmlReader.Create(strReader))
			{
				Debug.Log("File Found.");
				return serializer.Deserialize(reader) as UseCal;
			}
		}
		else
		{
			Debug.Log("File Not Found.");
			return new UseCal();
		}
	}
}
public class ReadUseCal : MonoBehaviour {
	public GameObject SuggestDetail;
	public DietarySaveAndLoadData snl;
	//public string filePath = Path.Combine(Application.dataPath, "UseCal.xml");
	public List<GameObject> searchList = new List<GameObject>();
	public UseCal fileXml;
	public List<act> actListData = new List<act>();

	public Collider2D mbox;
	private bool IsTouch = false;
	private Vector2 lastTouch;
	public float ItemMoveMent = 0.0f;
	private float MoveSpeed = 0.0f;
	private float currentMoveSpeed = 0.0f;
	public float Sensitive = 10.0f;
	public float Smooth = 1.0f;
	private float height = 0;
	private MorphObject SnappingMorph = new MorphObject ();

	// Use this for initialization
	void Start () {
		TextAsset m_XmlAsset = (TextAsset)Resources.Load<TextAsset>("UseCal");
//		fileXml = UseCal.Load(Application.streamingAssetsPath+"/UseCal.xml");
		fileXml = UseCal.Load(m_XmlAsset);
		Debug.Log("datapath : "+Application.dataPath);
		Debug.Log("streamingAssetsPath : "+Application.streamingAssetsPath);
		if (fileXml.acts != null){
			for(int i=0;i<fileXml.acts.Length;i++){
				GameObject itemDetail = (GameObject)GameObject.Instantiate(SuggestDetail);
				itemDetail.SetActive(true);
				itemDetail.transform.parent = this.transform;
				itemDetail.transform.localPosition = new Vector3 (0, 2.8f - i * 0.68f, 0);
				
				SuggestDetail idt = itemDetail.GetComponent<SuggestDetail>();
				idt.setText(fileXml.acts[i].actName,fileXml.acts[i].calName);
				searchList.Add (itemDetail);
			}
		}

	}

	// Update is called once per frame
	void Update () {
		bool touchedDown = TouchInterface.GetTouchDown ();
		bool touchedUp = TouchInterface.GetTouchUp ();
		Vector2 touchPos = TouchInterface.GetTouchPosition ();
		if (touchedDown) {
			if(mbox.OverlapPoint(touchPos)){
				IsTouch = true;
				lastTouch = touchPos;
			}
		}
		if (touchedUp) 
		{
			if (IsTouch) {
				IsTouch = false;
			}
		}
		ContentUpdate ();
	}
	void ContentUpdate () {
		if (IsTouch) {
			Vector2 touchPos = TouchInterface.GetTouchPosition ();
			//calculate last touch
			ItemMoveMent += (touchPos.y - lastTouch.y) * (Sensitive/10.0f);
			MoveSpeed = (touchPos.y - lastTouch.y)* (Sensitive/10.0f) / (Time.deltaTime);
			//Debug.Log(ItemMoveMent.ToString()+" "+height.ToString()+" "+IsDrag);
			currentMoveSpeed = MoveSpeed;
			if (ItemMoveMent < 0.0f)
				ItemMoveMent = 0.0f;
			if (ItemMoveMent > 20.0f)
				ItemMoveMent = 20.0f;
			lastTouch = touchPos;
			SnappingMorph.morphEasein (ItemMoveMent, ItemMoveMent+currentMoveSpeed, Mathf.Abs(MoveSpeed)/10.0f + Smooth*30.0f);
			if(currentMoveSpeed!=0){
				//IsDrag = true;
			}
			//move all item
			transform.localPosition = new Vector3(0,ItemMoveMent,0);
			//			}
		}
	}
}
