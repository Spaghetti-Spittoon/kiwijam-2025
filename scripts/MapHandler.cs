using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

using AtlasDictionary = System.Collections.Generic.Dictionary<Godot.Vector2I, TileDefinition>;

public class MapHandler
{
	public const int GridSize = 100;
	public const int HalfGridSize = GridSize / 2;
	public const int QuarterGridSize = HalfGridSize / 2;

	const int atlasYOffset = 8;

	AtlasDictionary atlasTiles = new AtlasDictionary();
	TileMapLayer _map;

	public int PixelToTile(float pos) => Mathf.FloorToInt(pos / GridSize);
	public int PixelInTile(float pos) => Mathf.FloorToInt(pos % GridSize);

	public int PixelToHalfTile(float pos) => Mathf.FloorToInt(pos / HalfGridSize);

	public MapHandler(TileMapLayer inputMap)
	{
		//Each TileDefinition matches a tile in the TileMapLayer in order of the atlas.
		atlasTiles.Add(new Vector2I(6, 1), new TileDefinition(TileTypes.SailTopLeft));
		atlasTiles.Add(new Vector2I(5, 1), new TileDefinition(TileTypes.SailTopRight));
		atlasTiles.Add(new Vector2I(1, 1), new TileDefinition(TileTypes.DownEnd));
		atlasTiles.Add(new Vector2I(2, 0), new TileDefinition(TileTypes.LeftEnd));
		atlasTiles.Add(new Vector2I(5, 0), new TileDefinition(TileTypes.RightEnd));
		atlasTiles.Add(new Vector2I(4, 1), new TileDefinition(TileTypes.SailBotLeft));
		atlasTiles.Add(new Vector2I(3, 1), new TileDefinition(TileTypes.SailBotRight));
		atlasTiles.Add(new Vector2I(2, 1), new TileDefinition(TileTypes.UpEnd));
		atlasTiles.Add(new Vector2I(7, 1), new TileDefinition(TileTypes.Straight));
		atlasTiles.Add(new Vector2I(0, 1), new TileDefinition(TileTypes.Middle));

		_map = inputMap;
	}

	public TileDefinition GetTileInfo(Vector2 worldSpacePosition)
	{
		GD.Print($"{nameof(GetTileInfo)}, input: {worldSpacePosition}");
		// Convert world position to local coordinates relative to TileMapLayer
		Vector2 localPos = _map.ToLocal(worldSpacePosition);

		// Convert local position to cell (map) coordinates
		Vector2I cellPos = _map.LocalToMap(localPos);
		Vector2I atlasCoords = _map.GetCellAtlasCoords(cellPos);

		return GetTileAtAtlasCoords(atlasCoords);
	}

	public TileDefinition GetTileAtAtlasCoords(Vector2I atlasCoords)
	{
		GD.Print($"{nameof(GetTileAtAtlasCoords)}, input: {atlasCoords}");

		if (atlasTiles.ContainsKey(atlasCoords))
		{
			var match = atlasTiles[atlasCoords];
			match.AtlasCoordinates = atlasCoords;
			return match;
		}
		GD.Print("no match");
		return new TileDefinition(); //return empty since there is no tile info availablew
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
