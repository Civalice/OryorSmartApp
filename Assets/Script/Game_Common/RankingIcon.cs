using UnityEngine;
using System.Collections;
using TMPro;
public class RankingIcon : MonoBehaviour {
	public TextMeshPro oldRankTxt;
	public GameObject GameIcon;
	public GameObject LevelupIcon;
	public TextMeshPro newRankTxt;
	
	private int oldRank;
	private int newRank;

	const int smooth = 5;

	public void SetIcon(Sprite icon)
	{
		GameIcon.GetComponent<SpriteRenderer>().sprite = icon;
	}

	public void Reset()
	{
		Vector3 scale = new Vector3 (0, 0, 1);
		GameIcon.transform.localScale = scale;
		oldRankTxt.transform.localScale = scale;
		LevelupIcon.transform.localScale = scale;
		newRankTxt.transform.localScale = scale;
		oldRankTxt.text = oldRank.ToString ();
		newRankTxt.text = newRank.ToString ();
	}

	public void ShowRanking(int _old,int _new)
	{
		oldRank = _old;
		newRank = _new;
		Reset ();
		StartCoroutine ("PlayRanking");
	}

	IEnumerator PlayRanking()
	{
		StartCoroutine ("PopIcon");
		yield return new WaitForSeconds (0.3f);
		StartCoroutine ("PopOldRank");
		yield return new WaitForSeconds (0.3f);
		StartCoroutine ("PopLevel");
		yield return new WaitForSeconds (0.3f);
		StartCoroutine ("PopNewRank");
	}

	IEnumerator PopIcon()
	{
		Vector3 nScale = new Vector3 (1, 1, 1);
		GameObject icon = GameIcon;
		while (Vector3.Distance(icon.transform.localScale,nScale)>0.01f) {
			Vector3 cScale = Vector3.Lerp(icon.transform.localScale,nScale,Time.deltaTime*smooth);
			icon.transform.localScale = cScale;
			yield return null;
		}
		icon.transform.localScale = nScale;
	}

	IEnumerator PopOldRank()
	{
		Vector3 nScale = new Vector3 (1, 1, 1);
		GameObject icon = oldRankTxt.gameObject;
		while (Vector3.Distance(icon.transform.localScale,nScale)>0.01f) {
			Vector3 cScale = Vector3.Lerp(icon.transform.localScale,nScale,Time.deltaTime*smooth);
			icon.transform.localScale = cScale;
			yield return null;
		}
		icon.transform.localScale = nScale;

	}

	IEnumerator PopLevel()
	{
		Vector3 nScale = new Vector3 (1, 1, 1);
		GameObject icon = LevelupIcon;
		while (Vector3.Distance(icon.transform.localScale,nScale)>0.01f) {
			Vector3 cScale = Vector3.Lerp(icon.transform.localScale,nScale,Time.deltaTime*smooth);
			icon.transform.localScale = cScale;
			yield return null;
		}
		icon.transform.localScale = nScale;

	}

	IEnumerator PopNewRank()
	{
		Vector3 nScale = new Vector3 (1, 1, 1);
		GameObject icon = newRankTxt.gameObject;
		while (Vector3.Distance(icon.transform.localScale,nScale)>0.01f) {
			Vector3 cScale = Vector3.Lerp(icon.transform.localScale,nScale,Time.deltaTime*smooth);
			icon.transform.localScale = cScale;
			yield return null;
		}
		icon.transform.localScale = nScale;

	}

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
