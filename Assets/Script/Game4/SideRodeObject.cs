using UnityEngine;
using System.Collections;
namespace Game4
{
	public class SideRodeObject : MonoBehaviour {

		SpriteRenderer spRenderer;

		public float spd = 0;

		GoldStarItemObject starLock;
		Vector2 velocity;

		void Awake () {
			spRenderer = GetComponent<SpriteRenderer>();
			velocity = new Vector2(0,0);
		}

		public void SetSidewayObject(Sprite spr)
		{
			spRenderer.sprite = spr;
		}

		public void SetThrowStar(GoldStarItemObject star)
		{
			starLock = star;
			starLock.transform.position = this.transform.position;
			Debug.Log("Gold Star Position Start = "+starLock.transform.position.x);
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
