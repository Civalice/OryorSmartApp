using UnityEngine;
using System.Collections;
[System.Serializable]
public class LinkObject
{
};

public class LinkGlobal : MonoBehaviour {
	public static LinkGlobal sObject;
	public string[] LinkList;

	public static string GetLinkIndex(int idx)
	{
		if (idx < 0)
			return "";
		if (sObject.LinkList.Length < idx)
			return "";
		return sObject.LinkList [idx];
	}

	// Use this for initialization
	void Start () {
		sObject = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
