using UnityEngine;
using System.Collections;

public class Mole : BadGuy {

	[SerializeField]
	private Sprite[] moleSprites;
	private int colorSet = 0;

	private Animator anim;
	private SpriteRenderer sr;

	void Awake()
	{	
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		var ran = Random.Range(0f, 2.99f);
		colorSet = (int)ran;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[SerializeField]
	private bool isFinishHidding = false;

	public override void MoveToTarget (Vector3 target)
	{
		StartCoroutine( moving( target ) );
	}

	IEnumerator moving(Vector3 target)
	{
		setActivateLifeText(false);
		anim.SetBool("Show", false);

		while( !isFinishHidding ) yield return null;

		//yield return new WaitForSeconds(1f);

		this.transform.position = target;

		anim.SetBool("Show", true);
		setActivateLifeText(true);

	}

	public void SetSprite(int level)
	{
		if(sr == null)Debug.LogError("SR null"+level+":"+colorSet);
		sr.sprite = moleSprites[ level + (3*colorSet) ];
	}
}
