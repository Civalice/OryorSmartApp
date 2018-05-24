using UnityEngine;
using System.Collections;

public class ContentButtonObject : StateButtonObject<DetailState> {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PageDetailGlobal.state != WorkingState)
			return;
		base.Update();
	}
}
