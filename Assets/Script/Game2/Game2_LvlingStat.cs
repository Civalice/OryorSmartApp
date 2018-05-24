using UnityEngine;
using System.Collections;

[System.Serializable]
public class PigLvling
{
	public float AngryGuageTime = 10.0f;
	public int TokenCount = 4;
	public int MaxToken = 1;
	public int ExtraToken = 0;
	public int ExperienceRequired = 5;
	public float EatModifier = 0.02f;
	public float ExcerciseModifier = 0.01f;
};
public class Game2_LvlingStat : MonoBehaviour {
	public float MoneyMultiplier = 0.02f;
	public float EXPMultiplier = 0.02f;
	public int HardModeLevel = 10;
	public PigLvling[] PigLvlList;

	public static Game2_LvlingStat GameLvlingObject;
	
	public static int currentLevel;
	public static int currentEXP;
	public static int EXPRequired;
	
	public static void AddEXP()
	{
		currentEXP++;
		if (currentEXP >= EXPRequired) {
			currentEXP -= EXPRequired;
			if (currentLevel < GameLvlingObject.PigLvlList.Length-1)
			{
				//lvl up!
				currentLevel++;
			}
		}
	}
	public static PigLvling GetLvling()
	{
		Debug.Log("Get Level = "+currentLevel);
		return GameLvlingObject.PigLvlList [currentLevel];
	}
	public static bool IsMaxLevel(int level)
	{
		return (GameLvlingObject.PigLvlList.Length <= level+1);
	}

	public static int GetEatingScore()
	{
		int combo = Game2Global.GetCombo();
		PigLvling gameLvl = GameLvlingObject.PigLvlList[currentLevel];
		float scoreMultiplier = (1+(gameLvl.EatModifier * combo));
		return (int)(500 * scoreMultiplier);
	}

	public static int GetExcerciseScore()
	{
		int combo = Game2Global.GetCombo();
		PigLvling gameLvl = GameLvlingObject.PigLvlList[currentLevel];
		float scoreMultiplier = (1+(gameLvl.ExcerciseModifier * combo));
		return (int)(10000 * scoreMultiplier);
	}

	public static void ResetGame(bool IsHardMode = false)
	{
		currentLevel = 0;
		if (IsHardMode)
		{
			currentLevel = GameLvlingObject.HardModeLevel-1;
			if (currentLevel > GameLvlingObject.PigLvlList.Length-1)
				currentLevel = GameLvlingObject.PigLvlList.Length-1;
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
