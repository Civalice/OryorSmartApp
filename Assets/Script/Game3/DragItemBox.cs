using UnityEngine;
using System.Collections;
namespace Game3
{
	public class DragItemBox : MonoBehaviour {
		public ParashootType type;
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
			Vector2 pos = TouchInterface.GetTouchPosition();
			if (GetComponent<Collider2D>().OverlapPoint(pos))
			{
				Game3Global.pGlobal.boxObject = this;
			}
			else if (Game3Global.pGlobal.boxObject == this)
			{
				Game3Global.pGlobal.boxObject = null;
			}
		}
	}
}