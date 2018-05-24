using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

#if UNITY_WP8

#else 

using System.Runtime.Serialization.Formatters.Binary;
#endif

using System.IO;

[System.Serializable]
public class Food{
	public int FoodID;
	public string FoodName;
	public int reciveCal;
	public bool[] selCat = new bool[10];
	public long saveDate = DateTime.Now.Ticks;
}
public class DietarySaveAndLoadData : MonoBehaviour {
	public List<Food> RecordList =  new List<Food>();
	public List<Food> emptyList =  new List<Food>();
	private Food emptyFood = new Food();
	
	public TextMesh textDebug;
	
	void Awake(){
		//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}
	// Use this for initialization
	void Start () {
		emptyFood.FoodID = 0;
		emptyFood.FoodName = "";
		emptyFood.reciveCal = 0;
		//emptyFood.selCat = 
		emptyList.Add (emptyFood);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void saveData(List<Food> RecordFood){
		//RecordFood.Add(SelectedFood);
		if(File.Exists(Application.persistentDataPath + "/DietaryCal.xml")) {
			#if UNITY_WINRT
			string path = Application.persistentDataPath + "/DietaryCal.xml";
			//			FileStream file = new FileStream  (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Write);
			byte [] bytes = UnityPluginForWindowsPhone.FileIO.SerializeObject(RecordFood);
			//			
			////			File.WriteAllBytes(file, bytes);
			string textData = Convert.ToBase64String (bytes);
			
			//	string  textData=	Newtonsoft.Json.JsonConvert.SerializeObject(RecordFood);
			writeFile(path , textData);
			//			fs.Close ();
			
			#else
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = new FileStream  (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Write);
			bf.Serialize(file, RecordFood);
			
			file.Close();
			#endif
		}
		else{
			#if UNITY_WINRT
			
			FileStream file = new FileStream (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Create);
			file.Close();
			string path = Application.persistentDataPath + "/DietaryCal.xml";
			//				string  textData=	Newtonsoft.Json.JsonConvert.SerializeObject(RecordFood);
			//					writeFile(path , textData);
			byte [] bytes = UnityPluginForWindowsPhone.FileIO.SerializeObject(RecordFood);
			
			//				//	File.WriteAllBytes(file, bytes);
			//				string textData = Encoding.UTF8.GetString(bytes , 0 ,bytes.Length);
			
			string textData = 	Convert.ToBase64String (bytes);
			
			
			
			
			writeFile(path , textData);
			
			
			#else
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = new FileStream (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Create);
			file.Close();
			
			file = new FileStream  (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Write);
			bf.Serialize(file, emptyList);
			file.Close();
			#endif
			
		}
	}
	
	public List<Food> loadData(){
		
		if(File.Exists(Application.persistentDataPath + "/DietaryCal.xml")) {
			if(RecordList!=null){RecordList.Clear ();}
			
			#if UNITY_WINRT
			string path = Application.persistentDataPath + "/DietaryCal.xml";
			//			FileStream file = new FileStream (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Read);
			//			byte [] btyes = File.ReadAllBytes(file);
			
			string line = readFile(path );
			
			
			//			RecordList =Newtonsoft.Json.JsonConvert.DeserializeObject<List<Food>>(line);
			//			byte[] bArray = Encoding.UTF8.GetBytes(line );
			//
			
			//
			RecordList = UnityPluginForWindowsPhone.FileIO.DeserializeObject<List<Food>>(line);
			if(RecordList == null){
				
				RecordList = new List<Food>();
				return RecordList;
			}
			//			textDebug.text = "can serialize ";
			//			file.Close();
			#else
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = new FileStream (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Read);
			RecordList = (List<Food>)bf.Deserialize(file);
			file.Close();
			#endif
			
			
		}
		else{
			
			
			
			#if UNITY_WINRT
			
			
			FileStream file = new FileStream (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Create);
			file.Close();
			
			string path = Application.persistentDataPath + "/DietaryCal.xml";
			
			
			
			
			
			//			file = new FileStream  (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Write);
			
			//			byte [] bytes = UnityPluginForWindowsPhone.FileIO.SerializeObject(emptyList);
			//			textDebug.text = bytes.ToString();
			//			string textData = Convert.ToBase64String(bytes);
			//			string textData = Newtonsoft.Json.JsonConvert.SerializeObject(emptyList);
			//			writeFile(path , textData);
			
			
			//			file.Close();
			
			#else
			
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = new FileStream (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Create);
			file.Close();
			
			file = new FileStream  (Application.persistentDataPath + "/DietaryCal.xml", FileMode.Open, FileAccess.Write);
			bf.Serialize(file, emptyList);
			file.Close();
			#endif
			
			
		}
		return RecordList;
	}
	
	private void writeFile(string path , string data){
		
		using (StreamWriter sw = new StreamWriter(path)) 
		{
			// Add some text to the file.
			sw.Write(data , 0 , data.Length);
			
		}
	}
	private string readFile(string path ){
		
		try 
		{
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader(path)) 
			{
				String line;
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
