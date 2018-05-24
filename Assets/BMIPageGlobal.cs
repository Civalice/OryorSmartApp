using UnityEngine;
using System.Collections;

public class BMIPageGlobal : MonoBehaviour {
	public GameObject Male;
	public GameObject MalePress;
	public GameObject Female;
	public GameObject FemalePress;

	// Use this for initialization
	void Start () {
		Male.SetActive(false);
		MalePress.SetActive(true);
		Female.SetActive(true);
		FemalePress.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
