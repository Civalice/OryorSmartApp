using UnityEngine;
using System.Collections;
namespace Game4
{

	public class GoldStarItemObject : StarItemObject {
		bool IsGold = false;
		float timer = 0;

		float lenPosition;
		void Awake()
		{
			Debug.Log("GoldStar");
			PreAwake();
//			SetGoldStar(2.0f);
		}

		public void SetGoldStar(float _timer)
		{
			lenPosition = this.transform.localPosition.x;
			IsGold = true;
			timer = _timer;
			//set house and choose house to throw this star
			Game4Global.ForceSpawnHouse(this);
			StopCoroutine("CountTimer");
			StartCoroutine("CountTimer");
		}
		public override int GetScore()
		{
			//Get Star Score
			return Game4_LvlingStat.GetGoldStarScore();
		}

		IEnumerator CountTimer()
		{
			GetComponent<Collider2D>().enabled = false;
			while (timer > 0)
			{
				timer -= TurtleCharacter.currentSpeed/60.0f;
				yield return null;
			}
			GetComponent<Collider2D>().enabled = true;
			MainSoundSrc.PlaySound("ThrowItem");
			GetComponent<Animator>().Play("StarPop");
			//play throw out animation
			Vector2 targetPos = this.transform.position;
			while (Mathf.Abs(this.transform.position.x - lenPosition) > 0.1f)
			{
				Vector2 pos = this.transform.position;
				targetPos = this.transform.position;
				targetPos.x = lenPosition;
				pos = Vector2.Lerp(pos,targetPos,Time.deltaTime*10);
				this.transform.position = pos;
				yield return null;
			}
			targetPos.x = lenPosition;
			this.transform.position = targetPos;
		}
	}
}