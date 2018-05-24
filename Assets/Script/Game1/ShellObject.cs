using UnityEngine;
using System.Collections;
namespace Game1
{
	public class ShellObject : MonoBehaviour {

		public void ChangeShell(ShellType shell)
		{
			SpriteRenderer spr = GetComponent<SpriteRenderer> ();
			Sprite[] shellSpriteList = Game1_Leveling.GetShellList();
			switch (shell) {
			case ShellType.SHELL_CAN:
			{
				spr.sprite = shellSpriteList[0];
			}break;
			case ShellType.SHELL_NORMAL:
			{
				spr.sprite = shellSpriteList[1];
			}break;
			case ShellType.SHELL_TEMPLE:
			{
				spr.sprite = shellSpriteList[2];
			}break;
			case ShellType.SHELL_SPIKE:
			{
				spr.sprite = shellSpriteList[3];
			}break;
			case ShellType.SHELL_WING:
			{
				spr.sprite = shellSpriteList[4];
			}break;
			}
		}

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
