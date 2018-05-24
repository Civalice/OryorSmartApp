using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game4 
{
	public delegate void SpawnEvent(ItemCD obj);

	public class ItemCD
	{
		public SpawnEvent eventAction;
		float cooldown;
		bool IsDone = true;

		public void SetCooldown(float initialCD)
		{
			cooldown = initialCD;
			IsDone = false;
		}

		public void UpdateCooldown(float diff)
		{
			if (IsDone) return;
			cooldown -= diff;
			if (cooldown < 0)
			{
				IsDone = true;
				//do something
				eventAction(this);
			}
		}

		public void Delayed(float t)
		{
			if (cooldown < t) 
				cooldown += t;
		}
	}
	public class Spawner : MonoBehaviour {
		public GameObject[] ObstructPrefabs;
		public GameObject StarPrefabs;
		public GameObject GoldStarPrefabs;
		public GameObject BoosterPrefabs;

		ItemCD[] obsList;
		ItemCD starItem;
		ItemCD goldStarItem;
		ItemCD boosterItem;

		int obsCount = 2;

		GameObject rootParent;
		//if Obs is spawn we have to delay Item for 100point
		//if Item is spawn we have to delay obs for 100point too

		void Awake () {
			rootParent = new GameObject("Root");
			rootParent.transform.parent = transform;
			obsList = new ItemCD[2];
			obsList[0] = new ItemCD();
			obsList[1] = new ItemCD();
			starItem = new ItemCD();
			goldStarItem = new ItemCD();
			boosterItem = new ItemCD();

			TurtleLvling lvling = Game4_LvlingStat.GetLvling();

			obsList[0].eventAction = SpawnObstructcle;
			obsList[1].eventAction = SpawnObstructcle;

			obsList[0].SetCooldown(Random.Range(0,lvling.ObstructcleLength*lvling.RandomNoise/100.0f));
			obsList[1].SetCooldown(Random.Range(0,lvling.ObstructcleLength*lvling.RandomNoise/100.0f));

			starItem.eventAction = SpawnStar;
			starItem.SetCooldown(Random.Range(0,lvling.StarLength*lvling.RandomNoise/100.0f));

			goldStarItem.eventAction = SpawnGoldStar;
			goldStarItem.SetCooldown(Random.Range(lvling.GoldStarLength/100.0f,lvling.GoldStarLength*lvling.RandomNoise/100.0f));

			boosterItem.eventAction = SpawnBooster;
			boosterItem.SetCooldown(Random.Range(lvling.BoosterLength/100.0f,lvling.BoosterLength*lvling.RandomNoise/100.0f));
		}
		// Use this for initialization
		void Start () {
			
		}

		public void Clear()
		{
			if (rootParent != null)
				Destroy(rootParent);
			rootParent = new GameObject("Root");
			rootParent.transform.parent = transform;
		}

		// Update is called once per frame
		void Update () {
			if (Game4Global.GlobalPause) return;
			float cd = TurtleCharacter.currentSpeed/60.0f;
			for (int i = 0;i < obsCount;i++)
			{
				obsList[i].UpdateCooldown(cd);
			}
			starItem.UpdateCooldown(cd);
			goldStarItem.UpdateCooldown(cd);
			boosterItem.UpdateCooldown(cd);
		}

		void SpawnBooster(ItemCD item)
		{
			TurtleLvling lvling = Game4_LvlingStat.GetLvling();

			//spawn Booster
			
			GameObject obj = GameObject.Instantiate(BoosterPrefabs) as GameObject;
			obj.SetActive(true);
			obj.transform.position = new Vector3(-1.25f+Random.Range(0,3)*1.25f,6,0);
			obj.transform.parent = rootParent.transform;
			//Delayed other type cooldown]
			for (int i = 0;i < obsCount;i++)
			{
				obsList[i].Delayed(2.0f);
			}
			starItem.Delayed(2.0f);
			goldStarItem.Delayed(2.0f);
			
			//reset item CD
			item.SetCooldown(Random.Range(lvling.BoosterLength/100.0f,lvling.BoosterLength*lvling.RandomNoise/100.0f));
		}

		void SpawnGoldStar(ItemCD item)
		{
			TurtleLvling lvling = Game4_LvlingStat.GetLvling();

			//spawn GoldStar
			
			GameObject obj = GameObject.Instantiate(GoldStarPrefabs) as GameObject;
			GoldStarItemObject gItem = obj.GetComponent<GoldStarItemObject>();
			obj.SetActive(true);
			obj.transform.position = new Vector3(-1.25f+Random.Range(0,3)*1.25f,6,0);
			obj.transform.parent = rootParent.transform;
			gItem.SetGoldStar(Random.Range(2.0f,4.0f));
			//Delayed other type cooldown]
			for (int i = 0;i < obsCount;i++)
			{
				obsList[i].Delayed(2.0f);
			}
			starItem.Delayed(2.0f);
			boosterItem.Delayed(2.0f);
			//reset item CD
			item.SetCooldown(Random.Range(lvling.GoldStarLength/100.0f,lvling.GoldStarLength*lvling.RandomNoise/100.0f));
		}

		void SpawnStar(ItemCD item)
		{
			TurtleLvling lvling = Game4_LvlingStat.GetLvling();

			//spawn Star
			
			GameObject obj = GameObject.Instantiate(StarPrefabs) as GameObject;
			obj.SetActive(true);
			obj.transform.position = new Vector3(-1.25f+Random.Range(0,3)*1.25f,6,0);
			obj.transform.parent = rootParent.transform;
			//Delayed other type cooldown]
			for (int i = 0;i < obsCount;i++)
			{
				obsList[i].Delayed(2.0f);
			}
			goldStarItem.Delayed(2.0f);
			boosterItem.Delayed(2.0f);
			//reset item CD
			item.SetCooldown(Random.Range(lvling.StarLength/100.0f,lvling.StarLength*lvling.RandomNoise/100.0f));
		}

		void SpawnObstructcle(ItemCD item)
		{
			TurtleLvling lvling = Game4_LvlingStat.GetLvling();
			obsCount = lvling.ObstructcleNumber;
			//spawn Obstructcle
			
			GameObject obj = GameObject.Instantiate(ObstructPrefabs[Random.Range(0,ObstructPrefabs.Length)]) as GameObject;
			obj.SetActive(true);
			obj.transform.position = new Vector3(-1.25f+Random.Range(0,3)*1.25f,6,0);
			obj.transform.parent = rootParent.transform;
			//Delayed other type cooldown
			starItem.Delayed(2.0f);
			goldStarItem.Delayed(4.0f);
			boosterItem.Delayed(2.0f);
			//reset item CD
			item.SetCooldown(Random.Range(lvling.ObstructcleLength/100.0f,lvling.ObstructcleLength*lvling.RandomNoise/100.0f));
		}
	}
}