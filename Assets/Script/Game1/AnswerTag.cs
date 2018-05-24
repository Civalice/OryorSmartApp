using UnityEngine;
using System.Collections;
namespace Game1
{
	public class AnswerTag : MonoBehaviour {
		public Sprite[] RightTagList;
		public Sprite[] WrongTagList;
		public Sprite DangerTag;

		private SpriteRenderer spRenderer;
		// Use this for initialization
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void SetTagIdx(MoleType type)
		{
			spRenderer = GetComponent<SpriteRenderer> ();
			if (type == MoleType.MOLE_RIGHT) {
				//right Tag
				spRenderer.sprite = RightTagList[Random.Range(0,RightTagList.Length)];
			} 
			else if (type == MoleType.MOLE_WRONG)
			{
				spRenderer.sprite = WrongTagList[Random.Range(0,WrongTagList.Length)];
			}
			else
			{
				spRenderer.sprite = DangerTag;
			}
		}
	}
}