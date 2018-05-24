using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Game1
{

public class MoleSpawner : MonoBehaviour {
	public static List<Hole> HoleList = new List<Hole>();

	public static List<int> RandomHole = new List<int>();
	public GameObject HoleStructure;
	public BoxCollider2D boundery;
	public GameObject HoleObject;
	public int maxHole = 2;
		public List<Hole> HoleDebug;
		public List<int> RandomDebug;
		static private bool IsPlay = false;
	// Use this for initialization

	void Awake()
		{
			HoleDebug = HoleList;
			RandomDebug = RandomHole;
		}
	void Start () {
			RandomHole.Clear();
		for (int i = 0; i < HoleStructure.transform.childCount; i++)
			RandomHole.Add (i);
	}

	public static void DestroyHole(Hole hole)
	{
			hole.IsDestroy = true;
		HoleList.Remove (hole);
	}
	
	public static void ReleaseHole(Hole hole)
	{
			if (hole.IsDestroy)
			{
				RandomHole.Add (hole.idx);
				DestroyImmediate(hole.gameObject);
			}
			else
			{
				DestroyHole(hole);
				RandomHole.Add (hole.idx);
				DestroyImmediate(hole.gameObject);
			}
	}
	
	public static void GameStart()
	{
		IsPlay = true;
	}
	public static void GameStop()
	{
		IsPlay = false;
	}
	public static void ClearHole()
	{
		foreach (Hole item in HoleList) {
			if (item != null)
			{
					item.Clear();
					RandomHole.Add(item.idx);
					DestroyImmediate(item.gameObject);
			}
		}
		HoleList.Clear ();
	}

	public void ForceSpawn()
	{
		int idx = GetRandomHoleIndex ();
		if (idx == -1)
						return;
		GameObject hole = (GameObject) GameObject.Instantiate (HoleObject);
		hole.name = "Hole" + idx;
		//random some position
		Vector3 position = HoleStructure.transform.GetChild (idx).localPosition;
		hole.transform.localPosition = new Vector3(position.x + Random.Range(-0.5f,0.5f),
		                                           position.y + Random.Range(-0.5f,0.5f),
		                                           position.z);
		Hole holeComp = hole.GetComponent<Hole> ();
		holeComp.idx = idx;
		holeComp.startHole ();
		HoleList.Add (holeComp);
	}

	int GetRandomHoleIndex()
	{
		if (RandomHole.Count <= 0)
			return -1;
		int holeIdx = RandomHole [Random.Range(0,RandomHole.Count-1)];
		RandomHole.Remove (holeIdx);
		return holeIdx;
	}

	// Update is called once per frame
	void Update () {
		if (!IsPlay) return;
		GameState gState = GlobalGuage.GetGameState ();
		if (gState == GameState.GS_PLAY) {
			if ((RandomHole.Count > 0) && (HoleList.Count < Game1_Leveling.GetLvling().MoleMax)) {
				ForceSpawn ();
			}
		}
	}
}
}