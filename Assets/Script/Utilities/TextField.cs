using UnityEngine;
using System.Collections;
using TMPro;

public class TextField : MonoBehaviour {
	private Collider2D box;
	private TextMeshPro tmPro;
	public TouchScreenKeyboardType type;
	public bool multiline = false;

	public bool IsPassword = false;

	public string text{
		get{return currentText;}
		set{setText(value);}
	}

	private TouchScreenKeyboard keyboard;
	private string currentText = "";
	private AudioSource SoundClick;

	// Use this for initialization
	void Start () {
		box = GetComponent<Collider2D> ();
		tmPro = GetComponent<TextMeshPro> ();
		SoundClick = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (LoadingScript.IsLoading) return;
		if (PopupObject.IsPopup) return;
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
		if (keyboard.done) {
			setText(keyboard.text);
		}
		if (keyboard.done || keyboard.wasCanceled) {
			setText(currentText);
			keyboard = null;
		}
	}
#if UNITY_EDITOR
	bool IsKeyboardShow = false;
	string stringToEdit = "";
	
	void OnGUI() {
		Event e = Event.current;
		if (e.keyCode == KeyCode.Return)
						IsKeyboardShow = false;
				else if (IsKeyboardShow) {
			{
				stringToEdit = GUI.TextField (new Rect (0, 0, 200, 50), stringToEdit, 25);
				setText(stringToEdit);
			}
		}
	}
#endif
	void setText (string text)
	{
		currentText = text;
#if UNITY_EDITOR
		stringToEdit = currentText;
#endif
		string tmp = "";
		if (IsPassword)
		{
			if (text != null)
			{
				for (int i = 0;i < text.Length;i++)
					tmp += "*";
			}
		}
		else
		{
			if (text != null)
			{
				for (int i = 0;i < text.Length;i++)
				{
					tmp += text[i];
					if (text[i] != ' ')
						tmp += " ";
				}
			}
//				tmp = text.Replace (" ", "  ");
		}
		tmPro = GetComponent<TextMeshPro> ();
		tmPro.text = tmp;
	}
}
