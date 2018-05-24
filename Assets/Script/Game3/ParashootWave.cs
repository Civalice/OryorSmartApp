using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game3
{
	public class ParashootWave : MonoBehaviour {
		DragLvling lvling;
		int parashootCount;
		int parashootClear;
		float timer;
		GameObject parentObj;
		bool IsPerfect = true;
		void Awake()
		{
			lvling = Game3_LvlingStat.GetLvling();
			IsPerfect = true;
		}

		public void SetupWave(GameObject obj)
		{
			if (!this) return;
			timer = 0;
			parentObj = obj;
			transform.parent = parentObj.transform;
			transform.localPosition = new Vector2(0,0);
			parashootCount = lvling.ParashootCount;
			parashootClear = parashootCount;
		}

		public void CheckPerfectWave()
		{
			if (parashootClear <= 0)
			{
				if (IsPerfect)
				{
					MainSoundSrc.PlaySound("right");
					Game3Global.BirdOK();
					Game3Global.AddScore(10000);
				}
				Destroy (gameObject);
			}
		}

		public void ParashootClear(bool flag = true)
		{
			IsPerfect &= flag;
			parashootClear--;
			CheckPerfectWave();
		}

		public void Update()
		{
			if (Game3Global.GetGameState() == GameState.GS_PLAY)
			{
				//countdown Timer
				if (parashootCount > 0)
				{
					timer -= Time.deltaTime;
					if (timer <= 0)
					{
						timer += Random.Range(lvling.minParashootLength,lvling.maxParashootLength);
						float randPos = Random.Range(-2.0f,2.0f);
						Vector2 spawnPos = parentObj.transform.position;
						spawnPos.x += randPos;
						//spawn Parashoot
						GameObject obj = GameObject.Instantiate(Game3Global.pGlobal.ParashootPrefabs) as GameObject;
						obj.transform.position = spawnPos;
						obj.transform.parent = parentObj.transform;
						Parashoot pComp = obj.GetComponent<Parashoot>();
						//random Att
						ParashootType ran = (ParashootType)Random.Range(0,lvling.CatagoryCount+1);
						float parashootSpeed = Random.Range(lvling.minParashootSpeed,lvling.maxParashootSpeed);
						pComp.SetParentWave(this);
						pComp.SetParashoot(ran);
						pComp.SetSpeed(parashootSpeed);
						
						parashootCount--;
					}
				}
			}
//			if (parashootCount <= 0)
//			{
//				Destroy (gameObject);
//			}
		}
	}
}