using UnityEngine;
using System.Collections;

public class SherDog : MonoBehaviour {

	[SerializeField]
	private GameObject throwable;

	[SerializeField]
	private HPNotification hpNofityPrefab;

	private Animator body;
	private GameObject hit;

	// Use this for initialization
	void Start () {
		body = transform.FindChild("Body").GetComponent<Animator>();
		hit = transform.FindChild("Hit").gameObject;
	}

	public void Hit()
	{
		body.SetTrigger("Lose");
		StartCoroutine( hitCo() );
		var hpNotify = Instantiate(hpNofityPrefab);
		hpNotify.transform.position = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
		hpNotify.gameObject.SetActive(true);
		hpNotify.DecreaseLife(1);
	}

	IEnumerator hitCo(){
		hit.SetActive(true);
		yield return new WaitForSeconds(1f);
		hit.SetActive(false);
	}

	public void Combine()
	{
		body.SetTrigger("Combine");
	}

	public void SetTired(bool isTired)
	{
		body.SetBool("Tired", isTired);
	}

	public void Reset()
	{
		SetTired(false);
	}

	public GameObject GetThrowableObject()
	{
		return throwable;
	}

	public Vector3 GetThrowingPosition()
	{
		return transform.position - new Vector3(0f, -1.2f, 0f);
	}

}
