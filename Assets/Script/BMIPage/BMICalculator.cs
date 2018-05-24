using UnityEngine;
using System.Collections;
using TMPro;

public class BMICalculator : MonoBehaviour {
	public TextMeshPro AgeText;

	public void Calculate()
	{
		string ageText = AgeText.text;
		int age = int.Parse (ageText);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
