using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class act{
	[XmlAttribute("name")]
	public string actName;
	[XmlAttribute("min")]
	public string calName;
}

[System.Serializable]
public class foodcal{
	[XmlAttribute("id")]
	public string foodcalid;
	[XmlAttribute("name")]
	public string foodName;
	[XmlAttribute("cal")]
	public string foodCal;
	[XmlAttribute("cat1")]
	public string cat1;
	[XmlAttribute("cat2")]
	public string cat2;
	[XmlAttribute("cat3")]
	public string cat3;
	[XmlAttribute("cat4")]
	public string cat4;
	[XmlAttribute("cat5")]
	public string cat5;
	[XmlAttribute("cat6")]
	public string cat6;
	[XmlAttribute("cat7")]
	public string cat7;
	[XmlAttribute("cat8")]
	public string cat8;
	[XmlAttribute("cat9")]
	public string cat9;
	[XmlAttribute("cat10")]
	public string cat10;
}