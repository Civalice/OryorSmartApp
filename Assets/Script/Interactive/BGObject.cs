using UnityEngine;
using System.Collections;

public class BGObject : MonoBehaviour {
	public float Delay = 0;
	public float time = 60;
	public float noise = 20;
	
	private MorphObject BubbleVal;

	private float fixRotate;
	private float rotate;

	void Awake () {
		BubbleVal = new MorphObject ();
	}

	void Start () {
		fixRotate = transform.localRotation.eulerAngles.z;
		popup ();
		//Debug.Log ("BubbleScale = " + fixScale);
	}
	
	// Update is called once per frame
	void Update () {
			BubbleVal.Update ();
			transform.localRotation = Quaternion.Euler (0, 0, BubbleVal.val+fixRotate);
	}
	
	public void popup(){
		BubbleVal.morphBubble (0, 0, noise, time, Delay);
	}
}
