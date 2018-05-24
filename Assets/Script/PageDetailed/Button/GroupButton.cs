using UnityEngine;
using System.Collections;

public class GroupButton : MonoBehaviour {
	public GameObject ButtonSpriteNormal;
	public GameObject ButtonSpritePress;
	public delegate void ButtonAction();
	public event ButtonAction OnPressed;
	public bool IsPress = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Press()
	{
		if (IsPress)
			return;
		IsPress = true;
		ButtonSpritePress.SetActive (true);
		ButtonSpriteNormal.SetActive (false);
		if (OnPressed != null) {
			OnPressed();
		}
	}

	public void Release()
	{
		IsPress = false;
		ButtonSpritePress.SetActive (false);
		ButtonSpriteNormal.SetActive (true);
	}
}
