using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {

	private Selectable depositProduct;

	[SerializeField]
	private GameObject border;

	public Selectable SwapProdcut(Selectable addingProduct)
	{
		addingProduct.transform.parent = transform;
		addingProduct.transform.localPosition = Vector3.zero;
		addingProduct.transform.localScale = Vector3.one;
		//addingProduct.SetClickable(false);
		var withdrawProduct = depositProduct;
		if(withdrawProduct != null) withdrawProduct.SetClickable(true);
		depositProduct = addingProduct;

		return withdrawProduct;
	}

	public Selectable GetDepositProduct()
	{
		return depositProduct;
	}

	public void UseDeposit()
	{
		depositProduct = null;
	}

	public void AddDeposit(Selectable add)
	{
		depositProduct = add;
	}

	public void DestroyDeposit()
	{
		if(depositProduct!=null)
		{
			Destroy( depositProduct.gameObject );
			depositProduct = null;
		}
	}

	public void SetHilight(bool isVisible)
	{
		border.SetActive(isVisible);
	}
}
