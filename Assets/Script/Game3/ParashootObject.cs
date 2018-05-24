using UnityEngine;
using System.Collections;
namespace Game3
{
	public class ParashootObject : MonoBehaviour {
		SpriteRenderer spriteRenderer;
		ParashootType type;

		public bool IsDetach = false;
		public bool IsTouch = false;
		public Vector2 touchPos;

		bool IsEnterTrash = false;
		ParashootWave parentWave;

		void Awake() {
			spriteRenderer = GetComponent<SpriteRenderer>();
			GetComponent<Collider2D>().enabled = false;
		}

		public void SetParentWave(ParashootWave wave)
		{
			parentWave = wave;
		}

		public void DetachParashoot(ParashootType _type)
		{
			type = _type;
			Game3Global.pGlobal.AddGameplayObject(gameObject);
			gameObject.AddComponent<Rigidbody2D>();
			GetComponent<Collider2D>().enabled = true;
			IsDetach = true;
		}

		public void setObjectSprite(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}

		// Use this for initialization
		void Start () {
		
		}
		void UpdatePosition()
		{
			if (!IsDetach) return;
			if (IsTouch)
			{
				//no velocity
				GetComponent<Rigidbody2D>().gravityScale = 0;
				Vector2 currentPos = transform.position;
				GetComponent<Rigidbody2D>().velocity = (touchPos - currentPos)*60;
			}
			else
			{
				GetComponent<Rigidbody2D>().gravityScale = 1;
			}
		}
		// Update is called once per frame
		void Update () {
			Vector2 pos = TouchInterface.GetTouchPosition();
			bool isTouchDown = TouchInterface.GetTouchDown();
			bool isTouchUp = TouchInterface.GetTouchUp();
			if (isTouchDown && GetComponent<Collider2D>().OverlapPoint(pos))
			{
				if (Game3Global.pGlobal.dragObject == null)
				{
					Game3Global.pGlobal.dragObject = this;
					IsTouch = true;
				}
			}
			if (isTouchUp && Game3Global.pGlobal.dragObject == this)
			{
				//calculate score
				if (Game3Global.pGlobal.boxObject != null)
				{
					if (Game3Global.pGlobal.boxObject.type == this.type)
					{
						//correct
						MainSoundSrc.PlaySound("right");
						Game3Global.BirdOK();
						Game3Global.AddScore(300);
						Game3Global.AddCombo();
						Game3_LvlingStat.AddEXP();
						parentWave.ParashootClear();
						Destroy(this.gameObject);

					}
					else
					{
						MainSoundSrc.PlaySound("wrong");
						Game3Global.BirdWrong ();
						Game3Global.BrokeCombo();
						Game3Global.DecreaseLife();
						parentWave.ParashootClear(false);
						Destroy(this.gameObject);
					}
				}
				Game3Global.pGlobal.dragObject = null;
				IsTouch = false;
			}
			if (IsTouch)
			{
				touchPos = pos;
			}
			else
			{
				if (IsEnterTrash)
				{
					if (type != ParashootType.PARA_OTHER)
					{
						MainSoundSrc.PlaySound("wrong");
						Game3Global.BirdWrong ();
						Game3Global.BrokeCombo();
						Game3Global.DecreaseLife();
						parentWave.ParashootClear(false);
					}
					else
					{
						MainSoundSrc.PlaySound("bin");
						parentWave.ParashootClear();
					}
					Game3Global.TrashEnter();
					Destroy (this.gameObject);
				}
			}
			UpdatePosition();
		}
		
		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.tag == "Trash")
			{
				IsEnterTrash = true;
			}
		}
		
		void OnTriggerExit2D(Collider2D other)
		{
			if (other.tag == "GameBound")
			{
				MainSoundSrc.PlaySound("wrong");
				Game3Global.BirdWrong ();
				Game3Global.DecreaseLife();
				Game3Global.BrokeCombo();
				Destroy(this.gameObject);
			}
			if (other.tag == "Trash")
			{
				IsEnterTrash = false;
			}
		}

	}
}
