using UnityEngine;
using System.Collections;
namespace Game4
{
	public class Game4Global : MonoBehaviour {
		public static Game4Global pGlobal;
		public static bool GlobalPause = false;
		
		public GlobalController gController;
		public Spawner spawn;
		public SideWaySpawner sideSpawn;

		public float BoosterSpeedPlus = 5.0f;

		float obsLength;
		GameObject obsParent;

		public static GameState GetGameState()
		{
			return pGlobal.gController.state;
		}
		
		public static void AddScore(int score)
		{
			pGlobal.gController.mAddScore (score);
		}
		
		public static void DecreaseLife()
		{
			pGlobal.gController.mDecreaseLifeGuage ();
		}
		
		public static void AddCombo()
		{
			pGlobal.gController.mAddCombo ();
		}
		
		public static void BrokeCombo()
		{
			pGlobal.gController.mBrokeCombo ();
		}
		
		public static int GetCombo()
		{
			return pGlobal.gController.GetCombo();
		}

		public static void ForceSpawnHouse(GoldStarItemObject item)
		{
			pGlobal.sideSpawn.ForceSpawnHouse(item);
		}

		void PreGameStart()
		{
			if (obsParent != null)
			{
				Destroy(obsParent);
				obsParent = null;
			}
			obsParent = new GameObject("GamePlayParent");
		}
		
		void GameReset()
		{
			TurtleCharacter.currentSpeed = 0;
			spawn.Clear();
		}
		
		void GameStart()
		{
			Game4_LvlingStat.ResetGame(gController.mainMenu.Item4.IsSelect);
			TurtleLvling currentLevel = Game4_LvlingStat.GetLvling();
			TurtleCharacter.maxSpeed = currentLevel.speed;
			obsLength = currentLevel.ObstructcleLength / 100.0f;
		}
		
		void GameOver()
		{
			Destroy(obsParent);
			obsParent = null;
		}
		
		void GamePause()
		{
			GlobalPause = true;
			Time.timeScale = 0;
		}
		
		void GameUnPause()
		{
			GlobalPause = false;
			Time.timeScale = 1;
		}

		
		// Use this for initialization
		void Awake() {
			pGlobal = this;
		}
		
		void Start () {
			gController.gResetEvent += GameReset;
			gController.gStartEvent += GameStart;
			gController.gGameOverEvent += GameOver;
			gController.gPauseEvent += GamePause;
			gController.gUnPauseEvent += GameUnPause;
			gController.gCountdownEvent += PreGameStart;
			gController.SetMoneyMultiplier(Game4_LvlingStat.GameLvlingObject.MoneyMultiplier);
			gController.SetEXPMultiplier(Game4_LvlingStat.GameLvlingObject.EXPMultiplier);
			gController.SetGameID(3);
			gController.mGameReset ();
		}
		// Update is called once per frame
		void Update () {
			if (GetGameState() != GameState.GS_PLAY) 
				return;
		}
		
	}
}
