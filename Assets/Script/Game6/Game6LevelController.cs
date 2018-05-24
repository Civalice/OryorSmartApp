using UnityEngine;
using System.Collections;

public class Game6LevelController : MonoBehaviour
{
	[SerializeField]
	private int[] ProductCreationScores;
	[SerializeField]
	private int[] ProductCombinationScores;
	[SerializeField]
	private int RatKillingScore;
	[SerializeField]
	private int MoleKillingScore;
	[SerializeField]
	private float OryorMultiplier;
	[SerializeField]
	private int[] OryorChargingRates;
	[SerializeField]
	private LevelData[] levelData;

	private int gameLevel = 0, currentTurn = 0;

	public void Reset()
	{
		gameLevel = 0;
		currentTurn = 0;
	}

	public int GetCreateProductScore(int level)
	{
		return ProductCreationScores[ level - 1 ];
	}

	public int GetCombineProductScore(int level, int num)
	{
		return (int)(ProductCombinationScores[ level - 1 ] * num * currentLevelData.Multiplier );
	}

	public int GetKillRatScore()
	{
		return RatKillingScore;
	}

	public int GetKillMoleScore()
	{
		return MoleKillingScore;
	}

	public float GetOryorMultiplier()
	{
		return OryorMultiplier;
	}

	public int GetOryorScore(Product[] products)
	{
		var score = 0;
		foreach(var p in products)
		{
			score += ProductCombinationScores[ p.Level - 1 ];
		}

		return (int)(score * OryorMultiplier);
	}

	public int[] GetProductChances()
	{
		return currentLevelData.ProdcutChances;
	}

	public int GetMagChance()
	{
		return currentLevelData.MagnifierChance;
	}

	public int GetLockerChance()
	{
		return currentLevelData.LockerChance;
	}

	public int GetMoleChance(int moleNum)
	{
		if ( moleNum >= currentLevelData.MoleMax ) return 0;
		return currentLevelData.MoleChance;
	}

	public int GetMoleTurns()
	{
		return currentLevelData.MoleTurns;
	}

	public int GetRatChance(int ratNum)
	{
		if ( ratNum >= currentLevelData.RatMax ) return 0;
		return currentLevelData.RatChance;
	}

	public int GetRatTurns()
	{
		return currentLevelData.RatTurns;
	}

	public int GetOryorChargingRate( int level )
	{
		return OryorChargingRates[level-1];
	}

	public void AddTurn()
	{
		currentTurn++;
		if( gameLevel + 1 < levelData.Length && currentTurn >= currentLevelData.LevelEndsAtMoveNumber )
		{
			gameLevel++;
			Debug.Log("LevelUp Lv:"+gameLevel+" turn:"+currentTurn);
		}
	}

	private LevelData currentLevelData{
		get{
			return levelData[ gameLevel ]; 
		}
	}

	[System.Serializable]
	private class LevelData
	{
		public int LevelEndsAtMoveNumber;
		public float Multiplier;
		public int[] ProdcutChances;
		public int LockerChance;
		public int MagnifierChance;

		public int MoleChance;
		public int MoleTurns;
		public int MoleMax;
		public int RatChance;
		public int RatTurns;
		public int RatMax;

	}
}

