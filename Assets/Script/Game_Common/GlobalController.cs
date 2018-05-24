using UnityEngine;
using System.Collections;
using TMPro;

public enum GameState
{
	GS_STARTMENU = 0,
	GS_TUTORIAL,
	GS_COUNTDOWN,
	GS_PLAY,
	GS_PAUSE,
	GS_RESULT,
};

public class GlobalController : MonoBehaviour {
	public static GlobalController pGlobal;

	public delegate void EventAction ();
	public event EventAction gResetEvent;
	public event EventAction gStartEvent;
	public event EventAction gGameOverEvent;
	public event EventAction gPauseEvent;
	public event EventAction gUnPauseEvent;
	public event EventAction gCountdownEvent;

	public GameState state = GameState.GS_STARTMENU;

	public int lifeCount = 4;

	public HeartTank lifeObject;
	public TextMeshPro scoreText;
	public ComboUI mCombo;
	public StartMenu mainMenu;
	public GameResult resultMenu;
	public TextMeshPro faceText;
	public Collider2D PauseBtn;
	public GameObject pauseMenu;
	//Pause Button
	public GamePlayButton ResumeButton;
	public GamePlayButton RestartButton;
	public GamePlayButton ExitButton;

	public string GameName = "Game";
	public Sprite GameIcon;
	[System.NonSerialized]
	private MorphObject morphScore = new MorphObject();

	private float MoneyMultiplier;
	private float EXPMultiplier;
	private string GameID;
	private int life;
	private int Combo = 0;
	private int MaxCombo = 0;
	private int hit = 0;
	
	private int Score = 0;

	private long startTick;
	private long accumulateTick;

	public static GameState getState()
	{
		return pGlobal.state;
	}
	public static void setState(GameState state)
	{
		pGlobal.state = state;
	}

	public void SetMoneyMultiplier(float mul)
	{
		MoneyMultiplier = mul;
	}
	public void SetEXPMultiplier(float mul)
	{
		EXPMultiplier = mul;
	}

	public void mGameReset()
	{
		Score = 0;
		Combo = 0;
		MaxCombo = 0;
		hit = 0;
		string scTxt = Score.ToString("#,##0");
		scoreText.text = scTxt;
		mCombo.gameObject.SetActive (false);
		life = lifeCount;
		lifeObject.ClearTank ();
		if (gResetEvent != null)
			gResetEvent ();
		state = GameState.GS_STARTMENU;
	}
	
	public void mGameCountDown()
	{
		if (gCountdownEvent!=null)
			gCountdownEvent();
		StartCoroutine ("CountDown");
	}

	public void mGameMainMenu()
	{
		mGameReset();
		resultMenu.Hide ();
		mainMenu.Show ();
	}

	public void mGameRestart()
	{

		mGameReset ();
		mGameUnPause();
		mainMenu.Show();
		state = GameState.GS_STARTMENU;
	}

	public void mExitGame()
	{
		mGameUnPause();
		mGameReset();
		LoadingScript.ChangeScene("GameLand");
	}

	public void mGamePause()
	{
		if (state != GameState.GS_PLAY) return;
		//Stop [TICK] here
		long currentTick = System.DateTime.UtcNow.Ticks;
		accumulateTick += currentTick-startTick;
		state = GameState.GS_PAUSE;
		pauseMenu.SetActive(true);
		if (gPauseEvent!=null)
			gPauseEvent();
	}

	public void mGameUnPause()
	{
		//Accumulate and restart [TICK] here
		startTick = System.DateTime.UtcNow.Ticks;
		state = GameState.GS_PLAY;
		pauseMenu.SetActive(false);
		if (gUnPauseEvent!=null)
			gUnPauseEvent();
	}

	public void mGameOver()
	{
		//Stop [TICK] count here
		long currentTick = System.DateTime.UtcNow.Ticks;
		accumulateTick += currentTick - startTick;
		//calculate time from accumulateTick
		double playTime = System.TimeSpan.FromTicks(accumulateTick).TotalSeconds;
		Debug.Log("Play Time = "+playTime);
		state = GameState.GS_RESULT;
		if (gGameOverEvent != null)
			gGameOverEvent ();

		int current_money = int.Parse(UserCommonData.pGlobal.user.user_money)+mainMenu.moneyUsed;

		//Item 1 Life up
		//Item 2 Score up
		//Item 3 EXP up

		resultMenu.ResetResult(int.Parse(UserCommonData.pGlobal.user.user_level),
		                       UserCommonData.pGlobal.GetEXPProgress(),current_money,
		                       mainMenu.Item1.IsSelect&&(mainMenu.Item1.ItemCount>0),
		                       mainMenu.Item2.IsSelect&&(mainMenu.Item2.ItemCount>0),
		                       mainMenu.Item3.IsSelect&&(mainMenu.Item3.ItemCount>0),
		                       mainMenu.moneyUsed,
		                       (int)playTime);

		float rawMoney = (Score * MoneyMultiplier)*(1+(float.Parse(UserCommonData.pGlobal.user.user_coin_plus))/100.0f);
		float rawEXP = (Score * EXPMultiplier)*((mainMenu.Item3.IsSelect)?2:1)*(1+(float.Parse(UserCommonData.pGlobal.user.user_exp_plus))/100.0f);

		resultMenu.SetGameResultData(Score,
		                             int.Parse(UserCommonData.pGlobal.user.user_score_plus)+((mainMenu.Item2.IsSelect)?100:0),
		                             MaxCombo,
		                             rawMoney,
		                             rawEXP,
		                             hit);
		resultMenu.Show();
	}

	public int GetCombo()
	{
		return Combo;
	}

	void mGameStart()
	{
		mCombo.gameObject.SetActive (true);
		mCombo.setCombo (Combo);
		if (gStartEvent != null)
			gStartEvent ();
	}
	
	IEnumerator CountDown()
	{
		accumulateTick = 0;
		life = lifeCount+((mainMenu.Item1.IsSelect)?1:0);
		lifeObject.SetupHeart (life);
		state = GameState.GS_COUNTDOWN;
		faceText.gameObject.SetActive (true);
		faceText.text = "READY";
		yield return new WaitForSeconds (1.0f);
		faceText.text = "3";
		yield return new WaitForSeconds (1.0f);
		faceText.text = "2";
		yield return new WaitForSeconds (1.0f);
		faceText.text = "1";
		yield return new WaitForSeconds (1.0f);
		faceText.text = "START";
		yield return new WaitForSeconds (1.0f);
		faceText.text = "PAUSE";
		faceText.gameObject.SetActive (false);
		state = GameState.GS_PLAY;
		mGameStart ();
		startTick = System.DateTime.UtcNow.Ticks;
		//start [TICK] count here
		yield return null;
	}
	
	IEnumerator ScoreRunning()
	{
		morphScore.morphEaseout (Score, 30.0f);
		while (!morphScore.IsFinish()) {
			morphScore.Update();
			int val = (int)morphScore.val;
			string scTxt = val.ToString("#,##0");
			scoreText.text = scTxt;
			yield return null;
		}
	}

	public int GetLife()
	{
		return life;
	}

	public void mDecreaseLifeGuage()
	{
		//PlayAnimation Decrease Life
		lifeObject.DecreaseLife ();
		life--;
		
		if (life <= 0) {
			life = 0;
			mGameOver();
		}
	}
	public void mAddCombo()
	{
		hit++;
		Combo++;
		if (Combo > MaxCombo) 
			MaxCombo = Combo;
		mCombo.setCombo (Combo);
	}
	public void mBrokeCombo()
	{
		Combo = 0;
		mCombo.setCombo (Combo);
	}
	public void mAddScore(int score)
	{
		Score += score;
		//make score running
		StopCoroutine ("ScoreRunning");
		StartCoroutine ("ScoreRunning");
	}

	public void SetGameID(int id)
	{
		GameID = id.ToString();
		Debug.Log ("GlobalController SetGameID : "+GameID);
		resultMenu.SetGameID(GameID);
	}

	void Awake()
	{
		pGlobal = this;
	}
	// Use this for initialization
	void Start () {
		mainMenu.startButton += mGameCountDown;
		resultMenu.replayButton += mGameMainMenu;
		ResumeButton.OnReleased += mGameUnPause;
		RestartButton.OnReleased += mGameRestart;
		ExitButton.OnReleased += mExitGame;
		mainMenu.SetGameLogo(GameIcon,GameName);
		resultMenu.SetGameLogo(GameIcon);
	}
	
	// Update is called once per frame
	void Update () {
		if ((state != GameState.GS_PLAY)&&(state != GameState.GS_PAUSE))
			return;
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		if (TouchDown) {
			if (PauseBtn.OverlapPoint (pos)) {
				if (state == GameState.GS_PLAY)
				{
					mGamePause();
				}
				else if (state == GameState.GS_PAUSE)
				{
					mGameUnPause();
				}
			}
		}
	}
}
