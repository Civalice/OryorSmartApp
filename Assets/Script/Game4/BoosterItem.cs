using UnityEngine;
using System.Collections;
namespace Game4
{
	public class BoosterItem : MonoBehaviour {
		public float spd = 0;
		
		public bool IsEat = false;
		Vector2 velocity;
		
		void Awake() {
			PreAwake();
		}
		protected void PreAwake()
		{
			velocity = new Vector2(0,0);
		}
		// Use this for initialization
		void Start () {
		}
		
		public void Eat()
		{
			IsEat = true;
			GetComponent<Animator>().Play("BoosterEat");
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
			}
		}
	}
}