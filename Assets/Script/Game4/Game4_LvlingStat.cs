using UnityEngine;
using System.Collections;
using Game4;
//namespace Game4
//{
	public class Game4_LvlingStat : MonoBehaviour {
		public float MoneyMultiplier = 0.02f;
		public float EXPMultiplier = 0.02f;
		public int HardModeLevel = 10;

		public TurtleLvling[] TurtleLvlList;
		
		public static Game4_LvlingStat GameLvlingObject;
		
		public static int currentLevel;
		public static int currentEXP;
		public static int EXPRequired;
		
		public static void AddEXP()
		{
			currentEXP++;
			if (currentEXP >= EXPRequired) {
				currentEXP -= EXPRequired;
				if (currentLevel < GameLvlingObject.TurtleLvlList.Length-1)
				{
					//lvl up!
					currentLevel++;
					TurtleLvling lvling = GetLvling();
					TurtleCharacter.maxSpeed = lvling.speed;

				}
			}
		}
		public static TurtleLvling GetLvling()
		{
			return GameLvlingObject.TurtleLvlList [currentLevel];
		}
		public static bool IsMaxLevel(int level)
		{
			return (GameLvlingObject.TurtleLvlList.Length <= level+1);
		}
		
		public static int GetStarScore()
		{
			int combo = Game4Global.GetCombo();
			TurtleLvling gameLvl = GameLvlingObject.TurtleLvlList[currentLevel];
			float scoreMultiplier = (1+(gameLvl.Multiplier1 * combo));
			return (int)(gameLvl.StarPoint * scoreMultiplier);
		}
			
		public static int GetGoldStarScore()
		{
			int combo = Game4Global.GetCombo();
			TurtleLvling gameLvl = GameLvlingObject.TurtleLvlList[currentLevel];
			float scoreMultiplier = (1+(gameLvl.Multiplier2 * combo));
			return (int)(gameLvl.GoldStarPoint * scoreMultiplier);
		}
		
		public static void ResetGame(bool IsHardMode = false)
		{
			currentLevel = 0;
			if (IsHardMode)
			{
				currentLevel = GameLvlingObject.HardModeLevel-1;
				if (currentLevel > GameLvlingObject.TurtleLvlList.Length-1)
					currentLevel = GameLvlingObject.TurtleLvlList.Length-1;
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
	[System.Serializable]
	public class TurtleLvling
	{
		public float speed;
		public int ObstructcleNumber = 1;
		public float ObstructcleLength = 300;
		public float StarLength = 400;
		public float GoldStarLength = 1500;
		public float BoosterLength = 3000;
		public float RandomNoise = 1.1f;
		public int StarPoint = 300;
		public int GoldStarPoint = 2000;
		public float Multiplier1 = 0.05f;
		public float Multiplier2 = 0.05f;
		public int ExperienceRequired = 5;
	}
//}