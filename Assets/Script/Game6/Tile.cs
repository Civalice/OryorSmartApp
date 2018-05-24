using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour {

	private GameObject tileBG;
	private Tile top, left, right, bottom;
	private bool isHighlightOn = false;
	private Product currentProduct;
	private BadGuy badGuy;

	public void SetupTiles(Tile top, Tile left, Tile right, Tile bottom)
	{
		this.top = top;
		this.left = left;
		this.right = right;
		this.bottom = bottom;
	}

	/*
	public Tile Top(){ return top; }
	public Tile Left() { return left; }
	public Tile Right() { return right; }
	public Tile Bottom() { return bottom; }
	*/

	public Tile[] GetSurroundTiles()
	{
		var tileList = new List<Tile>();
		if(top != null)
		{
			tileList.Add(top);
			if( top.left != null) tileList.Add( top.left );
			if( top.right != null) tileList.Add( top.right );
		}
		if(bottom != null)
		{
			tileList.Add(bottom);
			if( bottom.left != null) tileList.Add( bottom.left );
			if( bottom.right != null) tileList.Add( bottom.right );
		}
		if(right != null)
		{
			tileList.Add(right);
		}
		if(left != null)
		{
			tileList.Add(left);
		}

		return tileList.ToArray();
	}

	public void SetupProduct(Product product)
	{
		currentProduct = product;
		currentProduct.transform.parent = this.transform;
		currentProduct.transform.localPosition = Vector3.zero;
		currentProduct.SetClickable(false);
	}

	public int GetCurrentProductLevel()
	{
		if(currentProduct==null)return 0;
		return currentProduct.Level;
	}

	public bool IsBuildAble()
	{
		return currentProduct == null && badGuy == null;
	}

	public List<Tile> GetCrossBuildableTileList()
	{
		var tileList = new List<Tile>();
		if( top != null && top.IsBuildAble() ) tileList.Add( top );
		if( left != null && left.IsBuildAble() ) tileList.Add( left );
		if( bottom != null && bottom.IsBuildAble() ) tileList.Add( bottom );
		if( right != null && right.IsBuildAble() ) tileList.Add( right );
		return tileList;
	}

	public int[] GetCrossBuildingHighLevel()
	{
		var levels = new List<int>();
		if(top != null && !levels.Contains(top.GetCurrentProductLevel())) levels.Add(top.GetCurrentProductLevel());
		if(left != null && !levels.Contains(left.GetCurrentProductLevel())) levels.Add(left.GetCurrentProductLevel());
		if(bottom != null && !levels.Contains(bottom.GetCurrentProductLevel())) levels.Add(bottom.GetCurrentProductLevel());
		if(right != null && !levels.Contains(right.GetCurrentProductLevel())) levels.Add(right.GetCurrentProductLevel());
		return levels.OrderBy( m => m ).ToArray();
	}

	// Use this for initialization
	void Start () {
		tileBG = transform.FindChild("tile_bg").gameObject;
		tileBG.SetActive(false);
	}

	bool isSelected = false;
	// Update is called once per frame
	void Update () {
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		var isHit = GetComponent<Collider2D>().OverlapPoint (pos);
		if(isHit)
		{
			isSelected = true;
			Game6Controller.GetInstance().SetSelectTile(this);
		}else if(isSelected)
		{
			isSelected = false;
			Game6Controller.GetInstance().DeselectTile(this);
		}
	}

	private void SetCrossHighlight(bool isOn)
	{
		SetHighlight(isOn);
		if(top != null) top.SetHighlight(isOn);
		if(left != null) left.SetHighlight(isOn);
		if(right != null) right.SetHighlight(isOn);
		if(bottom != null) bottom.SetHighlight(isOn);

		UpdateHighlight();
		if(top != null) top.UpdateHighlight();
		if(left != null) left.UpdateHighlight();
		if(right != null) right.UpdateHighlight();
		if(bottom != null) bottom.UpdateHighlight();
	}

	public void SetHighlight(bool isOn, bool forceUpdate = false)
	{
		isHighlightOn = isOn;
		if(forceUpdate) UpdateHighlight();
	}

	public void UpdateHighlight()
	{
		tileBG.SetActive( isHighlightOn );
	}

	public Tile[] GetSimilarTile(int? targetLevel = null, Tile startTile = null)
	{
		var currentLevel = targetLevel ?? GetCurrentProductLevel();
		if(currentLevel == 0)return null;

		var tileList = new List<Tile>();

		var nextTop = top;
		if(nextTop != null && ( startTile == null || startTile != nextTop ) && nextTop.GetCurrentProductLevel() == currentLevel)
		{
			tileList.Add(nextTop);
			tileList.AddRange( nextTop.GetSimilarTile(currentLevel, this) );
		}
		
		var nextBottom = bottom;
		if(nextBottom != null && ( startTile == null || startTile != nextBottom ) && nextBottom.GetCurrentProductLevel() == currentLevel)
		{
			tileList.Add(nextBottom);
			tileList.AddRange( nextBottom.GetSimilarTile(currentLevel, this) );
		}

		var nextLeft = left;
		if(nextLeft != null && ( startTile == null || startTile != nextLeft ) && nextLeft.GetCurrentProductLevel() == currentLevel)
		{
			tileList.Add(nextLeft);
			tileList.AddRange( nextLeft.GetSimilarTile(currentLevel, this) );
		}

		var nextRight = right;
		if(nextRight != null && ( startTile == null || startTile != nextRight ) && nextRight.GetCurrentProductLevel() == currentLevel)
		{
			tileList.Add(nextRight);
			tileList.AddRange( nextRight.GetSimilarTile(currentLevel, this) );
		}

		return tileList.Distinct().ToArray();
	}

	public Product RemoveProduct()
	{
		var removedProduct = currentProduct;
		currentProduct = null;
		return removedProduct;
	}

	public void UpgradeProduct()
	{
		if(currentProduct!=null)
		{
			currentProduct.Upgrade();
		}
	}

	public bool IsProductComplete()
	{
		if(currentProduct==null)return false;
		else return currentProduct.Level == 6;
	}

	public void setHighlightProduct(bool isOn)
	{
		if(currentProduct == null) return;
		currentProduct.GetComponent<Animator>().SetBool("Select", isOn);

	}

	public void AddBadGuy(BadGuy badGuy, bool isJustCreated = false)
	{
		this.badGuy = badGuy;
		badGuy.transform.parent = this.transform;
		if(isJustCreated)
			badGuy.transform.localPosition = Vector3.zero;
		else
			badGuy.MoveToTarget( this.transform.position );
	}

	public BadGuy GetBadGuy()
	{
		return badGuy;
	}

	public BadGuy RemoveBadGuy()
	{
		var temp = badGuy;
		badGuy = null;;
		return temp;
	}

	public void Reset()
	{
		var p = RemoveProduct();
		if( p != null) Destroy(p.gameObject);
		var b = RemoveBadGuy();
		if( b != null) Destroy(b.gameObject);
	}
}
