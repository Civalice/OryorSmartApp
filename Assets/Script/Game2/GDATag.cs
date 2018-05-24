using UnityEngine;
using System.Collections;

public class GDATag : MonoBehaviour {
	public GameObject EnergyTag;
	public GameObject SugarTag;
	public GameObject FatTag;
	public GameObject SodiumTag;

	public int GDAFlag;

	public void SetTagFlag(int flag)
	{
		GDAFlag = flag;
		EnergyTag.SetActive (false);
		SugarTag.SetActive (false);
		FatTag.SetActive (false);
		SodiumTag.SetActive (false);
		if ((flag & (1<<4))!=0)
			EnergyTag.SetActive(true);
		if ((flag & (1<<3))!=0)
			SugarTag.SetActive(true);
		if ((flag & (1<<2))!=0)
			FatTag.SetActive(true);
		if ((flag & (1<<1))!=0)
			SodiumTag.SetActive(true);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
