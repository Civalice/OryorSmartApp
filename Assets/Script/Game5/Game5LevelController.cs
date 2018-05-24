using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Game5LevelController : MonoBehaviour {

	[SerializeField]
	private List<levelData> levelDataList;

	public int GetScore(int levelNumber, float timeLeft)
	{
		var levelData = getLevelData( levelNumber );

		return (int)(levelData.BaseScore + (timeLeft * levelData.BaseScore * levelData.Weight));
	}

	public int GetTimeToAnswer(int levelNumber)
	{
		return getLevelData( levelNumber ).TimeInSecond;
	}

	public int GetCorrectAnwers(int levelNumber)
	{
		return getLevelData( levelNumber ).CorrectAnswers;
	}

	public int GetTotalAnwers(int levelNumber)
	{
		return getLevelData( levelNumber ).TotalAnswers;
	}

	private levelData getLevelData(int levelNumber)
	{
		var levelData = levelDataList.FirstOrDefault(l => levelNumber <= l.LevelEndsAtMoveNumber);
		if(levelData == null)
			levelData = levelDataList.Last(); 
		return levelData;
	}

	[Serializable]
	internal class levelData
	{
		public int TimeInSecond;
		public int BaseScore;
		public float Weight;
		public int TotalAnswers;
		public int CorrectAnswers;
		public int LevelEndsAtMoveNumber;
	}
}

