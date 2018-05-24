using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour {
	public int sizeX = 5, sizeY = 6;

	public static int TotalSize = 0;

	private static List<Tile> tileList = new List<Tile>();

	// Use this for initialization
	void Start () {
		TotalSize = sizeX * sizeY;

		var array = new Tile[sizeX, sizeY];
		int x = 0;
		int y = 0;
		for(var i = 0; i < this.transform.childCount-1 ; i++)
		{
			var tile = transform.GetChild(i).GetComponent<Tile>();
			array[x,y] = tile;
			if(x < sizeX-1) 
				x++;
			else
			{
				x = 0;
				y++;
			}
			tileList.Add( tile );
		}

		for(x = 0; x < sizeX ; x++)
		{
			for(y = 0; y < sizeY ; y++)
			{
				var top = (y-1 >= 0)? array[x, y-1]:null;
				var left = (x-1 >= 0)? array[x-1, y]:null;
				var right = (x+1 < sizeX)? array[x+1, y]:null;
				var bottom = (y+1 < sizeY)? array[x, y+1]:null;
				array[x,y].SetupTiles( top, left, right, bottom );
				//Debug.Log("Setup "+x+":"+y);
			}
		}

	}

	public static List<Tile> GetTileList()
	{
		return tileList;
	}


}
