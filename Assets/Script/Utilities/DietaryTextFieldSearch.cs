using UnityEngine;
using System.Collections;
using TMPro;

public class DietaryTextFieldSearch : MonoBehaviour {
	public ControlAddEvent cae;
	private Collider2D box;
	private TextMeshPro tmPro;
	public TouchScreenKeyboardType type;
	public bool multiline = false;
	public int maxLine = 3;
	private TouchScreenKeyboard keyboard;
	private string currentText = "";
	private AudioSource SoundClick;
	// Use this for initialization
	void Start () {
		box = GetComponent<Collider2D> ();
		tmPro = GetComponent<TextMeshPro> ();
		SoundClick = GetComponent<AudioSource>();
		tmPro.maxVisibleLines = maxLine;
	}
	
	// Update is called once per frame
	void Update () {
		if (LoadingScript.IsLoading) return;
		if (box == null)
			return;
		bool touchDown = TouchInterface.GetTouchDown ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		
		if (touchDown) {
			if (box.OverlapPoint(pos))
			{
				//show keyboard
				if(SoundClick!=null)SoundClick.Play();
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
		if (keyboard.active) {
			setText(keyboard.text);
		}
		if (keyboard.done || keyboard.wasCanceled) {
			keyboard = null;
			cae.IsSearchInput = true;
			cae.selectingCat(0);
		}
	}
	#if UNITY_EDITOR
	bool IsKeyboardShow = false;
	string stringToEdit = "";
	
	void OnGUI() {
		Event e = Event.current;
		if (e.keyCode == KeyCode.Return){
			IsKeyboardShow = false;
			cae.IsSearchInput = true;
			cae.selectingCat(0);
		}
		else if (IsKeyboardShow) {
			stringToEdit = GUI.TextField (new Rect (0, 0, 100, 50), stringToEdit, 25);
			setText(stringToEdit);
		}
	}
	#endif
	void setText (string text)
	{
		currentText = text;
		string tmp = text.Replace (" ", "  ");
		tmPro.text = tmp;
		tmPro.maxVisibleLines = maxLine;
	}
}
