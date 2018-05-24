using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour{
	
	protected bool isClickable = false;
	public void SetClickable(bool isClickable)
	{
		this.isClickable = isClickable;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isClickable || Game6Controller.GetInstance().IsPause())return;
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		var isHit = GetComponent<Collider2D>().OverlapPoint (pos);
		if(TouchDown && isHit)
		{
			Game6Controller.GetInstance().StartDrag( this );
		}
	}
}

public class Product : Selectable {

	[SerializeField]
	private Sprite [] items;
	
	public int Level{
		get;
		private set;
	}

	public void Setup(int level, bool isClickable = false)
	{
		GetComponent<SpriteRenderer>().sprite = items[ level - 1 ];
		this.isClickable = isClickable;
		Level = level;
	}
	
	public void Upgrade()
	{
		Level++;
		GetComponent<SpriteRenderer>().sprite = items[ Level - 1 ];
	}

	// Use this for initialization
	void Start () {

	}

	public void Capture()
	{
		GetComponent<Animator>().SetTrigger("Capture");
	}

	public void MoveAndDestroy(Vector3 target, float time)
	{
		StartCoroutine( mad (target, time) );
	}

	private IEnumerator mad(Vector3 target, float time)
	{
		var startTime = 0f;
		var startX = transform.position.x;
		var startY = transform.position.y;
		var startZ = transform.position.z;

		while( startTime < time )
		{
			startTime += Time.deltaTime;
			var t = startTime/time;
			var x = Mathf.Lerp( startX, target.x, t );
			var y = Mathf.Lerp( startY, target.y, t );
			transform.position = new Vector3(x, y, startZ);

			yield return null;
		}

		Destroy( gameObject );
	}
}

