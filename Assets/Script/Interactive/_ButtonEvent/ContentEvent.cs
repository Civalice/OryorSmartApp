using UnityEngine;
using System.Collections;

public enum ContentType
{
	Null = 0,
	SmartTips_KnowOryor = 1,
	SmartTips_Choose = 2,
	SmartTips_Banner = 3,
	SmartTips_GDA = 4,
	Library_HealthBook = 5,
	Library_TradeInsight = 6,
	Library_Report = 7,
	Media_Animation = 8,
	Media_Movie = 9,
	News = 10,
	Library_FDA = 11,
	Library_FactSheet = 12,
	BMI = 13,
	Diatery = 14,
	Check_Cosmatic = 15,
	Check_Doctor = 16,
	Check_Drug = 17,
	Check_Danger = 18,
	Report = 19,
	News_Law = 20,
	Library_Infographic = 21
};

public class ContentEvent : ChangeSceneEvent {

	public Color pageColor = new Color(1,1,1,1);
	public ContentType type = ContentType.Null;
	public int PageIndex;

	public override void preChangeScene()
	{
		//change color here
		PageDetailGlobal.pageColor = pageColor;
		PageDetailGlobal.type = type;
	}
}
