using UnityEngine;
using System.Collections;

public class ScoreFloat : MonoBehaviour
{
	public void Setup(int score, Vector3 pos)
	{
		transform.position = new Vector3(pos.x, pos.y, -1);
		GetComponent<TMPro.TextMeshPro>().text = "+"+score.ToString();
		StartCoroutine( moveUp() );
	}

	IEnumerator moveUp()
	{
		var time = 0f;
		while(time< 1f)
		{
			time += Time.deltaTime;
			transform.Translate( Vector3.up * Time.deltaTime );
			yield return null;
		}
		Destroy( gameObject );
	}
}

