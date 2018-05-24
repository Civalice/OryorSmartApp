using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	private Animator animator;

	[SerializeField]
	private TMPro.TextMeshPro text;

	[SerializeField]
	private GameObject oryorLogo;

	[SerializeField]
	private GameObject ex, tired, bye;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetupHint(InfoSign.SignType signType)
	{
		ex.SetActive(true);
		tired.SetActive(false);
		bye.SetActive(false);
		//hint.sprite = hintSprites[ (int)signType ];
		text.gameObject.SetActive( true );
		Answer.SetupText(signType, oryorLogo, text);
	}

	public void SetupTired()
	{
		ex.SetActive( false );
		tired.SetActive( true );
		bye.SetActive( false );
		text.gameObject.SetActive( false );
	}

	public void SetupBye()
	{
		ex.SetActive( false );
		tired.SetActive( false );
		bye.SetActive( true );
		text.gameObject.SetActive( false );
	}

	public void Show()
	{
		animator.SetBool("Show", true);
	}

	public void Hide()
	{
		animator.SetBool("Show", false);
	}
}
