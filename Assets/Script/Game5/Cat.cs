using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

	[SerializeField]
	private Bubble bubble;

	[SerializeField]
	private int byeLevel;

	private bool isActivate = true;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeLevel(int levelNumber, InfoSign.SignType signType)
	{
		if(!isActivate) return;
		if(levelNumber < byeLevel)
		{
			bubble.SetupHint( signType );
			bubble.Show();
		}else{
			isActivate = false;
			StartCoroutine(byeCoroutine());
		}
	}

	public void FinishLevel()
	{
		if(!isActivate) return;
		bubble.Hide();
	}

	private IEnumerator byeCoroutine()
	{
		animator.SetBool("Tired", true);
		bubble.SetupTired();
		bubble.Show();
		yield return new WaitForSeconds(3f);
		bubble.Hide();
		yield return new WaitForSeconds(1f);
		animator.SetBool("Bye", true);
		bubble.SetupBye();
		bubble.Show();
		yield return new WaitForSeconds(4f);
		hideCat();
	}

	private void hideCat()
	{
		bubble.Hide();
		gameObject.SetActive(false);
	}

	public void AwakeUpNow()
	{
		bubble.Hide();
		gameObject.SetActive(true);
		isActivate = true;
		animator.Play("Idle");
	}
}
