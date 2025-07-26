using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MapHandler
{
	public const int GridSize = 100;
	public const int HalfGridSize = GridSize / 2;
	public const int QuarterGridSize = HalfGridSize / 2;

	public List<TileDefinition> TileDefinitions = new List<TileDefinition>();
	TileMapLayer _map;

	public int PixelToTile(float pos) => Mathf.FloorToInt(pos / GridSize);
	public int PixelInTile(float pos) => Mathf.FloorToInt(pos % GridSize);

	public int PixelToHalfTile(float pos) => Mathf.FloorToInt(pos / HalfGridSize);

	public MapHandler(TileMapLayer inputMap)
	{
		//Each TileDefinition matches a tile in the TileMapLayer in order of the atlas.
		TileDefinitions.Add(new TileDefinition(true, false, false, false, false, false));
		TileDefinitions.Add(new TileDefinition(true, false, true, false, false, false));
		TileDefinitions.Add(new TileDefinition(true, true, true, false, false, false));
		TileDefinitions.Add(new TileDefinition(false, false, true, false, false, false));
		TileDefinitions.Add(new TileDefinition(false, false, true, false, false, false));
		TileDefinitions.Add(new TileDefinition(false, false, false, true, true, true));
		TileDefinitions.Add(new TileDefinition(false, false, false, true, false, true));
		TileDefinitions.Add(new TileDefinition(false, false, false, false, false, true));
		TileDefinitions.Add(new TileDefinition(true, true, true, true, true, true));
		TileDefinitions.Add(new TileDefinition(true, false, false, false, false, true));
		TileDefinitions.Add(new TileDefinition(false, false, true, true, false, false));
		TileDefinitions.Add(new TileDefinition(true, false, false, true, true, true));
		TileDefinitions.Add(new TileDefinition(true, true, true, false, false, true));
		TileDefinitions.Add(new TileDefinition(false, false, true, true, true, true));
		TileDefinitions.Add(new TileDefinition(true, true, true, true, false, false));
		TileDefinitions.Add(new TileDefinition(false, true, false, false, true, false));
		_map = inputMap;
	}

	public TileDefinition GetTileInfo(Vector2 pos)
	{
		var result = new TileDefinition();

		var halfX = PixelToHalfTile(pos.X);
		var halfY = PixelToHalfTile(pos.Y);
		var fullX = halfX / 2;
		var fullY = halfY / 2;
		var halfXOffset = halfX % 2;
		var halfYOffset = halfY % 2;

		//Center of the tile
		if (halfXOffset == 1 && halfYOffset == 1)
		{
			return GetTileInfo(fullX, fullY);
		}

		//Left of center
		if (halfXOffset == 0 && halfYOffset == 1)
		{
			GD.Print("left of center");
			var leftTile = GetTileInfo(fullX - 1, fullY);
			var rightTile = GetTileInfo(fullX, fullY);

			if (leftTile.Directions.Contains(Vector2I.Right))
			{
				result.Directions.Add(Vector2I.Left);
			}
			if (rightTile.Directions.Contains(Vector2I.Left))
			{
				result.Directions.Add(Vector2I.Right);
			}
		}

		//Top left
		if (halfXOffset == 0 && halfYOffset == 0)
		{
			//Top left
			if (GetTileInfo(fullX - 1, fullY - 1).Directions.Contains(Vector2I.Down + Vector2I.Right))
			{
				result.Directions.Add(Vector2I.Up + Vector2I.Left);
			}
			//Top Right
			if (GetTileInfo(fullX, fullY - 1).Directions.Contains(Vector2I.Down + Vector2I.Left))
			{
				result.Directions.Add(Vector2I.Up + Vector2I.Right);
			}
			//Bottom left
			if (GetTileInfo(fullX - 1, fullY).Directions.Contains(Vector2I.Up + Vector2I.Right))
			{
				result.Directions.Add(Vector2I.Down + Vector2I.Left);
			}
			//Bottom Right
			if (GetTileInfo(fullX, fullY).Directions.Contains(Vector2I.Up + Vector2I.Left))
			{
				result.Directions.Add(Vector2I.Down + Vector2I.Right);
			}
		}

		return result;
	}

	public TileDefinition GetTileInfo(int x, int y)
	{
		var atlasPos = _map.GetCellAtlasCoords(new Vector2I(x, y));
		var id = atlasPos.X + (atlasPos.Y * 8);

		if (id < 0)
		{
			GD.Print("Not on map " + x + ":" + y);
		}
		else if (id >= TileDefinitions.Count)
		{
			GD.Print("Undefined tile " + id + " at " + x + ":" + y);
		}
		else
		{
			var match = TileDefinitions[id]; //set the Atlas Coordinates
			match.AtlasCoordinates = atlasPos;
			return match;
		}

		return new TileDefinition();
	}

	public bool HasPassedHalfTile(Vector2 oldPos, Vector2 newPos, Vector2I Direction)
	{
		//Check that we weren't previously standing on the boundary
		var oldHalf = SnapToHalfTile(oldPos);
		if(oldHalf.X == oldPos.X && oldHalf.Y == oldPos.Y)
		{
			return false;
		}

		if (Direction.X > 0)
		{
			if (PixelToHalfTile(newPos.X) > PixelToHalfTile(oldPos.X)) return true;
		}
		if (Direction.X < 0)
		{
			if (PixelToHalfTile(newPos.X) < PixelToHalfTile(oldPos.X)) return true;
		}
		if (Direction.Y > 0)
		{
			if (PixelToHalfTile(newPos.Y) > PixelToHalfTile(oldPos.Y)) return true;
		}
		if (Direction.Y < 0)
		{
			if (PixelToHalfTile(newPos.Y) < PixelToHalfTile(oldPos.Y)) return true;
		}
		return false;
	}

	internal Vector2 SnapToHalfTile(Vector2 oldPos)
	{
		var result = new Vector2();
		result.X = PixelToHalfTile(oldPos.X + HalfGridSize / 2) * HalfGridSize;
		result.Y = PixelToHalfTile(oldPos.Y + HalfGridSize / 2) * HalfGridSize;
		return result;
	}

}
