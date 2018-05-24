using UnityEngine;
using System.Collections;
using TMPro;

public class MoneyGuage : MonoBehaviour {
	public TextMeshPro MoneyText;
	private MorphObject MoneyMorph = new MorphObject();

	public int money {
		get{ return mPrice; }
	}

	private int mPrice = 0;

	public void SetMoney(int money)
	{
		StopCoroutine ("RunMoney");
		mPrice = money;
		MoneyMorph.morphEasein (money, 60);
		StartCoroutine ("RunMoney");
	}
	public void ChangeMoney(int diff)
	{
		SetMoney (mPrice + diff);
	}
	public void SetStartMoney(int money)
	{
		mPrice = money;
		MoneyMorph.setVal (money);
		MoneyText.text = money.ToString("#,##0");
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator RunMoney()
	{
		while (!MoneyMorph.IsFinish()) {
			MoneyMorph.Update();
			int money = (int)MoneyMorph.val;
			MoneyText.text = money.ToString("#,##0");
			yield return null;
		}
	}
}
