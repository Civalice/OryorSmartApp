using UnityEngine;

public class BadGuy : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshPro lifeText;

	private int life = 5;

	public void SetLife(int life)
	{
		this.life = life;
		lifeText.text = life.ToString();
	}

	public int DecreaseLife()
	{
		life--;
		SetLife( life );
		return life;
	}

	public int Life()
	{
		return life;
	}

	public virtual void MoveToTarget(Vector3 target)
	{

	}

	public void die()
	{
		setActivateLifeText(false);
		GetComponent<Animator>().SetTrigger("Capture");
	}

	protected void setActivateLifeText(bool active)
	{
		lifeText.gameObject.SetActive( active );
		transform.FindChild("owner_time").gameObject.SetActive(active);
	}
}

