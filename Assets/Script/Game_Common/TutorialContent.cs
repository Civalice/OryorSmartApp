using UnityEngine;
using System.Collections;

public class TutorialContent : MonoBehaviour {
	public GamePlayButton NextButton;
	public GamePlayButton PrevButton;
	public GamePlayButton CloseButton;

	public SpriteRenderer TutorialPage;

	public Sprite[] TutorialList;

	int currentPage = 1;
	void Awake() 
	{
		NextButton.OnReleased += NextPage;
		PrevButton.OnReleased += PrevPage;
		CloseButton.OnReleased += ClosePage;
		gameObject.SetActive(false);
	}

	public void OpenTutorial()
	{
		currentPage = 1;
		TutorialPage.sprite = TutorialList[0];
		gameObject.SetActive(true);
		GlobalController.setState(GameState.GS_TUTORIAL);
	}

	void NextPage()
	{
		currentPage++;
		if (currentPage > TutorialList.Length)
			ClosePage();
		else
		{
			TutorialPage.sprite = TutorialList[currentPage-1];
		}
	}

	void PrevPage()
	{
		currentPage--;
		if (currentPage < 1)
			ClosePage();
		else
		{
			TutorialPage.sprite = TutorialList[currentPage-1];
		}
	}

	void ClosePage()
	{
		gameObject.SetActive(false);
		GlobalController.setState(GameState.GS_STARTMENU);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
