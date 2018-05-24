using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game6Controller : MonoBehaviour {

	private static Game6Controller inst;
	private Selectable selectedProduct;
	private Tile selectedTile;
	private bool isGameOver = false, isPause = false;
	private List<Tile> tileHintList = new List<Tile>();
	private int oryorCharge = 0;

	[SerializeField]
	private Slot slot;

	[SerializeField]
	private Mole molePrefab;

	[SerializeField]
	private Rat ratPrefab;

	[SerializeField]
	private ScoreFloat scorePrefab;

	[SerializeField]
	private UnityEngine.UI.Image oryorGauge;

	[SerializeField]
	private SherDog sherDog;

	[SerializeField]
	private GlobalController gController;

	[SerializeField]
	private Game6LevelController levelController;

	private enum itemType
	{
		None,
		Magnifier,
		Oryor
	}

	void Awake()
	{
		inst = this;
	}

	public static Game6Controller GetInstance()
	{
		return inst;
	}

	// Use this for initialization
	void Start () {
		gController.gResetEvent += gameReset;
		gController.gStartEvent += gameStart;
		gController.gGameOverEvent += gameOver;
		gController.gPauseEvent += gamePause;
		gController.gUnPauseEvent += gameResume;
		//gController.gCountdownEvent += PreGameStart;
		
		gController.SetMoneyMultiplier(1);
		gController.SetEXPMultiplier(1);
		gController.SetGameID(6);

		gController.mGameReset();
		//gController.mGameCountDown();
	}
	
	void gameStart()
	{
		isPause = false;
		isGameOver = false;
		oryorCharge = 0;
		oryorGauge.fillAmount = 0f;
		levelController.Reset();
		ProductFactory.GetInstance().GenerateProduct();
	}
	
	void gameReset()
	{
		isPause = false;
		isGameOver = false;
		oryorCharge = 0;
		oryorGauge.fillAmount = 0f;
		levelController.Reset();
		TileGenerator.GetTileList().ForEach(t=>t.Reset());
		ProductFactory.GetInstance().DestroyLastGenerated();
		slot.DestroyDeposit();
	}
	
	void gameOver()
	{
		isGameOver = true;
	}
	
	void gamePause()
	{
		isPause = true;
	}
	
	void gameResume()
	{
		isPause = false;
	}

	public bool IsPause()
	{
		return isPause;
	}
	
	// Update is called once per frame
	void Update () {
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 pos = TouchInterface.GetTouchPosition ();

		if( selectedProduct != null )
		{
			selectedProduct.transform.position = Vector3.back + (Vector3)pos;
			if(TouchUp)
			{
				if( selectedTile != null && (selectedProduct is Product || selectedProduct is Magnifier) && selectedTile.IsBuildAble() )
				{
					if( selectedProduct is Product)
						buildSelected();
					else if( selectedProduct is Magnifier)
						useMagnifier();
				}
				else if( selectedTile != null && selectedProduct is Locker && selectedTile.GetBadGuy() != null)
				{
					useLocker();
				}
				else if( selectedTile != null && selectedProduct is Oryor )
				{
					useOryor();
				}
				else if( isSlotSelected(pos) )
					swapProduct();
				else if( isProductFactorySelected(pos) )
					swapProductBackToFactory();
				else
					cancelSelected();
			}
		}
	}

	private void swapProduct()
	{
		var withdrawProduct = slot.SwapProdcut( selectedProduct );
		selectedProduct = null;
		slot.SetHilight(false);
		if(withdrawProduct != null)
		{
			withdrawProduct.transform.parent = ProductFactory.GetInstance().transform;
			withdrawProduct.transform.localPosition = Vector3.zero;
			withdrawProduct.transform.localScale = Vector3.one;
			ProductFactory.GetInstance().AddLastGeneratedProduct( withdrawProduct );
		}
		else if(ProductFactory.GetInstance().GetLastGeneratedProduct() == null)
		{
			ProductFactory.GetInstance().UseLastGeneratedProduct();
			ProductFactory.GetInstance().GenerateProduct(TileGenerator.GetTileList().Any(t=>t.GetBadGuy()!=null));
		}
	}

	private void swapProductBackToFactory()
	{
		if(ProductFactory.GetInstance().GetLastGeneratedProduct() == null)return;
		var withdrawProduct = ProductFactory.GetInstance().SwapProdcut( selectedProduct );
		selectedProduct = null;
		slot.SetHilight(false);
		if(withdrawProduct!=null)
		{
			withdrawProduct.transform.parent = slot.transform;
			withdrawProduct.transform.localPosition = Vector3.zero;
			withdrawProduct.transform.localScale = Vector3.one;
			slot.AddDeposit(withdrawProduct);
		}
	}

	private void cancelSelected()
	{
		selectedProduct.transform.localPosition = Vector3.zero;
		if(selectedProduct == ProductFactory.GetInstance().GetLastGeneratedProduct())
			ProductFactory.GetInstance().AddLastGeneratedProduct( selectedProduct );
		else
			slot.AddDeposit( selectedProduct );
		selectedProduct = null;
		slot.SetHilight(false);

		if(selectedTile!=null)
			selectedTile.SetHighlight(false, true);

		setTileHint(false, true);

		selectedTile = null;
	}

	private void buildSelected()
	{
		var builtTile = selectedTile;
		selectedTile = null;
		builtTile.SetupProduct( selectedProduct as Product );

		var score = levelController.GetCreateProductScore( builtTile.GetCurrentProductLevel() );
		addScore( score, builtTile.transform.position );

		selectedProduct = null;
		slot.SetHilight(false);
		builtTile.SetHighlight(false, true);

		StartCoroutine( finishMove( builtTile ) );
	}

	private void useMagnifier()
	{
		var builtTile = selectedTile;
		selectedTile = null;
		Destroy( selectedProduct.gameObject );
		selectedProduct = null;
		slot.SetHilight(false);
		builtTile.SetHighlight(false, true);

		StartCoroutine( finishMove( builtTile , itemType.Magnifier) );
	}

	private void useLocker()
	{
		var builtTile = selectedTile;
		selectedTile = null;
		Destroy( selectedProduct.gameObject );
		selectedProduct = null;
		slot.SetHilight(false);
		builtTile.SetHighlight(false, true);
		StartCoroutine( userLockerCo( builtTile ) );
	}

	private void useOryor()
	{
		var builtTile = selectedTile;
		selectedTile = null;
		Destroy( selectedProduct.gameObject );
		selectedProduct = null;
		slot.SetHilight(false);
		builtTile.SetHighlight(false, true);
		setTileHint( false, true);
		StartCoroutine( finishMove( builtTile, itemType.Oryor ) );
	}

	IEnumerator userLockerCo(Tile target, bool toNextTurn = true)
	{
		var bg = target.RemoveBadGuy();
		bg.die();
		MainSoundSrc.PlaySound("jail");
		
		var score = ( bg is Rat )?levelController.GetKillRatScore():levelController.GetKillMoleScore();
		addScore( score, bg.transform.position );

		yield return new WaitForSeconds(1f);
		Destroy( bg.gameObject );

		if(toNextTurn)StartCoroutine( nextTurn() );
	}

	private bool isSlotSelected(Vector3 pos)
	{
		return slot.GetComponent<Collider2D>().OverlapPoint (pos);
	}

	private bool isProductFactorySelected(Vector3 pos)
	{
		return ProductFactory.GetInstance().GetComponent<Collider2D>().OverlapPoint( pos );
	}

	IEnumerator finishMove(Tile target, itemType it = itemType.None)
	{
		Tile[] similarTiles = null;
		var highestMatchLevel = 0;

		if(it == itemType.Magnifier)
		{
			var highLevel = target.GetCrossBuildingHighLevel();

			foreach(var level in highLevel)
			{
				similarTiles = target.GetSimilarTile(level);
				if(similarTiles!=null && similarTiles.Length >= 2)
				{
					highestMatchLevel = level;
					break;
				}
			}
		}
		else if(it == itemType.Oryor)
		{
			var temp = new List<Tile>();
			temp.AddRange( target.GetSurroundTiles().Where ( t => t.GetCurrentProductLevel() != 0 ) );
			if(target.GetCurrentProductLevel() != 0)temp.Add( target );
			similarTiles = temp.ToArray();

			target.GetSurroundTiles().Where( t => t.GetBadGuy() != null ).ToList().ForEach( b => StartCoroutine(userLockerCo(b, false)) );
			if(target.GetBadGuy() != null) StartCoroutine(userLockerCo(target, false));
		}
		else
			similarTiles = target.GetSimilarTile();
		
		if(similarTiles != null && ((it == itemType.Oryor && similarTiles.Length > 0) || similarTiles.Length >= 2))
		{
			sherDog.Combine();
			gController.mAddCombo();
			var sound2 = Random.Range( 0, 20 );
			var combineSoundName = (sound2>10)?"combine":"combine2";
			MainSoundSrc.PlaySound( combineSoundName );

			//Score
			var score = levelController.GetCombineProductScore( similarTiles[0].GetCurrentProductLevel(), similarTiles.Length );
			oryorCharge += levelController.GetOryorChargingRate( similarTiles[0].GetCurrentProductLevel() );
			var formatGauge = oryorCharge/100f;
			oryorGauge.fillAmount = formatGauge;

			//Combine
			foreach(var tile in similarTiles)
			{
				var removedProduct = tile.RemoveProduct();
				removedProduct.MoveAndDestroy( target.transform.position, .25f );
			}
			yield return new WaitForSeconds(.25f);
			if(it == itemType.Magnifier)
			{
				var product = ProductFactory.GetInstance().CreateProduct( highestMatchLevel+1, true );
				target.SetupProduct( product );
			}
			else if(it == itemType.Oryor)
			{
				score = (int)(score * levelController.GetOryorMultiplier());
			}
			else
			{
				target.UpgradeProduct();
			}
			addScore( score, target.transform.position );

			yield return new WaitForSeconds(1f);
			
			if(target.IsProductComplete())
			{
				var removedProduct = target.RemoveProduct();
				removedProduct.Capture();
				yield return new WaitForSeconds(1f);
				Destroy( removedProduct.gameObject );

				/* Do not destroy bad guy after 6 combine
				var badGuys = target.GetSurroundTiles().Select( t => t.RemoveBadGuy() ).ToList();

				badGuys.ForEach( bg => {if(bg!=null)Destroy(bg.gameObject);} );
				*/				
			}

			yield return StartCoroutine( finishMove( target ));
		}
		else
		{
			yield return StartCoroutine( nextTurn() );
		}

		yield return 0;
	}

	private IEnumerator nextTurn()
	{
		levelController.AddTurn();

		var tileList = TileGenerator.GetTileList();

		var ratTile = tileList.FindAll( t => t.GetBadGuy() is Rat ).OrderBy( t => t.GetBadGuy().Life() );
		foreach(var ratT in ratTile)
		{
			if( ratT.GetBadGuy().DecreaseLife() <= 0 )
			{
				Debug.Log("Deduct User Life");

				var throwingProduct = sherDog.GetThrowableObject();
				throwingProduct.SetActive( true );
				throwingProduct.transform.position = new Vector3(ratT.transform.position.x , ratT.transform.position.y, throwingProduct.transform.position.z);
				yield return StartCoroutine( Util.SmoothMovement( throwingProduct.transform, sherDog.GetThrowingPosition(), 10f ) );

				throwingProduct.SetActive( false );
				sherDog.Hit();
				var ranSound = Random.Range(0,20);
				var hitSoundname = (ranSound)>10?"hit":"hit2";
				MainSoundSrc.PlaySound( hitSoundname );
				
				gController.mDecreaseLifeGuage();
				gController.mBrokeCombo();

				yield return new WaitForSeconds(1f);

				//Destroy( ratT.RemoveBadGuy().gameObject );
				ratT.GetBadGuy().SetLife( levelController.GetRatTurns() );

			}

			var availableTileList = ratT.GetCrossBuildableTileList();
			if(availableTileList.Count() == 0)continue;
			var selectedIndex = (int)Random.Range(0f, availableTileList.Count()-0.01f);
			var rat = ratT.RemoveBadGuy();
			availableTileList[selectedIndex].AddBadGuy( rat );

		}


		var moleTile = tileList.FindAll( t => t.GetBadGuy() is Mole );
		var moleList = moleTile.Select( mt => mt.RemoveBadGuy() as Mole ).OrderBy( m => m.Life() );

		var buildableTile = tileList.FindAll( t => t.IsBuildAble() );
		var buildableCount = buildableTile.Count;

		foreach(var mole in moleList)
		{
			if( mole.DecreaseLife() <= 0 )
			{Debug.Log("Deduct User Life");
				
				var throwingProduct = sherDog.GetThrowableObject();
				throwingProduct.SetActive( true );
				throwingProduct.transform.position = new Vector3(mole.transform.position.x , mole.transform.position.y, throwingProduct.transform.position.z);
				yield return StartCoroutine( Util.SmoothMovement( throwingProduct.transform, sherDog.GetThrowingPosition(), 10f ) );
				
				throwingProduct.SetActive( false );
				sherDog.Hit();
				var ranSound = Random.Range(0,20);
				var hitSoundname = (ranSound)>10?"hit":"hit2";
				MainSoundSrc.PlaySound( hitSoundname );
				
				gController.mDecreaseLifeGuage();
				gController.mBrokeCombo();
				
				yield return new WaitForSeconds(1f);
				
				//Destroy( mole.gameObject );
				mole.SetLife( levelController.GetMoleTurns() );
			}

			var newHomeIndex = (int)Random.Range(0f, buildableCount-0.01f);
			buildableTile[ newHomeIndex ].AddBadGuy( mole );
			buildableCount--;
			buildableTile.RemoveAt( newHomeIndex );

		}

		//Create Mole
		var moleChance = levelController.GetMoleChance( tileList.Count( t => t.GetBadGuy() is Mole ) );
		var ratChance = levelController.GetRatChance( tileList.Count( t => t.GetBadGuy() is Rat ) );
		var chance = Random.Range(0,100);
		if( buildableCount > 0 && chance < moleChance +  ratChance )
		{

			BadGuy bg = null;

			if( chance < ratChance )
			{
				bg = Instantiate( ratPrefab ) as BadGuy;
				bg.SetLife( levelController.GetRatTurns() );
			}
			else
			{
				bg = Instantiate( molePrefab ) as BadGuy;
				bg.SetLife( levelController.GetMoleTurns() );
			}

			var newHomeIndex = (int)Random.Range(0f, buildableCount-0.01f);
			buildableTile[ newHomeIndex ].AddBadGuy( bg, true );
			buildableCount--;
			buildableTile.RemoveAt( newHomeIndex );

		}

		yield return new WaitForSeconds(0.5f);

		if( buildableCount <= 0)
		{
			//Game Over
			Debug.Log("Game Over");
			gController.mGameOver();
		}else{
			var tileLeft = (float)buildableCount / TileGenerator.TotalSize;
			if( tileLeft <= 0.4f || gController.GetLife() == 1)
				sherDog.SetTired( true );
			else
				sherDog.SetTired( false );
		}

		//Check Game State
		if(!isGameOver)
		{
			if(oryorCharge < 100)
				ProductFactory.GetInstance().GenerateProduct(tileList.Any(t=>t.GetBadGuy()!=null));
			else
			{
				oryorCharge = 0;
				oryorGauge.fillAmount = 0;
				ProductFactory.GetInstance().GenerateOryor();
				MainSoundSrc.PlaySound("oryorready");
			}
		}
	}

	public void StartDrag(Selectable product)
	{
		selectedProduct = product;
		if(product == ProductFactory.GetInstance().GetLastGeneratedProduct())
			ProductFactory.GetInstance().UseLastGeneratedProduct();
		else
			slot.UseDeposit();
		slot.SetHilight(true);
	}

	public void DeselectTile(Tile tile)
	{
		if( selectedTile == null || selectedTile != tile )return;
		selectedTile.SetHighlight( false, true );
		selectedTile = null;
		if( selectedProduct is Oryor)
			setTileHint( false, true );
	}

	public void SetSelectTile(Tile tile)
	{
		if( selectedProduct == null )return;
		if( selectedTile == tile ) return;

		if(selectedTile != null) selectedTile.SetHighlight( false, true );
		selectedTile = tile;
		setTileHint(false, true);
		tileHintList.Clear();

		if( selectedTile.IsBuildAble() && (selectedProduct is Product || selectedProduct is Magnifier) ) 
		{
			selectedTile.SetHighlight( true, true );
			Tile[] hints = null;
			if(selectedProduct is Product)
			{
				hints = selectedTile.GetSimilarTile((selectedProduct as Product).Level);
			}
			else if(selectedProduct is Magnifier)
			{
				var highLevel = selectedTile.GetCrossBuildingHighLevel();
				foreach(var level in highLevel)
				{
					hints = selectedTile.GetSimilarTile(level);
					if(hints!=null && hints.Length>=2)break;
				}
			}
			if(hints != null && hints.Length >= 2)tileHintList.AddRange( hints );
			setTileHint(true);
		}else if( selectedProduct is Locker && selectedTile.GetBadGuy() != null )
		{
			selectedTile.SetHighlight( true, true );
		}
		else if( selectedProduct is Oryor)
		{
			tileHintList.AddRange(selectedTile.GetSurroundTiles());
			tileHintList.Add( selectedTile );
			setTileHint( true, true );
		}
	}

	private void setTileHint(bool isOn, bool highlight = false)
	{
		tileHintList.ForEach( t => {
			t.setHighlightProduct( isOn );
			if(highlight) t.SetHighlight( isOn, true );
		});
	}

	private void addScore(int score, Vector3 pos)
	{
		gController.mAddScore( score );
		var scoreText = Instantiate( scorePrefab );
		scoreText.Setup( score, pos );
	}
}
