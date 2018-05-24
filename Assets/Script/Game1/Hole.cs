using UnityEngine;
using System.Collections;
namespace Game1
{
	public enum MoleType
	{
		MOLE_RIGHT = 0,
		MOLE_WRONG,
		MOLE_DANGER,
	}
	public class Hole : MonoBehaviour {
		public AnswerTag MoleTag;
		public Collider2D MoleCollider;
		public bool IsPop = false;
		public bool IsLock = false;
		private Animator MoleAnimator;
		private float waitTime = 3.0f;
		public GameObject LockOn;

		public int idx;
//		public bool IsGoodMole;
		public MoleType type = MoleType.MOLE_RIGHT;
		public GameObject HPNortify;

		public AudioClip[] hitSound;
		public bool IsDestroy = false;
		// Use this for initialization
		void Start () {
			LockOn.SetActive (false);
		}

		public void Clear()
		{
			StopCoroutine("Wait");
			StopCoroutine("PopMole");
			StopCoroutine("Countdown");
			StopCoroutine("DestroyCatchHole");
			StopCoroutine("DestroyCountHole");
			StopCoroutine("DestroyHole");
		}

		public void startHole()
		{
					MoleAnimator = GetComponent<Animator> ();
			Debug.Log("HoleStart");
					MoleAnimator.Play ("HoleStart");
					//random Tag
					SnailLeveling lvl = Game1_Leveling.GetLvling ();
					float randomTag = Random.Range (0.0f, lvl.RightMoleRate + lvl.WrongMoleRate + lvl.DangerMoleRate);
					if (randomTag < lvl.RightMoleRate) {
							type = MoleType.MOLE_RIGHT;
							MoleTag.SetTagIdx (type);
					} else if (randomTag > lvl.RightMoleRate + lvl.WrongMoleRate) {
							//IsDangerMole
				type = MoleType.MOLE_DANGER;
				MoleTag.SetTagIdx (type);

					} else {
							//wrong mole
				type = MoleType.MOLE_WRONG;
				MoleTag.SetTagIdx (type);
					}
					StartCoroutine ("PopMole");
			}

		public bool CatchHole()
		{
			if (IsPop) {
				StartCoroutine ("DestroyCatchHole");
				return true;
			} else {
				return false;
			}
		}
		public void WalkToHole()
		{
			IsLock = true;
		}
		public void LockHole()
		{
			LockOn.SetActive (true);
			StopCoroutine ("Countdown");
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

		IEnumerator PopMole()
		{
			MoleAnimator = GetComponent<Animator> ();
			waitTime = Random.Range (Game1_Leveling.GetLvling().MinWaitTime, Game1_Leveling.GetLvling().MaxWaitTime);
			yield return StartCoroutine(Wait(waitTime));
			Debug.Log("HolePop");
			MoleAnimator.Play ("HolePop");
			yield return StartCoroutine(Wait(0.1f));
			yield return StartCoroutine(Wait(MoleAnimator.GetCurrentAnimatorStateInfo (0).length-0.1f));
			Debug.Log("HolePop : Get State Info");
			IsPop = true;
			if (type != MoleType.MOLE_RIGHT)
				StartCoroutine ("Countdown");
		}

		IEnumerator Countdown()
		{
			yield return StartCoroutine(Wait(waitTime));
			IsPop = false;
			StartCoroutine ("DestroyCountHole");
		}

		IEnumerator DestroyCatchHole()
		{
			//calculate score here
			LockOn.SetActive (false);
			IsPop = false;
			MoleAnimator = GetComponent<Animator> ();
			MoleAnimator.Play ("HoleDepop");
			while ((MoleAnimator != null) && MoleAnimator.GetCurrentAnimatorStateInfo(0).IsName("HolePop")) 
			{
				yield return null;
			}
			if (MoleAnimator==null) yield break;
			yield return StartCoroutine(Wait(MoleAnimator.GetCurrentAnimatorStateInfo (0).length));

			MoleSpawner.DestroyHole (this);
		}
		IEnumerator DestroyCountHole()
		{
			//calculate score here
			IsPop = false;
			GlobalGuage.BrokeCombo ();
			MoleAnimator = GetComponent<Animator> ();
			MoleAnimator.Play ("HoleDepop");
			while ((MoleAnimator != null) && MoleAnimator.GetCurrentAnimatorStateInfo(0).IsName("HolePop")) 
			{
				yield return null;
			}
			if (MoleAnimator==null) yield break;
			yield return StartCoroutine(Wait(MoleAnimator.GetCurrentAnimatorStateInfo (0).length));
			MoleSpawner.DestroyHole (this);
			if (!IsLock)
				MoleSpawner.ReleaseHole (this);
		}
		IEnumerator DestroyHole()
		{
			IsPop = false;
			LockOn.SetActive (false);
			MoleAnimator = GetComponent<Animator> ();
			GetComponent<AudioSource>().PlayOneShot(hitSound[Random.Range(0,2)]);
			MoleAnimator.Play ("HoleTouch");
			if (type == MoleType.MOLE_RIGHT) {
				Catcher.BoringCatcher();
				//catcher make bored face here
				GameObject hpObj = GameObject.Instantiate(HPNortify) as GameObject;
				hpObj.SetActive(true);
				hpObj.transform.position = this.transform.position;
				hpObj.GetComponent<HPNotification>().DecreaseLife(1);
				GlobalGuage.DecreaseLife ();
				GlobalGuage.BrokeCombo ();
			} else {
				GlobalGuage.AddScore(Game1_Leveling.GetTouchMoleScore());
				GlobalGuage.AddCombo();
			}
			while ((MoleAnimator != null) && MoleAnimator.GetCurrentAnimatorStateInfo(0).IsName("HolePop")) 
			{
				yield return null;
			}
			if (MoleAnimator==null) yield break;
			yield return StartCoroutine(Wait(MoleAnimator.GetCurrentAnimatorStateInfo (0).length));
			MoleSpawner.DestroyHole (this);
			if (!IsLock)
				MoleSpawner.ReleaseHole (this);
		}
		// Update is called once per frame
		void Update () {
			if (MoleCollider == null)
				return;
			//check collider
			if (!IsPop)
				return;
			bool TouchDown = TouchInterface.GetTouchDown ();
			Vector2 pos = TouchInterface.GetTouchPosition ();
			if (TouchDown) {
				if (IsPop && MoleCollider.OverlapPoint (pos)) {
					StopCoroutine ("Countdown");
					StopCoroutine ("PopMole");
					StartCoroutine ("DestroyHole");
				}
			}
		}
	}
}