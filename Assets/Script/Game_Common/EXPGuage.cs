using UnityEngine;
using System.Collections;
using TMPro;

public class EXPGuage : MonoBehaviour {
	public GameObject LevelGuage;
	public TextMeshPro LevelTxt;
	public TextMeshPro EXPPercent;

	private MorphObject progressMorph = new MorphObject();

	private int currentLevel = 0;
	private int currentProgress;

	private int nextLevel = 0;
	private int nextProgress;

	public void SetCurrentProgress(int _percent,int _lvl)
	{
		currentLevel = _lvl;
		currentProgress = _percent;
		LevelTxt.text = currentLevel.ToString ();
		SetProgress (currentProgress);
	}

	public void SetNextProgress(int _percent,int _lvl)
	{
		nextLevel = _lvl;
		nextProgress = _percent;
	}

	public void RunProgress()
	{
		StartCoroutine ("IRunProgress");
	}

	public void SetProgress(int _percent)
	{
		LevelGuage.transform.localScale = new Vector3(_percent/100.0f,1,1);
		EXPPercent.text = _percent.ToString () + "%";
	}

	IEnumerator IRunProgress()
	{
		int lvlCount = nextLevel - currentLevel;
		int progressRun = (nextProgress + lvlCount * 100) - currentProgress;
		progressMorph.morphEasein(currentProgress,currentProgress+progressRun,120);
		while (!progressMorph.IsFinish()) {
			progressMorph.Update();
			int cPro = (int)progressMorph.val;
			int lvl = currentLevel + (cPro)/100;
			SetProgress(cPro%100);
			LevelTxt.text = lvl.ToString ();

			yield return null;
		}
		yield return null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
