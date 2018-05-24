using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[SCORETAG] use for Score Indexing
public class Game2Global : MonoBehaviour {
	public static Game2Global pGlobal;

	public GlobalController gController;

	public SpawnPoint spawnPoint;
	public GameObject EatingPoint;
	public PigCharacter pig;
	public BottomBarGDA GDABar;
	public AngryGuage guage;
	public List<int> tokenFlag;
	public int ExtraTokenCount;
	public int lastestExtraToken;

	private int lastestTokenCount;

	public static bool GlobalPause = false;

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

	void GameReset()
	{
		//clear all Food Item
		FoodItem.ClearFoodList();
		FoodItem.IsMoving = true;
		guage.gameObject.SetActive (false);
		GDABar.ClearFlag();
		tokenFlag.Clear ();
	}
	
	void GameStart()
	{
		Game2_LvlingStat.ResetGame (gController.mainMenu.Item4.IsSelect);
		PigLvling currentLvl = Game2_LvlingStat.GetLvling();
		guage.SetTimer(currentLvl.AngryGuageTime);
		lastestTokenCount = currentLvl.TokenCount;
		lastestExtraToken = currentLvl.ExtraToken;
		ExtraTokenCount = lastestExtraToken;
		for (int i = 0;i < currentLvl.TokenCount;i++)
		{
			for (int j = 1;j <= 4;j++)
			{
				tokenFlag.Insert(Random.Range(0,tokenFlag.Count),j);
			}
		}
		spawnPoint.FirstSpawn ();
		guage.StartPlayGame();
		guage.gameObject.SetActive (true);
	}
	
	void GameOver()
	{
		//stop angry guage
		FoodItem.ClearFoodList();
		guage.GameOver();
	}

	void GamePause()
	{
		GlobalPause = true;
		pig.Pause();
	}

	void GameUnPause()
	{
		GlobalPause = false;
		pig.UnPause();
	}

	public static int GetExtraToken()
	{
		//process extra Token
		PigLvling currentLvl = Game2_LvlingStat.GetLvling();
		int randomRange = (currentLvl.MaxToken-1>pGlobal.ExtraTokenCount)?
			pGlobal.ExtraTokenCount+1:
				currentLvl.MaxToken;
		int token = Random.Range (0, randomRange);
		pGlobal.ExtraTokenCount -= token;
		return token;
	}
	public static void ReturnExtraToken(int token)
	{
		pGlobal.ExtraTokenCount += token;
	}
	public static void Spawning()
	{
		pGlobal.spawnPoint.Spawn ();
	}
	public static Vector3 GetEatingPoint()
	{
		return pGlobal.EatingPoint.transform.position;
	}
	public static void SetSick(bool flag)
	{
		pGlobal.guage.setSick (flag);
	}
	public static void PigEating(int flag)
	{
		if (pGlobal.GDABar.EatFlag (flag)) {
			if (!pGlobal.GDABar.FullFlagUpdate())
			{
				PigLvling currentLvl = Game2_LvlingStat.GetLvling();
				pGlobal.guage.SetTimer(currentLvl.AngryGuageTime);
				//[SCORETAG]
				AddScore(Game2_LvlingStat.GetEatingScore());
				AddCombo();
				pGlobal.pig.Eating ();
			}
			else
			{
				Game2_LvlingStat.AddEXP();
				pGlobal.calculateNewToken();
				pGlobal.calculateExtraToken();
				PigLvling currentLvl = Game2_LvlingStat.GetLvling();
				pGlobal.guage.SetTimer(currentLvl.AngryGuageTime);
				//[SCORETAG]
				AddScore(Game2_LvlingStat.GetExcerciseScore());
				AddCombo();
				pGlobal.pig.Exercise();
			}
		} 
		else {
			PigLvling currentLvl = Game2_LvlingStat.GetLvling();
			pGlobal.guage.SetTimer(currentLvl.AngryGuageTime);
			BrokeCombo();
			DecreaseLife();
			pGlobal.pig.Sick();
		}
	}

	public static void PigAngry()
	{
		DecreaseLife();
		pGlobal.pig.Angry ();
	}

	public static bool PigIsSick()
	{
		return pGlobal.pig.IsSick;
	}

	public static int GetRandomPackageFlag(int token)
	{
		int flag = 0;
		for (int i = 0; i < token; i++) {
			int k = 0;
			int bw = pGlobal.tokenFlag[k];
			while ((flag & (1<<bw))!=0)
			{
				bw = pGlobal.tokenFlag[++k];
			}
			flag |= 1<<bw;
			pGlobal.tokenFlag.RemoveAt(k);

		}
		return flag;
	}

	public static void ReturnFlag(int flag)
	{
		for (int i = 1;i <= 4;i++)
		if ((flag & (1 << i))!=0) {
			pGlobal.tokenFlag.Insert(Random.Range(0,pGlobal.tokenFlag.Count),i);
		}
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
		gController.SetMoneyMultiplier(Game2_LvlingStat.GameLvlingObject.MoneyMultiplier);
		gController.SetEXPMultiplier(Game2_LvlingStat.GameLvlingObject.EXPMultiplier);
		gController.SetGameID(1);
		gController.mGameReset ();
	}

	public void calculateNewToken()
	{
		PigLvling currentLvl = Game2_LvlingStat.GetLvling();
		int currentToken = currentLvl.TokenCount;
		if (currentToken > lastestTokenCount) {
					
			for (int i = 0; i < currentToken-lastestTokenCount; i++) {
			
				for (int j = 1; j <= 4; j++) {
					tokenFlag.Insert (Random.Range (0, tokenFlag.Count), j);
				
				}

			}

		} else {
		
			for (int i = 0; i < lastestTokenCount-currentToken; i++) {
			
				for (int j = 1; j <= 4; j++) {
				
					tokenFlag.Remove(j);

				}

			}

		}
		lastestTokenCount = currentToken;
	}

	public void calculateExtraToken()
	{
		PigLvling currentLvl = Game2_LvlingStat.GetLvling();
		int currentExtraToken = currentLvl.ExtraToken;
		ExtraTokenCount += currentExtraToken-lastestExtraToken;
		lastestExtraToken = currentExtraToken;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
