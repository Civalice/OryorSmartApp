using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
namespace Game1
{

	public class GlobalGuage : MonoBehaviour {
		public int snailCount = 1;

		public GameObject CatcherPrefab;
		public GlobalController gController;
		
		public static GlobalGuage lgObject;
		public static bool GlobalPause = false;

		public static GameObject GetCatcherPrefab()
		{
			return lgObject.CatcherPrefab;
		}

		public static GameState GetGameState()
		{
			return lgObject.gController.state;
		}
		
		public static void AddScore(int score)
		{
			lgObject.gController.mAddScore (score);
		}

		public static void DecreaseLife()
		{
			lgObject.gController.mDecreaseLifeGuage ();
		}

		public static void AddCombo()
		{
			lgObject.gController.mAddCombo ();
		}

		public static void BrokeCombo()
		{
			lgObject.gController.mBrokeCombo ();
		}

		public static int GetCombo()
		{
			return lgObject.gController.GetCombo();
		}

		public static void DangerMole()
		{
			lgObject.gController.mDecreaseLifeGuage ();
			lgObject.gController.mDecreaseLifeGuage ();
//			lgObject.gController.mGameOver();
		}
		void GameReset()
		{
			GlobalPause = false;
			MoleSpawner.GameStop();
			Catcher.ClearCatcher ();
			MoleSpawner.ClearHole ();
		}

		void GameStart()
		{
			Game1_Leveling.ResetGame (gController.mainMenu.Item4.IsSelect);
//			MoleSpawner.ClearHole ();
			MoleSpawner.GameStart();
			Catcher.CreateCatcher (snailCount);
		}

		void GameOver()
		{
			Catcher.ClearCatcher ();
			MoleSpawner.ClearHole ();
			MoleSpawner.GameStop();
			//set HighScore
		}
		void GamePause()
		{
			MoleSpawner.GameStop();
			GlobalPause = true;
		}
		void GameUnPause()
		{
			MoleSpawner.GameStart();
			GlobalPause = false;
		}
		
		void Awake()
		{
			lgObject = this;
		}
		// Use this for initialization
		void Start () 
		{
			gController.gResetEvent += GameReset;
			gController.gStartEvent += GameStart;
			gController.gGameOverEvent += GameOver;
			gController.gPauseEvent += GamePause;
			gController.gUnPauseEvent += GameUnPause;
			gController.SetMoneyMultiplier(Game1_Leveling.GameLvlingObject.MoneyMultiplier);
			gController.SetEXPMultiplier(Game1_Leveling.GameLvlingObject.EXPMultiplier);
			gController.SetGameID(4);
			gController.mGameReset ();
		}
		
		// Update is called once per frame
		void Update () 
		{
		}
	}
}