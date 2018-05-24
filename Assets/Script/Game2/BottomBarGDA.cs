using UnityEngine;
using System.Collections;

public class BottomBarGDA : MonoBehaviour {
	public GameObject EnergyTag;
	public GameObject SugarTag;
	public GameObject FatTag;
	public GameObject SodiumTag;

	public int PigFlag = 0;

	public void ClearFlag()
	{
		PigFlag = 0;
		UpdateFlag();
	}

	public bool EatFlag(int flag)
	{
		if ((PigFlag & flag) == 0) {
				PigFlag |= flag;
				UpdateFlag();
				return true;
		} else {
				PigFlag = 0;
				UpdateFlag();
				return false;
		}
	}
	public bool FullFlagUpdate()
	{
		if ((PigFlag^0x1E)==0) {
			PigFlag = 0;
			UpdateFlag();
			return true;
		} 
		return false;
	}
	void UpdateFlag()
	{
				if ((PigFlag & (1 << 4)) != 0) {
						EnergyTag.SetActive (true);
				} else {
						EnergyTag.SetActive (false);
				}
				if ((PigFlag & (1 << 3)) != 0) {
						SugarTag.SetActive (true);
				} else {
						SugarTag.SetActive (false);
				}
				if ((PigFlag & (1 << 2)) != 0) {
						FatTag.SetActive (true);
				} else {
						FatTag.SetActive (false);
				}
				if ((PigFlag & (1 << 1)) != 0) {
						SodiumTag.SetActive (true);
				} else {
						SodiumTag.SetActive (false);
				}
		}

	// Use this for initialization
	void Start () {
		UpdateFlag ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
