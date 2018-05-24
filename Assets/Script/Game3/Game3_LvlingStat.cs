using UnityEngine;
using System.Collections;
using Game3;
[System.Serializable]
public class DragLvling
{
	public float minParashootSpeed = 1;
	public float maxParashootSpeed = 2;
	public int CatagoryCount = 1;
	public int ParashootCount = 7;
	public float minParashootLength = 0.5f;
	public float maxParashootLength = 1;
	public float airplaneDelayed = 10;
	public float Multiplier1 = 0;
	public float Multiplier2 = 0;
	public int ExperienceRequired = 5;
}

public class Game3_LvlingStat : MonoBehaviour {
	public int parashootScore = 300;
	public int waveScore = 10000;

	public float MoneyMultiplier = 0.02f;
	public float EXPMultiplier = 0.02f;
	public int HardModeLevel = 10;
	public DragLvling[] DragLvlList;
	
	public static Game3_LvlingStat GameLvlingObject;
	
	public static int currentLevel;
	public static int currentEXP;
	public static int EXPRequired;
	
	public static void AddEXP()
	{
		currentEXP++;
		if (currentEXP >= EXPRequired) {
			currentEXP -= EXPRequired;
			if (currentLevel < GameLvlingObject.DragLvlList.Length-1)
			{
				//lvl up!
				currentLevel++;
			}
		}
	}
	public static DragLvling GetLvling()
	{
		return GameLvlingObject.DragLvlList [currentLevel];
	}
	public static bool IsMaxLevel(int level)
	{
		return (GameLvlingObject.DragLvlList.Length <= level+1);
	}
	
	public static int GetParashootScore()
	{
		int combo = Game3Global.GetCombo();
		DragLvling gameLvl = GameLvlingObject.DragLvlList[currentLevel];
		float scoreMultiplier = (1+(gameLvl.Multiplier1 * combo));
		return (int)(500 * scoreMultiplier);
	}
	
	public static int GetLvlupScore()
	{
		int combo = Game3Global.GetCombo();
		DragLvling gameLvl = GameLvlingObject.DragLvlList[currentLevel];
		float scoreMultiplier = (1+(gameLvl.Multiplier2 * combo));
		return (int)(10000 * scoreMultiplier);
	}
	
	public static void ResetGame(bool IsHardMode = false)
	{
		currentLevel = 0;
		if (IsHardMode)
		{
			currentLevel = GameLvlingObject.HardModeLevel-1;
			if (currentLevel > GameLvlingObject.DragLvlList.Length-1)
				currentLevel = GameLvlingObject.DragLvlList.Length-1;
		}

		currentEXP = 0;
		EXPRequired = GetLvling ().ExperienceRequired;
	}
	// Use this for initialization
	void Awake () {
		GameLvlingObject = this;
		ResetGame ();
	}
}
