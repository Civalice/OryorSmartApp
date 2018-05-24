using UnityEngine;
using System.Collections;
using TMPro;

public class ComboUI : MonoBehaviour {
	public SpriteRenderer comboRenderer;
	public TextMeshPro tmPro;
	private Color tmColor;
	// Use this for initialization
	void Start () {
		tmColor = tmPro.color;
	}

	public void setCombo(int combo)
	{
		Animation anim = GetComponent<Animation> ();
		if (combo == 0) {
			tmPro.text = "" + combo;
			anim.Play ("ComboFade");
		} else {
				tmPro.text = "" + combo;
				anim ["ComboAnim"].time = 0;
				anim.Play ("ComboAnim");
		}
	}

	// Update is called once per frame
	void Update () {
		tmPro.color = new Color(tmColor.r,tmColor.g,tmColor.b,comboRenderer.color.a); 
	}
}
