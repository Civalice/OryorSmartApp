using UnityEngine;
using System.Collections;
namespace Game3
	{
	[System.Serializable]
	public class SpriteList
	{
		public Sprite[] spriteList;
	}

	public class Game3Global : MonoBehaviour {
		public static Game3Global pGlobal;
		
		public GlobalController gController;
		
		public GameObject spawnPoint;
		public GameObject airplane;
		public GameObject bird;
		public GameObject HPNortify;

		public SpriteList[] parashootObjectList;
		public Sprite parashootSymbolDrug;
		public Sprite parashootSymbolFood;
		public Sprite parashootSymbolLip;
		public Sprite parashootSymbolMedic;
		public Sprite parashootSymbolPoison;
		public TrashObject trash;

		public GameObject ParashootPrefabs;

		public static bool GlobalPause = false;
		//Touch Global
		public ParashootObject dragObject = null;
		public DragItemBox boxObject = null;
		public static bool IsTouch = false;

		float timer;
		GameObject gameplayItem = null;

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

		public static void TrashEnter()
		{
			pGlobal.trash.TrashEnter();
		}

		public static void BirdOK()
		{
			pGlobal.bird.GetComponent<Animator>().Play("bird_ok",-1,0f);
		}
		public static void BirdWrong()
		{
			GameObject hpObj = GameObject.Instantiate(pGlobal.HPNortify) as GameObject;
			hpObj.SetActive(true);
			hpObj.transform.position = pGlobal.bird.transform.position;
			hpObj.GetComponent<HPNotification>().DecreaseLife(1);
			pGlobal.bird.GetComponent<Animator>().Play("bird_wrong",-1,0f);
		}

		void AddWave()
		{
			MainSoundSrc.PlaySound("airplane");
			airplane.GetComponent<Animator>().Play("AirplaneFly",-1,0f);
			bird.GetComponent<Animator>().Play("bird_airplane",-1,0f);
			timer = Game3_LvlingStat.GetLvling().airplaneDelayed;
			GameObject gobj = new GameObject("Wave");
			ParashootWave wave = gobj.AddComponent<ParashootWave>();
			wave.SetupWave(gameplayItem);
		}

		void GameReset()
		{
			dragObject = null;
			Destroy(gameplayItem);
			timer = 0;
		}
		
		void GameStart()
		{
			Game3_LvlingStat.ResetGame(gController.mainMenu.Item4.IsSelect);
			gameplayItem = new GameObject("GameItem");
			gameplayItem.transform.position = spawnPoint.transform.position;
		}
		
		void GameOver()
		{
			dragObject = null;
			Destroy(gameplayItem);
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
			gController.SetMoneyMultiplier(Game3_LvlingStat.GameLvlingObject.MoneyMultiplier);
			gController.SetEXPMultiplier(Game3_LvlingStat.GameLvlingObject.EXPMultiplier);
			gController.SetGameID(2);
			gController.mGameReset ();
		}
		// Update is called once per frame
		void Update () {
			if (pGlobal.gController.state == GameState.GS_PLAY) {
					timer -= Time.deltaTime;
					//popup airplane and delayed
					if (timer <= 0) {
							timer += Game3_LvlingStat.GetLvling ().airplaneDelayed;
							AddWave ();
					}
			}
		}

		public void AddGameplayObject(GameObject obj)
		{
			obj.transform.parent = gameplayItem.transform;
		}
	}
}
