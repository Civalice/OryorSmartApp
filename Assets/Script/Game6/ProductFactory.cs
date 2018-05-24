using UnityEngine;
using System.Collections;
using System.Linq;

public class ProductFactory : MonoBehaviour {

	private static ProductFactory inst;

	[SerializeField]
	private GameObject product;
	
	[SerializeField]
	private Locker lockerPrefab;
	
	[SerializeField]
	private Magnifier magPrefab;

	[SerializeField]
	private Oryor oryorPrefab;

	[SerializeField]
	private Game6LevelController levelController;

	[SerializeField]
	private GameObject oryorSign;

	private Selectable lastGeneratedSelectable;

	private int totalProductChance = 0;

	void Awake()
	{
		inst = this;
	}

	public static ProductFactory GetInstance()
	{
		return inst;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenerateOryor()
	{
		var oryor = Instantiate(oryorPrefab);
		oryor.SetClickable(true);
		oryor.transform.parent = this.transform;
		//oryor.transform.localPosition = Vector3.zero;
		oryor.transform.position = oryorSign.transform.position;

		StartCoroutine( Util.SmoothMovement( oryor.transform, transform.position, 200f ) );

		lastGeneratedSelectable = oryor;
	}

	public Selectable GetLastGeneratedProduct()
	{
		return lastGeneratedSelectable;
	}

	public void GenerateProduct(bool hasBadGuy = false)
	{
		if(lastGeneratedSelectable != null) return;

		var productChances = levelController.GetProductChances();
		var totalProductChance = productChances.Sum();
		var magChance = levelController.GetMagChance();
		var lockerChance = levelController.GetLockerChance();

		var totalChance = (hasBadGuy) ? totalProductChance + magChance + lockerChance : totalProductChance + magChance;

		Selectable p = null;

		var ranNum = Random.Range(1, totalChance);
		if(ranNum < totalProductChance )
		{
			var level = 1;
			var currentLevelChance = 0;
			for(level = 1; level <= productChances.Length; level++)
			{
				currentLevelChance += productChances[level-1];
				if( currentLevelChance > ranNum )break;
			}
			p = CreateProduct(level, true);
		}else if(ranNum < totalProductChance + magChance)
		{
			p = Instantiate( magPrefab );
			p.SetClickable(true);
		}
		else
		{
			p = Instantiate( lockerPrefab );
			p.SetClickable(true);
		}

		p.transform.parent = this.transform;
		p.transform.localPosition = Vector3.zero;

		lastGeneratedSelectable = p;
	}

	public void UseLastGeneratedProduct()
	{
		lastGeneratedSelectable = null;
	}

	public void AddLastGeneratedProduct(Selectable add)
	{
		lastGeneratedSelectable = add;
	}

	public Selectable SwapProdcut(Selectable addingProduct)
	{
		addingProduct.transform.parent = transform;
		addingProduct.transform.localPosition = Vector3.zero;
		addingProduct.transform.localScale = Vector3.one;
		//addingProduct.SetClickable(false);
		var withdrawProduct = lastGeneratedSelectable;
		if(withdrawProduct != null) withdrawProduct.SetClickable(true);
		lastGeneratedSelectable = addingProduct;
		
		return withdrawProduct;
	}

	public Product CreateProduct(int level, bool isClickable = false)
	{
		var p = Instantiate( product ).GetComponent<Product>();
		p.Setup( level, isClickable );
		return p;
	}

	public void DestroyLastGenerated()
	{
		if(lastGeneratedSelectable != null)
		{
			Destroy(lastGeneratedSelectable.gameObject);
			lastGeneratedSelectable = null;
		}
	}

}
