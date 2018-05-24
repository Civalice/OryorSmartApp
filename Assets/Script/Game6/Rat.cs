using UnityEngine;
using System.Collections;

public class Rat : BadGuy {

	[SerializeField]
	private Sprite[] ratSprites;

	private int colorSet = 0;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		var ran = Random.Range(0f, 2.99f);
		colorSet = (int)ran;
		sr = transform.FindChild("body").GetComponent<SpriteRenderer>();
		sr.sprite = ratSprites[ colorSet*2 ];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void walk(Vector3 direction)
	{
		sr.sprite = ratSprites[ (colorSet * 2) +1 ];
		if(direction.x < 0)
			sr.transform.localScale = new Vector3( 1, 1, 1);
		else if(direction.x > 0)
			sr.transform.localScale = new Vector3( -1, 1, 1);
	}

	private void stand()
	{
		sr.sprite = ratSprites[ colorSet * 2 ];
	}

	public override void MoveToTarget (Vector3 target)
	{
		StartCoroutine( moveCo( target ) );
	}

	IEnumerator moveCo(Vector3 target)
	{
		walk( target - transform.position );
		yield return StartCoroutine( Util.SmoothMovement( this.transform, target, 2f ) );
		stand();
	}


}
