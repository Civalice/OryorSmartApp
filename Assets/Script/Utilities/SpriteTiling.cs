using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpriteTiling : MonoBehaviour {
	public bool ScaleTiling = false;
	public Vector2 Tiling = new Vector2(1,1);
	public Vector2 Offset = new Vector2(0,0);
	private Material tilingMaterial = null;
	// Use this for initialization

	void Start () {
		SpriteRenderer spRenderer = GetComponent<SpriteRenderer> ();
		if (tilingMaterial == null) {
			tilingMaterial = new Material(spRenderer.sharedMaterial);
			spRenderer.sharedMaterial = tilingMaterial;
		}
		tilingMaterial.name = "SpriteTiling - "+this.gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		if (ScaleTiling) {
			Tiling.x = this.transform.localScale.x;
			Tiling.y = this.transform.localScale.y;
		}
		if (tilingMaterial != null) {
			//set tiling
			tilingMaterial.SetFloat("_ScaleX",Tiling.x);
			tilingMaterial.SetFloat("_ScaleY",Tiling.y);
			tilingMaterial.SetFloat("_OffsetX",Offset.x);
			tilingMaterial.SetFloat("_OffsetY",Offset.y);
		}
	}
}
