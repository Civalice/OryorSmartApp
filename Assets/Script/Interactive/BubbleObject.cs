using UnityEngine;
using System.Collections;

enum BubbleState
{
	BS_HIDE = 0,
	BS_BUBBLE,
	BS_FIX
};

public class BubbleObject : MonoBehaviour {
	public float Delay = 0;

	private MorphObject BubbleVal;
	private MorphObject ScaleVal;

	private float fixScale;
	private float scale;
	private BubbleState state;
	// Use this for initialization
	private ButtonObject[] buttonObjList;
	void Start () {
				buttonObjList = GetComponents<ButtonObject> ();
				BubbleVal = new MorphObject ();
				ScaleVal = new MorphObject ();
				state = BubbleState.BS_HIDE;
				scale = 0;
				fixScale = transform.localScale.x;
				transform.localScale = new Vector3 (scale, scale, 1);
				popup ();
				//Debug.Log ("BubbleScale = " + fixScale);
		}
	
	// Update is called once per frame
	void Update () {
				switch (state) {
				case BubbleState.BS_HIDE:
						{
								transform.localScale = new Vector3 (scale, scale, 1);
						}
						break;
				case BubbleState.BS_BUBBLE:
						{
								ScaleVal.Update ();
								BubbleVal.Update ();
								scale = ScaleVal.val;
								transform.localScale = new Vector3 (ScaleVal.val, ScaleVal.val, 1);
								transform.rotation = Quaternion.Euler (0, 0, BubbleVal.val);
								if (ScaleVal.IsFinish () && BubbleVal.IsFinish ()) {
										setButton (true);
										state = BubbleState.BS_FIX;
								}
						}
						break;
				case BubbleState.BS_FIX:
						{
								transform.localScale = new Vector3 (scale, scale, 1);
						}
						break;
				}
		}

	public void hide(){
				state = BubbleState.BS_HIDE;
				scale = 0;
				setButton (false);
		}

	public void popup(){

				BubbleVal.morphBubble (0, 0, 20, 60, Delay);
				ScaleVal.morphEasein (0, fixScale, 15, Delay);

				state = BubbleState.BS_BUBBLE;
				setButton (false);
		}

	void setButton (bool _enable){
				if (buttonObjList == null)
						return;
		
				foreach (ButtonObject bObj in buttonObjList) {
						if (_enable)
								bObj.Enable ();
						else
								bObj.Disable ();
				}
		}
	public bool IsReady() {
		if (BubbleVal.IsFinish () && ScaleVal.IsFinish ())
						return true;
				else
						return false;
	}
}
