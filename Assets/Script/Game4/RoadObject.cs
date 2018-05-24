using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Game4
{
	public class RoadObject : MonoBehaviour {
		//road height size = 156 px
		const int roadHeight = 156;
		public GameObject roadGameObject;


		public float roadSpeed = 1.0f;

		float roadPosition = 0.0f;
		Vector3 roadVector = new Vector3(0,0,0);
		List<GameObject> roadList = new List<GameObject>();
		void Awake()
		{
			//spawn from top and keep destroy
			//calculate size to used
			//start pixel = +860 to -860
			int roadsize = 860*2/roadHeight;
			for(int i = 0;i<roadsize;i++)
			{
				GameObject tmp = GameObject.Instantiate(roadGameObject) as GameObject;
				tmp.transform.parent = this.transform;
				tmp.transform.localPosition = new Vector3(0,8.6f-((float)roadHeight/100)*i,0);
				tmp.SetActive(true);
				roadList.Add(tmp);
			}
		}
		// Use this for initialization
		void Start () {
		
		}

		void FixedUpdate() {
			if (Game4Global.GetGameState() != GameState.GS_PLAY) 
				return;
			//calculate roadSpeed
			roadSpeed = TurtleCharacter.currentSpeed;
			roadPosition += roadSpeed/60.0f;
			while(roadPosition > roadHeight/100.0f)
			{
				roadPosition -= roadHeight/100.0f;
			}
			
			roadVector.y = -roadPosition;
		}


		// Update is called once per frame
		void Update () {
			transform.localPosition = roadVector;
		}
	}
}
