using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Game1
{

	public class Catcher : MonoBehaviour {
		public static List<Catcher> CatcherList = new List<Catcher>();

		private Hole targetHole = null;
		private Vector2 targetPosition;
		public float speed;

		public float idleTime;

		public SnailFace face;
		public ShellObject shell;

		private bool idle = false;
		private bool Moving = false;
		
		private Hole LastestHole = null;
		public GameObject HPNotify;
		public static void CreateCatcher (int number)
		{
			GameObject prefab = GlobalGuage.GetCatcherPrefab();
			for (int i = 0; i < number; i++) {
				GameObject snail = (GameObject) GameObject.Instantiate(prefab);
				Catcher ca = snail.GetComponent<Catcher>();
				ca.setLevel();
				//set random position
				CatcherList.Add(ca);
			}
		}

		public static void ClearCatcher ()
		{
			foreach (Catcher snail in CatcherList) {
				if (snail.LastestHole != null)
					MoleSpawner.ReleaseHole (snail.LastestHole);

				DestroyObject(snail.gameObject);
			}
			CatcherList.Clear ();
		}

		public static void BoringCatcher()
		{
				Game1_Leveling.PlayScoreSound(false);
			foreach (Catcher snail in CatcherList) {
				snail.face.BoredSnail();
					}
		}
		// Use this for initialization
		void Start () {
			setLevel ();
				StartCoroutine("FindHole");
		}

		public void setLevel()
		{
			SnailLeveling lvl = Game1_Leveling.GetLvling ();
			speed = lvl.CatcherSpeed/100.0f;
			idleTime = lvl.CatcherIdleTime;
			shell.ChangeShell (Game1_Leveling.GetLvling ().Shell);
		}

		void catchHole()
		{
			if (targetHole != null) {
				if (targetHole.CatchHole ())
				{
					if (targetHole.type == MoleType.MOLE_RIGHT)
					{
						//Plus Score
						face.HappySnail();
						Game1_Leveling.PlayScoreSound(true);

						//Gain EXP
						Game1_Leveling.AddEXP();
						setLevel();
						GlobalGuage.AddScore(Game1_Leveling.GetCatchMoleScore());
					}
					else if (targetHole.type == MoleType.MOLE_WRONG)
					{
						//decrease life
						face.ShockSnail();
							Game1_Leveling.PlayScoreSound(false);
						GameObject hpObj = GameObject.Instantiate(HPNotify) as GameObject;
						hpObj.SetActive(true);
						hpObj.transform.position = this.transform.position;
						hpObj.GetComponent<HPNotification>().DecreaseLife(1);
						GlobalGuage.DecreaseLife();
						GlobalGuage.BrokeCombo ();
					}
					else
					{
						//Game Over
						GameObject hpObj = GameObject.Instantiate(HPNotify) as GameObject;
						hpObj.transform.position = this.transform.position;
						hpObj.GetComponent<HPNotification>().DecreaseLife(2);
						GlobalGuage.DangerMole();
					}
				}
				targetHole = null;
			}
			StartCoroutine("FindHole");
		}

		IEnumerator FindHole()
		{
			while(targetHole == null)
			{
				float minRange = 0;
				bool IsFirst = false;
				if (MoleSpawner.HoleList.Count <= 0) {
					yield return null;
				}
				foreach (Hole hole in MoleSpawner.HoleList) {
					if (!IsFirst && hole.IsPop && !hole.IsLock)
					{
						IsFirst = true;
						minRange = Vector3.Distance(this.transform.localPosition,hole.transform.localPosition);
						targetHole = hole;
					}
					else{
						if (hole != null)
						{
							float range = Vector3.Distance(this.transform.localPosition, hole.transform.localPosition);
							if (hole.IsPop&&!hole.IsLock&&( minRange > range))
							{
								minRange = range;
								targetHole = hole;
							}
						}
					}
				}
					yield return null;
			}
			targetHole.LockHole ();
			StartCoroutine("Idle");
		}

		class MyWait : YieldInstruction
		{
			public MyWait(float seconds)
			{
			}
		}

		IEnumerator Wait(float seconds)
		{
			float timer = 0;
			while (seconds > timer)
			{
				if (!GlobalGuage.GlobalPause)
				{
					timer+=Time.deltaTime;
				}
				yield return 0;
			}
		}

		IEnumerator Idle()
		{
			idle = true;
			Animator anim = GetComponent<Animator> ();
			anim.Play ("SnailIdle");
			yield return StartCoroutine( Wait (idleTime));
			if(targetHole.IsPop)
				{
					idle = false;
					if (LastestHole != null)
						MoleSpawner.ReleaseHole (LastestHole);
					LastestHole = targetHole;
					StartCoroutine("Moveto",targetHole);
				}
				else
				{
					targetHole = null;
					StartCoroutine("FindHole");
				}
		}

		IEnumerator Moveto(Hole target)
		{
			Moving = true;
				target.WalkToHole();
			Animator anim = GetComponent<Animator> ();
			anim.Play ("SnailWalk");
			targetPosition = targetHole.transform.localPosition;
			if (this.transform.localPosition.x - targetPosition.x < 0)
				this.transform.localScale = new Vector3 (-1, 1, 1);
			else
				this.transform.localScale = new Vector3 (1, 1, 1);

			while (Vector3.Distance(this.transform.localPosition, targetPosition) > 0.05f) {
				if (!GlobalGuage.GlobalPause)
					this.transform.localPosition = Vector3.MoveTowards (this.transform.localPosition, targetPosition, speed);
				yield return null;
			}
			Moving = false;
			catchHole ();
		}

		// Update is called once per frame
		void Update () {
		}
	}
}
