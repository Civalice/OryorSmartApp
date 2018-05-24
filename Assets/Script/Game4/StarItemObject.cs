using UnityEngine;
using System.Collections;
namespace Game4
{
	public class StarItemObject : MonoBehaviour {
		public float spd = 0;

		public bool IsEat = false;
		Vector2 velocity;

		void Awake() {
			Debug.Log("StarItem");
			GetComponent<Animator>().Play("StarIdle");
			PreAwake();
		}
		protected void PreAwake()
		{
			velocity = new Vector2(0,0);
		}
		// Use this for initialization
		void Start () {
		}

		public virtual int GetScore()
		{
			//Get Star Score
			return Game4_LvlingStat.GetStarScore();
		}

		public virtual void Eat()
		{
			IsEat = true;
			GetComponent<Animator>().Play("StarEat");
			MainSoundSrc.PlaySound("right");
		}


		void UpdatePosition() {
			GetComponent<Rigidbody2D>().velocity = velocity;
		}
		// Update is called once per frame
		void Update () {
			spd = TurtleCharacter.currentSpeed;
			velocity.y = -spd;
			UpdatePosition();
		}

		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.tag == "Trash")
			{
				Destroy(this.gameObject);
				if (!IsEat)
					Game4Global.BrokeCombo();
			}
		}
	}
}