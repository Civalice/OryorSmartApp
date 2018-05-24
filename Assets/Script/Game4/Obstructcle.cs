using UnityEngine;
using System.Collections;

namespace Game4
{

	public class Obstructcle : MonoBehaviour {
		public float spd = 0;
		bool IsBounce = false;
		public SpriteRenderer sr;
		Color opacity;

		Vector2 velocity;
		void Awake () {
			opacity = sr.color;
			velocity = new Vector2(0,0);
		}
		// Use this for initialization
		void Start () {
		
		}
		
		void UpdatePosition() {
			GetComponent<Rigidbody2D>().velocity = velocity;
		}
		// Update is called once per frame
		void Update () {
			spd = TurtleCharacter.currentSpeed;
			if (!IsBounce)
			{
				velocity.y = -spd;
				UpdatePosition();
			}
			else
			{
				//Set Opacity to zero
				opacity.a -= 0.05f;
				if (opacity.a <= 0)
					Destroy (this.gameObject);
				sr.color = opacity;
			}
		}

		public void Bounce(Vector3 turtlePos)
		{
			IsBounce = true;
			GetComponent<Collider2D>().enabled = false;
			//get Force to push

			Vector2 direction = transform.position - turtlePos;

			GetComponent<Rigidbody2D>().AddForce(direction*3);
			GetComponent<Rigidbody2D>().AddTorque(100);
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
