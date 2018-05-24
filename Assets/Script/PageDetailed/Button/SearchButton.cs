using UnityEngine;
using System.Collections;

public class SearchButton : MonoBehaviour {
	private TouchScreenKeyboard keyboard;
	private AudioSource SoundClick;
	public TouchScreenKeyboardType type;
	public bool multiline = false;
	public int maxLine = 3;
	private string currentText = "";
	// Use this for initialization
	void Start () {
		SoundClick = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (LoadingScript.IsLoading) return;
		if (GetComponent<Collider2D>() == null)
			return;
		bool touchDown = TouchInterface.GetTouchDown ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		
		if (touchDown) {
			if (GetComponent<Collider2D>().OverlapPoint(pos))
			{
				//show keyboard
				if(SoundClick!=null)SoundClick.Play();
				PageDetailGlobal.state= DetailState.DS_SEARCH;
				#if UNITY_EDITOR
				IsKeyboardShow = true;
				#else
					#if !UNITY_WEBGL && !DISABLE_WEBVIEW
					if (!TouchScreenKeyboard.visible)
						keyboard = TouchScreenKeyboard.Open(currentText,type,false,multiline,false,false);
					#endif
				#endif
			}
		
		}
		if (keyboard == null)
			return;
		else if (keyboard.done)
		{
			if(keyboard.text!=""){
				//Search
				Debug.Log("string = "+keyboard.text);
				string toSearch = keyboard.text;
				Search(toSearch);
				keyboard = null;
			}
			PageDetailGlobal.state= DetailState.DS_LIST;
		}
		else if (keyboard.wasCanceled)
		{
			PageDetailGlobal.state= DetailState.DS_LIST;
			keyboard = null;
		}
	}

	#if UNITY_EDITOR
	bool IsKeyboardShow = false;
	string stringToEdit = "";
	
	void OnGUI() {
		if (PageDetailGlobal.state == DetailState.DS_SEARCH)
		{
			Event e = Event.current;
			if (e.keyCode == KeyCode.Return)
			{
				IsKeyboardShow = false;
				PageDetailGlobal.state= DetailState.DS_LIST;
				Debug.Log(stringToEdit);
				string toSearch = stringToEdit;
				Search(toSearch);
			}
			else if (IsKeyboardShow) {
				stringToEdit = GUI.TextField (new Rect (0, 0, 100, 50), stringToEdit, 25);
			}
		}
	}
	#endif

	public void Search(string filter)
	{
		PageDetailGlobal.DownloadNewFilter (filter);
	}
}
