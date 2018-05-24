using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Answer : MonoBehaviour {

	private InfoSign.SignType signType;

	[SerializeField]
	private TMPro.TextMeshPro text;
	[SerializeField]
	private GameObject oryorLogo;
	[SerializeField]
	private GameObject correct;
	[SerializeField]
	private GameObject wrong;

	private GameObject signFront, signBack;
	private Color nextColor = Color.white;

	private bool isDown = false, isActive = false;
	// Use this for initialization
	void Start () {
		text.GetComponent<MeshRenderer>().sortingOrder = 8;
		signFront = transform.FindChild("sign_front").gameObject;
		signBack = transform.FindChild("sign_back").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		bool TouchDown = TouchInterface.GetTouchDown ();
		bool TouchUp = TouchInterface.GetTouchUp ();
		Vector2 pos = TouchInterface.GetTouchPosition ();
		var ishit = GetComponent<Collider2D>().OverlapPoint (pos);
		if (TouchDown && ishit) {
			isDown = true;
		} 
		else if (TouchUp)
		{
			if( isDown && ishit )
			{
				onClick();
			}
			isDown = false;
		}
	}

	public void ShowResult()
	{
		StartCoroutine( showResultCo() );
	}

	private IEnumerator showResultCo()
	{
		if( isActive )
		{
			setupResult( QuestionController.GetInstance().IsCorrect( signType ) );
			isActive = false;
		}

		yield return new WaitForSeconds(2f);

		correct.SetActive(false);
		wrong.SetActive(false);
		GetComponent<Animator>().SetBool("Open", false);
	}

	public void Setup(InfoSign.SignType signType)
	{
		this.signType = signType;

		SetupText( signType, oryorLogo, text );

		isActive = true;
		GetComponent<Animator>().SetBool("Open", true);
	}

	public static void SetupText(InfoSign.SignType signType, GameObject oryorLogo, TMPro.TextMeshPro text)
	{
		if(signType == InfoSign.SignType.Drug || signType == InfoSign.SignType.Cosmatic)
		{
			oryorLogo.SetActive(false);
			text.fontSize = 4.5f;
		}
		else if(signType == InfoSign.SignType.Food)
		{
			oryorLogo.SetActive(true);
			text.fontSize = 2.5f;
		}
		else
		{
			oryorLogo.SetActive(true);
			text.fontSize = 3.5f;
		}
		
		text.text = generateAnswerCode(signType);
	}

	private Color getRandomColor()
	{
		var ran = UnityEngine.Random.Range(0, 6);
		if(ran == 0)
			return Color.cyan;
		else if(ran == 1)
			return Color.green;
		else if(ran == 2)
			return Color.magenta;
		else if(ran == 3)
			return Color.yellow;
		else if(ran == 4)
			return Color.blue;
		else if(ran == 5)
			return Color.grey;
		else 
			return Color.white;
	}

	public void Close()
	{
		if( !isActive )return;
		GetComponent<Animator>().SetBool("Open", false);
		correct.SetActive(false);
		wrong.SetActive(false);
	}

	protected void onClick()
	{
		if( !isActive )return;
		var qc = QuestionController.GetInstance();
		if(qc.State == QuestionController.QuestionState.WaitingForAnswer)
			setupResult(qc.AnswerCurrentQuestion(signType, transform.position));
	}

	private void setupResult(bool isCorrect)
	{
		if(isCorrect)
		{
			correct.SetActive(true);
			var playresult = correct.GetComponent<Animation>().Play();
			Debug.Log("Play Result "+playresult);
		}
		else
		{
			wrong.SetActive(true);
			wrong.GetComponent<Animation>().Play();
		}
		isActive = false;
	}

	public static string generateAnswerCode( InfoSign.SignType signType)
	{
		if( signType == InfoSign.SignType.Food )
		{
			var arr = new string[5];
			arr[0] = getNum(10, 87) ;
			arr[1] = getNum(1, 4) ;
			arr[2] = getNum(digits.three)+getNum(50, 58);
			arr[3] = getNum(1, 2) ;
			arr[4] = getNum(digits.four) ;

			return string.Join("-", arr);
		}
		else if( signType == InfoSign.SignType.Drug )
		{
			var midText = "A<space=0.5>";
			var ranTextNum = getNum (1,3);
			if(ranTextNum == "2")midText = "B<space=0.5>";
			else if(ranTextNum == "3") midText = "C<space=0.5>";
			return getNum(1, 2)+midText+getNum(0, 999)+"/"+getNum(50, 58);
		}
		else if( signType == InfoSign.SignType.Cosmatic )
		{
			var arr = new string[4];
			arr[0] = getNum(10, 87) ;
			arr[1] = getNum(1, 3) ;
			arr[2] = getNum(50, 58) ;
			arr[3] = getNum(digits.five) ;
			
			return string.Join("-", arr);
		}
		else if( signType == InfoSign.SignType.Medical )
		{
			var initial = "ผ<space=0.5>";
			var ranText = getNum(1,2);
			if(ranText == "2") initial = "น<space=0.5>";
			return initial+" "+getNum(1, 999)+" / "+getNum(2550, 2558);
		}
		else if( signType == InfoSign.SignType.Dangerous )
		{
			return "วอส<space=0.5>"+" "+getNum(1, 999)+" / "+getNum(2550, 2558);
		}
		return string.Empty;
	}

	private static string getNum(int start, int end)
	{
		return ((int)UnityEngine.Random.Range(start, end+0.9f)).ToString();
	}

	private static string getNum(digits digits)
	{
		var rand = UnityEngine.Random.Range(1,999999);
		var result =  rand % (int)digits;

		return result.ToString(getFormat(digits));
	}

	private enum digits
	{
		one = 10,
		two = 100,
		three = 1000,
		four = 10000,
		five = 100000,
	}

	private static string getFormat(digits digits)
	{
		if(digits == digits.one)return "D1";
		if(digits == digits.two)return "D2";
		if(digits == digits.three)return "D3";
		if(digits == digits.four)return "D4";
		if(digits == digits.five)return "D5";
		return "";
	}
}
