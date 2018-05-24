using UnityEngine;
using System.Collections;
using Game1;

public enum ShellType
{
	SHELL_CAN = 0,
	SHELL_NORMAL,
	SHELL_TEMPLE,
	SHELL_SPIKE,
	SHELL_WING
};

[System.Serializable]
public class SnailLeveling
{
	public ShellType Shell;
	public float CatcherSpeed = 1.0f;
	public float CatcherIdleTime = 2.0f;
	public int MoleMax = 3;
	public float RightMoleRate = 1.0f;
	public float WrongMoleRate = 2.0f;
	public float DangerMoleRate = 0.0f;
	public int ExperienceRequired = 5;
	public float RightModifier = 1.0f;
	public float TouchModifier = 1.0f;
	public float MinWaitTime = 1.0f;
	public float MaxWaitTime = 3.0f;
};

public class Game1_Leveling : MonoBehaviour {
	public float MoneyMultiplier = 0.02f;
	public float EXPMultiplier = 0.02f;
	public int HardModeLevel = 10;

	public SnailLeveling[] SnailLevelList;
	public Sprite[] ShellList;
	public AudioClip WrongSound;
	public AudioClip RightSound;

	public static Game1_Leveling GameLvlingObject;

	public static int currentLevel;
	public static int currentEXP;
	public static int EXPRequired;

	public static void PlayScoreSound(bool IsRight)
	{
		GameLvlingObject.GetComponent<AudioSource>().PlayOneShot(IsRight?GameLvlingObject.RightSound:
		                                                         GameLvlingObject.WrongSound);
	}

	public static Sprite[] GetShellList()
	{
		return GameLvlingObject.ShellList;
	}

	public static SnailLeveling GetLvling()
	{
		return GameLvlingObject.SnailLevelList [currentLevel];
	}
	public static bool IsCatcherMaxLevel(int level)
	{
		return (GameLvlingObject.SnailLevelList.Length <= level+1);
	}

	public static void AddEXP()
	{
		currentEXP++;
		if (currentEXP >= EXPRequired) {
			currentEXP -= EXPRequired;
			if (currentLevel < GameLvlingObject.SnailLevelList.Length-1)
			{
				//lvl up!
				GameLvlingObject.GetComponent<AudioSource>().Play();
				currentLevel++;
			}
		}
	}

	public static int GetTouchMoleScore()
	{
		//get combo and level to calculate score
		int ComboCount = GlobalGuage.GetCombo();
		SnailLeveling gameLvl = GameLvlingObject.SnailLevelList[currentLevel];
		float scoreMultiplier = (1+(gameLvl.TouchModifier * ComboCount));
		return (int)(500 * scoreMultiplier);
	}

	public static int GetCatchMoleScore()
	{
		//get combo and level to calculate score
		int ComboCount = GlobalGuage.GetCombo();
		SnailLeveling gameLvl = GameLvlingObject.SnailLevelList[currentLevel];
		float scoreMultiplier = (1+(gameLvl.RightModifier * ComboCount));
		return (int)(10000 * scoreMultiplier);
	}

	public static void ResetGame(bool IsHardMode = false)
	{
		currentLevel = 0;
		if (IsHardMode)
		{
			currentLevel = GameLvlingObject.HardModeLevel-1;
			if (currentLevel > GameLvlingObject.SnailLevelList.Length-1)
				currentLevel = GameLvlingObject.SnailLevelList.Length-1;
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
