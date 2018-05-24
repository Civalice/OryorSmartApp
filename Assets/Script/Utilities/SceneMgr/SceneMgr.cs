using UnityEngine;
using System.Collections;

public class SceneMgr : MonoBehaviour {

	static GameObject NextScene;
	//private AsyncOperation sceneLoadingOperation = null;

	public static void ChangeScene(string name) { 
		//set next scene name here
		LoadingScript.ChangeScene(name);
		Debug.Log ("Change Scene : " + name);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		}
}
