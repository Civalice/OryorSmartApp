using UnityEngine;
using System.Collections;
using TMPro;

public class ItemFlag : MonoBehaviour {
	public delegate void SetTextCallback(string text,int price);
	public SetTextCallback cb;

	public bool IsSelect = false;
	public int ItemCount = 0;
	public bool IsLock = false;

	public Sprite ChooseSprite;
	public Sprite UnChooseSprite;
	public GameObject Lock;

	public Collider2D chooseItem;
	public int ItemPrice;
	public string ItemDesc;

	public TextMeshPro PriceText;
	public TextMeshPro CountText;

	public void Select()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.sprite = ChooseSprite;
		IsSelect = true;
	}

	public void UnSelect()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.sprite = UnChooseSprite;
		IsSelect = false;
	}

	public void Toggle()
	{
		if (IsSelect)
						UnSelect ();
				else
						Select ();
	}

	public void SetLock(bool flag)
	{
		IsLock = flag;
		if (IsLock)
			Lock.SetActive(true);
		else
			Lock.SetActive(false);
	}
	public void SetPrice()
	{
		if (ItemCount > 0)
		{
			PriceText.text = "FREE";
			if (CountText != null)
			{
				CountText.text = ItemCount.ToString();
				CountText.gameObject.SetActive(true);
			}
		}
		else
		{
			if (ItemPrice == 0)
				PriceText.text = "FREE";
			else
				PriceText.text = ItemPrice.ToString();
			if (CountText != null)
			{
				CountText.gameObject.SetActive(false);
			}
		}
	}

	public void SetItemCount(int count)
	{
		ItemCount = count;
		SetPrice();
	}
	// Use this for initialization
	void Start () {
//		SetLock(true);
		SetPrice();
		UnSelect ();
	}

	// Update is called once per frame
	void Update () {
		if (IsLock) return;
		bool TouchDown = TouchInterface.GetTouchDown ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		if (TouchDown) {
			if (chooseItem.OverlapPoint (pos)) {
				Toggle();
				if (cb != null)
					if (ItemCount > 0)
						cb(ItemDesc,0);
					else
						cb(ItemDesc,IsSelect?-ItemPrice:ItemPrice);
					}
				}
	}
}
