using UnityEngine;
using System.Collections;
using TMPro;

public class CheckTextField : MonoBehaviour {
	private Collider2D box;
	private TextMeshPro tmPro;
	public TouchScreenKeyboardType type;
	public bool multiline = false;
	public int maxLine = 3;
	private TouchScreenKeyboard keyboard;
	private string currentText = "";
	public CheckPageGlobal CheckGlobal;
	private AudioSource SoundClick;
	
	// Use this for initialization
	void Start () {
		box = GetComponent<Collider2D> ();
		tmPro = GetComponent<TextMeshPro> ();
		SoundClick = GetComponent<AudioSource>();
		tmPro.maxVisibleLines = maxLine;
		CheckGlobal = CheckPageGlobal.pGlobal;
	}
	
	// Update is called once per frame
	void Update () {
		if (box == null)
			return;
		bool touchDown = TouchInterface.GetTouchDown ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		
		if (touchDown) {
			if (box.OverlapPoint (pos)) {
				if (SoundClick != null)
					SoundClick.Play ();
				//show keyboard
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
		if (keyboard == null || keyboard.wasCanceled){
			return;
		}
		if (keyboard.done) {
//			keyboard = null;
			CheckPageGlobal.CheckProduct();
		}
//		if (keyboard.wasCanceled) {
//			keyboard = null;
//		}
		if (keyboard.active) {
			setText(keyboard.text);
		}
	}
	#if UNITY_EDITOR
	bool IsKeyboardShow = false;
	string stringToEdit = "";
	
	void OnGUI() {
		Event e = Event.current;
		if (e.keyCode == KeyCode.Return){
			IsKeyboardShow = false;
			CheckPageGlobal.CheckProduct();
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
		
		if (CheckPageGlobal.pGlobal.ProductType == 0) {
			
		} else if (CheckPageGlobal.pGlobal.ProductType == 1) {
			if (tmp.Length >= 5) {
				tmp = tmp.Insert (5, "-").Insert (3, "-").Insert (2, "-");
			} else if (tmp.Length >= 3) {
				tmp = tmp.Insert (3, "-").Insert (2, "-");
			} else if (tmp.Length >= 2) {
				tmp = tmp.Insert (2, "-");
			}
		} else if (CheckPageGlobal.pGlobal.ProductType == 2) {
			if (tmp.Length >= 1) {
				if (tmp [0].Equals ('ว')) {
					if (tmp.Length >= 3) {
						tmp = "วอส." + tmp.Substring (1).Insert (2, "/");
					} else {
						tmp = "วอส." + tmp.Substring (1);
					}
				} else {
					if (tmp.Length >= 2) {
						tmp = tmp.Insert (2, "/");
					}
				}
			}
		} else if (CheckPageGlobal.pGlobal.ProductType == 3) {
			if (tmp.Length >= 5) {
				tmp = tmp.Insert (5, "/");
			}
		} else if (CheckPageGlobal.pGlobal.ProductType == 4) {
			if (tmp.Length >= 9) {
				tmp = tmp.Insert (9, "-").Insert (8, "-").Insert (3, "-").Insert (2, "-");
			} else if (tmp.Length >= 8) {
				tmp = tmp.Insert (8, "-").Insert (3, "-").Insert (2, "-");
			} else if (tmp.Length >= 3) {
				tmp = tmp.Insert (3, "-").Insert (2, "-");
			} else if (tmp.Length >= 2) {
				tmp = tmp.Insert (2, "-");
			}
		}
		
		tmPro.text = tmp;
		tmPro.maxVisibleLines = 3;
	}
}
