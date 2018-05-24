using UnityEngine;
using System.Collections;

public class AwardDBS : MonoBehaviour {
	public static AwardDBS pGlobal;
	public AwardItemData[] allGameData;
	public AwardItemData[] Game1Data;
	public AwardItemData[] Game2Data;
	public AwardItemData[] Game3Data;
	public AwardItemData[] Game4Data;


	void Awake() { 
		pGlobal = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
