using UnityEngine;
using System.Collections;
namespace Game4
{

	public class TurtleCharacter : MonoBehaviour {
		public static float maxSpeed;
		public static float currentSpeed;

		public GameObject Booster;
		public GameObject HPNortify;
		bool IsTouch = false;
		public Vector2 touchPos;
		Vector2 lastPos;
		//struggle variable
		bool IsStruggle = false;
		bool IsBoost = false;
		float boostTimer;
		float StruggleAnim;

		void Awake () {
			IsStruggle = false;
			Booster.SetActive(false);
		}
		// Use this for initialization
		void Start () {
			
		}

		void UpdatePosition()
		{
			if (IsTouch)
			{
				if (!IsStruggle)
				{
					//no velocity
	//				Vector2 currentPos = transform.position;
					Vector2 diff = touchPos - lastPos;
					lastPos = touchPos;
					diff.y = 0;
					GetComponent<Rigidbody2D>().velocity = diff*60;
				}
				else
				{
					lastPos = touchPos;
				}
			}
		}

		IEnumerator StruggleRun()
		{
			currentSpeed = 0;
			StruggleAnim = 0;
			//
			while (StruggleAnim < 1)
			{
				if(!Game4Global.GlobalPause)
				{
					StruggleAnim += Time.deltaTime;
				}
				yield return null;
			}
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			IsStruggle = false;
		}

		public void Damaged()
		{
			if (!IsStruggle)
			{
				//start Play Animation
				GetComponent<Animator>().Play("TurtleDamage");
				MainSoundSrc.PlaySound("Wrong");
				IsStruggle = true;
				StopCoroutine("StruggleRun");
				StartCoroutine("StruggleRun");
			}
		}

		public void Boost()
		{
			IsBoost = true;
			boostTimer = 5.0f;
			Booster.SetActive(true);
			StopCoroutine("BoostTimer");
			StartCoroutine("BoostTimer");
		}

		IEnumerator BoostTimer()
		{
			MainSoundSrc.PlaySound("airplane");
			while (boostTimer > 0)
			{
				boostTimer -= Time.deltaTime;
				yield return null;
			}
			IsBoost = false;
			Booster.SetActive(false);
		}

		// Update is called once per frame
		void FixedUpdate () {
			if (Game4Global.GetGameState() != GameState.GS_PLAY) return;
			//add Velocity
			if (!IsStruggle)
			{
				currentSpeed += maxSpeed/100.0f;
				if (!IsBoost)
				{
					if (currentSpeed > maxSpeed) 
						currentSpeed = maxSpeed;
				}
				else
				{
					currentSpeed = maxSpeed + Game4Global.pGlobal.BoosterSpeedPlus;
				}
			}
		}

		void Update()
		{
#if UNITY_EDITOR || UNITY_WEBPLAYER

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-5.0f,0);
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f,0);
			}

			if (Input.GetKeyUp(KeyCode.LeftArrow)||(Input.GetKeyUp(KeyCode.RightArrow)))
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			}
#endif
			Vector2 pos = TouchInterface.GetTouchPosition();
			bool isTouchDown = TouchInterface.GetTouchDown();
			bool isTouchUp = TouchInterface.GetTouchUp();
			if (isTouchDown)
			{
				IsTouch = true;
				lastPos = pos;
			}
			if (isTouchUp)
			{
				//calculate score
				GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				IsTouch = false;
			}
			if (IsTouch)
			{
				touchPos = pos;
			}
			UpdatePosition();

		}

		void OnCollisionEnter2D(Collision2D col)
		{
			if (col.gameObject.tag == "Obstructcle")
			{
				//add force too gameObject and delete that item
				GameObject gObj = col.gameObject;
				Obstructcle obs = gObj.GetComponent<Obstructcle>();
				Vector2 diff = transform.position - gObj.transform.position;
				diff.y = 0;
				if (!IsBoost)
				{
					if (!IsStruggle)
					{
						Damaged();
						GameObject hpObj = GameObject.Instantiate(HPNortify) as GameObject;
						hpObj.SetActive(true);
						hpObj.transform.position = this.transform.position;
						hpObj.GetComponent<HPNotification>().DecreaseLife(1);
						Game4Global.BrokeCombo();
						Game4Global.DecreaseLife();
					}
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					GetComponent<Rigidbody2D>().AddForce(diff * 100);
				}
				else
				{
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				}
				obs.Bounce(this.transform.position);
			}
		}

		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.tag == "Star")
			{
				//GetScoreFromStar
				GameObject gObj = other.gameObject;
				StarItemObject star = gObj.GetComponent<StarItemObject>();
				if (!star.IsEat)
				{
					int score = star.GetScore();
					star.Eat();
					Game4Global.AddScore(score);
					Game4Global.AddCombo();
					Game4_LvlingStat.AddEXP();
					//Eat Star
				}
			}
			if (other.tag == "Booster")
			{
				//Boost Speed
				GameObject gObj = other.gameObject;
				BoosterItem boost = gObj.GetComponent<BoosterItem>();
				if (!boost.IsEat)
				{
					Boost();
					boost.Eat();
				}
			}
		}

	}
}
