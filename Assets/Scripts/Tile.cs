using UnityEngine;
using System;
using System.Collections.Generic;

public enum HeightType
{
	DeepWater = 1,
	ShallowWater = 2,
	Shore = 3,
	Sand = 4,
	Grass = 5,
	Forest = 6,
	Rock = 7,
	Snow = 8,
	River = 9
}

public enum HeatType
{
	Coldest = 0,
	Colder = 1,
	Cold = 2,
	Warm = 3,
	Warmer = 4,
	Warmest = 5
}

public enum MoistureType
{
	Wettest = 5,
	Wetter = 4,
	Wet = 3,
	Dry = 2,
	Dryer = 1,
	Dryest = 0
}


public class Tile
{
	public HeightType HeightType;
	public HeatType HeatType;
	public MoistureType MoistureType;

	public float HeightValue { get; set; }
	public float HeatValue { get; set; }
	public float MoistureValue { get; set; }
	public int X, Y;
	public int Bitmask;

	public Tile Left;
	public Tile Right;
	public Tile Top;
	public Tile Bottom;

	public bool Collidable;
	public bool FloodFilled;

	public Color Color = Color.black;

	public List<River> Rivers = new List<River>();

	public int RiverSize { get; set; }

	public Tile()
	{
	}

	public void UpdateBitmask()
	{
		int count = 0;

		if (Collidable && Top.HeightType == HeightType)
			count += 1;
		if (Collidable && Right.HeightType == HeightType)
			count += 2;
		if (Collidable && Bottom.HeightType == HeightType)
			count += 4;
		if (Collidable && Left.HeightType == HeightType)
			count += 8;

		Bitmask = count;
	}


	public int GetRiverNeighborCount(River river)
	{
		int count = 0;
		if (Left.Rivers.Count > 0 && Left.Rivers.Contains(river))
			count++;
		if (Right.Rivers.Count > 0 && Right.Rivers.Contains(river))
			count++;
		if (Top.Rivers.Count > 0 && Top.Rivers.Contains(river))
			count++;
		if (Bottom.Rivers.Count > 0 && Bottom.Rivers.Contains(river))
			count++;
		return count;
	}

	public Direction GetLowestNeighbor()
	{
		if (Left.HeightValue < Right.HeightValue && Left.HeightValue < Top.HeightValue && Left.HeightValue < Bottom.HeightValue)
			return Direction.Left;
		else if (Right.HeightValue < Left.HeightValue && Right.HeightValue < Top.HeightValue && Right.HeightValue < Bottom.HeightValue)
			return Direction.Right;
		else if (Top.HeightValue < Left.HeightValue && Top.HeightValue < Right.HeightValue && Top.HeightValue < Bottom.HeightValue)
			return Direction.Right;
		else if (Bottom.HeightValue < Left.HeightValue && Bottom.HeightValue < Top.HeightValue && Bottom.HeightValue < Right.HeightValue)
			return Direction.Right;
		else
			return Direction.Bottom;
	}

	public void SetRiverPath(River river)
	{
		if (!Collidable)
			return;

		if (!Rivers.Contains(river))
		{
			Rivers.Add(river);
		}
	}

	private void SetRiverTile(River river)
	{
		SetRiverPath(river);
		HeightType = HeightType.River;
		HeightValue = 0;
		Collidable = false;
	}

	public void DigRiver(River river, int size)
	{
		SetRiverTile(river);
		RiverSize = size;

		if (size == 1)
		{
			Bottom.SetRiverTile(river);
			Right.SetRiverTile(river);
			Bottom.Right.SetRiverTile(river);
		}

		if (size == 2)
		{
			Bottom.SetRiverTile(river);
			Right.SetRiverTile(river);
			Bottom.Right.SetRiverTile(river);
			Top.SetRiverTile(river);
			Top.Left.SetRiverTile(river);
			Top.Right.SetRiverTile(river);
			Left.SetRiverTile(river);
			Left.Bottom.SetRiverTile(river);
		}

		if (size == 3)
		{
			Bottom.SetRiverTile(river);
			Right.SetRiverTile(river);
			Bottom.Right.SetRiverTile(river);
			Top.SetRiverTile(river);
			Top.Left.SetRiverTile(river);
			Top.Right.SetRiverTile(river);
			Left.SetRiverTile(river);
			Left.Bottom.SetRiverTile(river);
			Right.Right.SetRiverTile(river);
			Right.Right.Bottom.SetRiverTile(river);
			Bottom.Bottom.SetRiverTile(river);
			Bottom.Bottom.Right.SetRiverTile(river);
		}

		if (size == 4)
		{
			Bottom.SetRiverTile(river);
			Right.SetRiverTile(river);
			Bottom.Right.SetRiverTile(river);
			Top.SetRiverTile(river);
			Top.Right.SetRiverTile(river);
			Left.SetRiverTile(river);
			Left.Bottom.SetRiverTile(river);
			Right.Right.SetRiverTile(river);
			Right.Right.Bottom.SetRiverTile(river);
			Bottom.Bottom.SetRiverTile(river);
			Bottom.Bottom.Right.SetRiverTile(river);
			Left.Bottom.Bottom.SetRiverTile(river);
			Left.Left.Bottom.SetRiverTile(river);
			Left.Left.SetRiverTile(river);
			Left.Left.Top.SetRiverTile(river);
			Left.Top.SetRiverTile(river);
			Left.Top.Top.SetRiverTile(river);
			Top.Top.SetRiverTile(river);
			Top.Top.Right.SetRiverTile(river);
			Top.Right.Right.SetRiverTile(river);
		}
	}

}
