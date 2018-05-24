using UnityEngine;
using System.Collections;
namespace Game4
{
	public class SideWaySpawner : MonoBehaviour {
		const float ObjectLength = 1.2f;
		public GameObject SideWayPrefabs;
		public Sprite[] SideWayForestList;
		public Sprite[] SideWayHouseList;

		public float TravelLength = 0;
		GoldStarItemObject goldStar;
		bool IsForceSpawnHouse = false;
		void Awake () {
			goldStar = null;
			IsForceSpawnHouse = false;
			for (int i = 0;i < 10;i++)
			{
				int wayType = Random.Range(0,4);
				GameObject objR = GameObject.Instantiate(SideWayPrefabs) as GameObject;
				objR.transform.parent = this.transform;
				objR.transform.localPosition = new Vector2(2.88f,i*-ObjectLength);
				GameObject objL = GameObject.Instantiate(SideWayPrefabs) as GameObject;
				objL.transform.parent = this.transform;
				objL.transform.localPosition = new Vector2(-2.88f,i*-ObjectLength);
				objL.transform.localScale = new Vector2(-1,1);
				switch(wayType)
				{
				case 0:
				{
					//forest both
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
				}break;
				case 1:
				{
					//left house forest right
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
				}break;
				case 2:
				{
					//right house forest left
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
				}break;
				case 3:
				{
					//both house
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
				}break;
				}
			}
		}
		// Use this for initialization
		void Start () {
			
		}

		public void Clear()
		{
			goldStar = null;
		}

		public void ForceSpawnHouse(GoldStarItemObject star)
		{
			IsForceSpawnHouse = true;
			goldStar = star;
		}

		// Update is called once per frame
		void FixedUpdate () {
			if (Game4Global.GlobalPause) return;
			TravelLength += TurtleCharacter.currentSpeed/60.0f;
			if (TravelLength > ObjectLength)
			{
				int wayType = Random.Range(0,4);
				if (IsForceSpawnHouse)
				{
					IsForceSpawnHouse = false;
					wayType = Random.Range(1,4);
					Debug.Log("way Type = "+wayType);
				}

				TravelLength -= ObjectLength;
				GameObject objR = GameObject.Instantiate(SideWayPrefabs) as GameObject;
				objR.transform.parent = this.transform;
				objR.transform.localPosition = new Vector2(2.88f,-TravelLength);
				GameObject objL = GameObject.Instantiate(SideWayPrefabs) as GameObject;
				objL.transform.parent = this.transform;
				objL.transform.localPosition = new Vector2(-2.88f,-TravelLength);
				objL.transform.localScale = new Vector2(-1,1);

				switch(wayType)
				{
				case 0:
				{
					//forest both
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
				}break;
				case 1:
				{
					//left house forest right
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
					if (goldStar != null)
					{
						objL.GetComponent<SideRodeObject>().SetThrowStar(goldStar);
						goldStar = null;
					}
				}break;
				case 2:
				{
					//right house forest left
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayForestList[Random.Range(0,SideWayForestList.Length)]);
					if (goldStar != null)
					{
						objR.GetComponent<SideRodeObject>().SetThrowStar(goldStar);
						goldStar = null;
					}
				}break;
				case 3:
				{
					//both house
					objR.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
					objL.GetComponent<SideRodeObject>().SetSidewayObject(SideWayHouseList[Random.Range(0,SideWayHouseList.Length)]);
					//random choose objLR
					if (goldStar != null)
					{
						bool side = Random.Range(0,100)<50;
						if (side)
							objL.GetComponent<SideRodeObject>().SetThrowStar(goldStar);
						else
							objR.GetComponent<SideRodeObject>().SetThrowStar(goldStar);
						goldStar = null;
					}
				}break;
				}
			}
		}
	}
}