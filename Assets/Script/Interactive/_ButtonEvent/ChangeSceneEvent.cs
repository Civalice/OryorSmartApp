using UnityEngine;
using System.Collections;
[RequireComponent (typeof(ButtonObject))]
public class ChangeSceneEvent : MonoBehaviour {

	private ButtonObject btObj;
//	public Object scene;
	public string SceneName;
	public MainMenuSound mSound;
	private AudioSource SoundClick;

	// Use this for initialization
	void Start () {
		btObj = GetComponent<ButtonObject> ();
		SoundClick = GetComponent<AudioSource>();
		if (btObj != null) {
			btObj.OnReleased += ChangeScene;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void preChangeScene(){
	}

	void ChangeScene() {
		if (SceneName.Length == 0) {
			Debug.Log("Scene not found");
			return;
		}
		//mSound.playSound("click");
		mSound = GetComponent<MainMenuSound>();
		if(mSound != null){
			mSound.playSound("click");
		}
		if(SoundClick!=null){
			SoundClick.Play();
		}
		preChangeScene ();
//		Debug.Log ("ChangeScene OnReleaseEvent");
		SceneMgr.ChangeScene (SceneName);
	}
}
