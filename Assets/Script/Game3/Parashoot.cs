using UnityEngine;
using System.Collections;
namespace Game3
{
	public enum ParashootType
	{
		PARA_OTHER = 0,
		PARA_DRUG,
		PARA_FOOD,
		PARA_LIP,
		PARA_MED,
		PARA_POISON,
	}

	public class Parashoot : MonoBehaviour {
		public SpriteRenderer ParashootColor;
		public SpriteRenderer ParashootSymbol;
		public ParashootObject parashootObject;

		public ParashootType type;

		public float maxSpeed = 1;
		public Vector2 velocity;
		public GameObject obj;

		ParashootWave parentWave;

		float animVal = 0;

		public void SetParentWave(ParashootWave wave)
		{
			parentWave = wave;
			parashootObject.SetParentWave(wave);
		}

		public void SetSpeed(float spd)
		{
			maxSpeed = spd;
		}

		public void SetParashoot(ParashootType _type)
		{
			type = _type;

			switch(type)
			{
			case ParashootType.PARA_DRUG:
			{
				int idx = Random.Range(0,Game3Global.pGlobal.parashootObjectList[0].spriteList.Length);
				Sprite pSymbol = Game3Global.pGlobal.parashootSymbolDrug;
				Sprite pObject = Game3Global.pGlobal.parashootObjectList[0].spriteList[idx];
				//set object sprite
				ParashootSymbol.sprite = pSymbol;
				parashootObject.setObjectSprite(pObject);
			}
				break;
			case ParashootType.PARA_FOOD:
			{
				int idx = Random.Range(0,Game3Global.pGlobal.parashootObjectList[1].spriteList.Length);
				Sprite pSymbol = Game3Global.pGlobal.parashootSymbolFood;
				Sprite pObject = Game3Global.pGlobal.parashootObjectList[1].spriteList[idx];
				//set object sprite
				ParashootSymbol.sprite = pSymbol;
				parashootObject.setObjectSprite(pObject);
			}
				break;
			case ParashootType.PARA_LIP:
			{
				int idx = Random.Range(0,Game3Global.pGlobal.parashootObjectList[2].spriteList.Length);
				Sprite pSymbol = Game3Global.pGlobal.parashootSymbolLip;
				Sprite pObject = Game3Global.pGlobal.parashootObjectList[2].spriteList[idx];
				//set object sprite
				ParashootSymbol.sprite = pSymbol;
				parashootObject.setObjectSprite(pObject);
			}
				break;
			case ParashootType.PARA_MED:
			{
				int idx = Random.Range(0,Game3Global.pGlobal.parashootObjectList[3].spriteList.Length);
				Sprite pSymbol = Game3Global.pGlobal.parashootSymbolMedic;
				Sprite pObject = Game3Global.pGlobal.parashootObjectList[3].spriteList[idx];
				//set object sprite
				ParashootSymbol.sprite = pSymbol;
				parashootObject.setObjectSprite(pObject);
			}
				break;
			case ParashootType.PARA_POISON:
			{
				int idx = Random.Range(0,Game3Global.pGlobal.parashootObjectList[4].spriteList.Length);
				Sprite pSymbol = Game3Global.pGlobal.parashootSymbolPoison;
				Sprite pObject = Game3Global.pGlobal.parashootObjectList[4].spriteList[idx];
				//set object sprite
				ParashootSymbol.sprite = pSymbol;
				parashootObject.setObjectSprite(pObject);
			}
				break;
			case ParashootType.PARA_OTHER:
			{
				int idx = Random.Range(0,Game3Global.pGlobal.parashootObjectList[5].spriteList.Length);
				Sprite pObject = Game3Global.pGlobal.parashootObjectList[5].spriteList[idx];
				//set object sprite
				ParashootSymbol.gameObject.SetActive(false);
				parashootObject.setObjectSprite(pObject);
			}
				break;
			}
		}

		void Awake () {
			velocity = new Vector2();
		}
		// Use this for initialization
		void Start () {
		}

		void UpdatePhysic()
		{
			velocity = GetComponent<Rigidbody2D>().velocity;
			if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeed)
			{
				velocity.y = maxSpeed * Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y);
				GetComponent<Rigidbody2D>().velocity = velocity;
			}
		}

		// Update is called once per frame
		void FixedUpdate () {
			if (Game3Global.GlobalPause)
			{
				GetComponent<Rigidbody2D>().Sleep();
			}
			else
			{
				GetComponent<Rigidbody2D>().WakeUp();
			}
			animVal += Time.deltaTime;

			float s = Mathf.Sin(animVal);
			Quaternion rot = transform.rotation;
			Vector3 eul = new Vector3();
			eul.z = s*20;
			rot.eulerAngles = eul;
			transform.rotation = rot;
			//update Physics2D
			UpdatePhysic();
		}

		void Update()
		{
			Vector2 pos = TouchInterface.GetTouchPosition();
			bool isTouchDown = TouchInterface.GetTouchDown();
			if (isTouchDown && GetComponent<Collider2D>().OverlapPoint(pos))
			{
				parashootObject.DetachParashoot(type);
				//play animation detach parashoot before destroy object
				Destroy(this.gameObject);
			}
		}

		void OnTriggerExit2D(Collider2D other)
		{
			if (other.tag == "GameBound")
			{
	//			Debug.Log("Item Lost");
				MainSoundSrc.PlaySound("wrong");
				Game3Global.BirdWrong ();
				Game3Global.DecreaseLife();
				Game3Global.BrokeCombo();
				Destroy(this.gameObject);
			}
		}
	}
}