using UnityEngine;
using System.Collections;
using TMPro;

public class HPNotification : MonoBehaviour {
	public TextMeshPro numberText;
	Vector3 popupPoint;
	float movementPosY;
	// Use this for initialization
	void Awake () {
	}

	public void DecreaseLife(int life)
	{
		numberText.text = "-"+life.ToString ();
		popupPoint = transform.localPosition;
		movementPosY = 0;
		//start PopupText
		StartCoroutine ("TextPopup");
	}

	IEnumerator TextPopup()
	{
		Vector3 pts = popupPoint;
		Vector3 dest = popupPoint;
		dest.y += 1.5f;
		while (Vector3.Distance(pts,dest) > 0.05f) {
			pts = Vector3.Lerp(pts,dest,Time.deltaTime*2);
			transform.localPosition = pts;
			yield return null;
		}
		Destroy (this.gameObject);
		yield return null;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
