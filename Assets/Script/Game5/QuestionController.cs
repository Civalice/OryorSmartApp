using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour {

	public enum QuestionState
	{
		Init,
		GenerateQuestion,
		WaitingForAnswer,
		Result,
		Pause,
		Finish,
	}

	[SerializeField]
	private Answer[] answers;

	[SerializeField]
	private InfoSign infoSign;

	[SerializeField]
	private GlobalController gController;
	[SerializeField]
	private Game5LevelController levelContoller;

	[SerializeField]
	private Image gauge;

	[SerializeField]
	private HPNotification HPNotifyPrefab;

	[SerializeField]
	private Cat cat;

	private InfoSign.SignType currentQuestionType = InfoSign.SignType.Food;
	private int answerLeft = 1, currentLevel = 1;
	private float timeLeft = 10f, startTime = 10f;
	private List<Answer> activateAnswerList = new List<Answer>();

	public QuestionState State{
		get;
		private set;
	}

	private static QuestionController inst;

	public static QuestionController GetInstance()
	{
		return inst;
	}

	void Awake()
	{
		inst = this;
	}

	// Use this for initialization
	void Start () {
		State = QuestionState.Init;
		gController.gResetEvent += gameReset;
		gController.gStartEvent += gameStart;
		gController.gGameOverEvent += gameOver;
		gController.gPauseEvent += gamePause;
		gController.gUnPauseEvent += gameResume;
		//gController.gCountdownEvent += PreGameStart;

		gController.SetMoneyMultiplier(1);
		gController.SetEXPMultiplier(1);
		gController.SetGameID(5);
		gController.mGameReset();

		//gController.mGameCountDown();
	}

	void gameStart()
	{
		currentLevel = 1;
		cat.AwakeUpNow();
		State = QuestionState.GenerateQuestion;
		setupActivateAnswer( getTotalAnswer(currentLevel) );
		StartCoroutine(generateQuestion());
	}

	void gameReset()
	{
		currentLevel = 1;
		if(State == QuestionState.Init)return;
		State = QuestionState.Finish;
		StopAllCoroutines();
		cat.AwakeUpNow();
		closeAllAnswer();
		infoSign.Close();
	}

	void gameOver()
	{
		State = QuestionState.Finish;
		closeAllAnswer();
		infoSign.Close();
	}

	private QuestionState previousState;

	void gamePause()
	{
		previousState = State;
		State = QuestionState.Pause;
	}

	void gameResume()
	{
		if(State != QuestionState.Finish)
			State = previousState;
	}
	
	// Update is called once per frame
	void Update () {
		if(State == QuestionState.WaitingForAnswer)
		{
			timeLeft -= Time.deltaTime;

			gauge.fillAmount = timeLeft/startTime;

			if(timeLeft<=0)
				wrongAnswer(gauge.transform.position);
		}
	}

	private void addScore()
	{
		MainSoundSrc.PlaySound("right");
		var score = levelContoller.GetScore( currentLevel, timeLeft);
		
		gController.mAddCombo();
		gController.mAddScore(score);
	}

	private void decreaseLife()
	{
		StartCoroutine( decreaseLifeCo() );
	}

	private IEnumerator decreaseLifeCo()
	{
		yield return new WaitForSeconds(1f);
		MainSoundSrc.PlaySound("wrong");
		gController.mBrokeCombo();
		gController.mDecreaseLifeGuage();
	}

	public bool AnswerCurrentQuestion(InfoSign.SignType signType, Vector3 pos)
	{
		if(IsCorrect(signType))
		{
			answerLeft--;
			if(answerLeft == 0)
			{
				correctAnswer();
			}
			return true;
		}
		else
		{
			wrongAnswer(pos);
			return false;
		}
	}

	public bool IsCorrect(InfoSign.SignType signType)
	{
		return signType == currentQuestionType;
	}

	private void correctAnswer()
	{
		addScore();
		currentLevel++;
		StartCoroutine(showResultCo());
		MainSoundSrc.PlaySound("Signchange");
	}

	private void wrongAnswer(Vector3 pos)
	{
		var hpNoti = Instantiate(HPNotifyPrefab);
		hpNoti.gameObject.SetActive(true);
		hpNoti.transform.position = pos;
		hpNoti.DecreaseLife(1);
		decreaseLife();
		StartCoroutine(showResultCo());
		MainSoundSrc.PlaySound("Signchange");
	}

	private IEnumerator showResultCo()
	{
		cat.FinishLevel();
		activateAnswerList.ForEach( (a)=> {a.ShowResult();} );

		if(State != QuestionState.Finish)
		{
			State = QuestionState.Result;

			yield return new WaitForSeconds(3f);

			while(State != QuestionState.Result)
				yield return null;

			StartCoroutine(generateQuestion());
		}
	}

	private void closeAllAnswer()
	{
		for(var i=0 ; i<answers.Length ; i++)
		{
			answers[i].Close();
		}
	}

	private void setupActivateAnswer(int totalAnswer)
	{
		activateAnswerList.Clear();
		for(var i=0 ; i<answers.Length ; i++)
		{
			if(i<totalAnswer)
			{
				activateAnswerList.Add( answers[i] );
				answers[i].gameObject.SetActive(true);
			}
			else
			{
				answers[i].gameObject.SetActive(false);
			}
		}
	}

	private IEnumerator generateQuestion()
	{
		State = QuestionState.GenerateQuestion;
		currentQuestionType = (InfoSign.SignType)Random.Range(0,5);

		setupActivateAnswer( getTotalAnswer( currentLevel ) );

		Shuffle( activateAnswerList );

		var correctAnswer = answerLeft = getCorrectAnswer( currentLevel );
		startTime = timeLeft = getTimeToAnswer( currentLevel );

		infoSign.SwitchSign( currentQuestionType );

		cat.ChangeLevel( currentLevel, currentQuestionType );

		foreach(var answer in activateAnswerList)
		{
			if(correctAnswer > 0)
			{
				answer.Setup( currentQuestionType );
				correctAnswer--;
			}
			else
			{
				var randomAnswerType = (InfoSign.SignType)Random.Range(0,5);
				while( randomAnswerType == currentQuestionType )
				{
					randomAnswerType = (InfoSign.SignType)Random.Range(0,5);
				}
				answer.Setup( randomAnswerType );
			}
		}
		MainSoundSrc.PlaySound("Signchange2");

		while(State != QuestionState.GenerateQuestion)
			yield return null;

		State = QuestionState.WaitingForAnswer;
	}

	private int getTotalAnswer(int levelNumber)
	{
		return levelContoller.GetTotalAnwers( levelNumber );
	}
		
	private int getCorrectAnswer(int levelNumber)
	{
		return levelContoller.GetCorrectAnwers( levelNumber );
	}

	private float getTimeToAnswer(int levelNumber)
	{
		return levelContoller.GetTimeToAnswer( levelNumber );
	}

	public void Shuffle(List<Answer> list)  
	{   
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range( 0, n + 1 );  
			Answer value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
