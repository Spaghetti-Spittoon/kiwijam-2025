using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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
		// GD.Print($"{nameof(GetTileInfo)}, input: {worldSpacePosition}");
		// Convert world position to local coordinates relative to TileMapLayer
		Vector2 localPos = _map.ToLocal(worldSpacePosition);

		// Convert local position to cell (map) coordinates
		Vector2I cellPos = _map.LocalToMap(localPos);
		Vector2I atlasCoords = _map.GetCellAtlasCoords(cellPos);

		return GetTileAtAtlasCoords(atlasCoords);
	}

	public TileDefinition GetTileAtCellCoords(Vector2I cellCoords)
	{
		Vector2I atlasCoords = _map.GetCellAtlasCoords(cellCoords);
		return GetTileAtAtlasCoords(atlasCoords);
	}

	public TileDefinition GetTileAtAtlasCoords(Vector2I atlasCoords)
	{
		// GD.Print($"{nameof(GetTileAtAtlasCoords)}, input: {atlasCoords}");

		if (atlasTiles.ContainsKey(atlasCoords))
		{
			var match = atlasTiles[atlasCoords];
			match.AtlasCoordinates = atlasCoords;
			return match;
		}
		// GD.Print($"No known tile at {atlasCoords} (might be background tile)");
		return new TileDefinition(); //return empty since there is no tile info availablew
	}

	public bool IsCentered(Vector2 oldPos, CanvasItem canvas = null)
	{
		const int boundaryOffset = 25;
		//Check that we weren't previously standing on the boundary
		var snapped = SnapToFullTiles(oldPos); //assume this is the center
		var offsetFifty = new Vector2I(snapped.X - 50, snapped.Y - 50);
		var diffX = Math.Abs(offsetFifty.X - oldPos.X);
		var diffY = Math.Abs(offsetFifty.Y - oldPos.Y);
		var crossedCenterX = diffX < boundaryOffset;
		var crossedCenterY = diffY < boundaryOffset;
		var isCentered = crossedCenterX && crossedCenterY;
		var message = $"snapped: {snapped}, offsetFifty: {offsetFifty}, diffX: {diffX}, diffY: {diffY}, crossedCenterX: {crossedCenterX}, crossedCenterY: {crossedCenterY}, isCentered: {isCentered}";
		// GD.Print(message);

		if (canvas != null) //print a string on screen
		{
			Font font = ThemeDB.FallbackFont;
			var position = new Vector2(-400, -200);
			canvas.DrawString(font, position, message, HorizontalAlignment.Left, -1, 24);
		}

		if (crossedCenterX && crossedCenterY)
		{
			return true;
		}
		return false;
	}

	public bool HasPassedFullTile(Vector2 startPos, Vector2 endPos)
	{
		var fullTileCoords = (new float[] { startPos.X, startPos.Y, endPos.X, endPos.Y }).Select(
			coord => (int)Math.Floor((coord + 50.0) / 100.0F)
		).ToArray();

		var x1 = fullTileCoords[0];
		var y1 = fullTileCoords[1];
		var x2 = fullTileCoords[2];
		var y2 = fullTileCoords[3];

		return x1 != x2 || y1 != y2;
	}

	public Vector2I SnapToHalfTile(Godot.Vector2 oldPos)
	{
		var result = new Vector2I();
		result.X = PixelToHalfTile(oldPos.X + HalfGridSize / 2) * HalfGridSize;
		result.Y = PixelToHalfTile(oldPos.Y + HalfGridSize / 2) * HalfGridSize;
		return result;
	}

	public Vector2I SnapToFullTiles(Vector2 oldPos)
	{
		var result = new Vector2I();
		result.X = PixelToTile(oldPos.X + GridSize) * GridSize;
		result.Y = PixelToTile(oldPos.Y + GridSize) * GridSize;
		return result;
	}

	public Vector2 SnapToCenterOfTile(Vector2 position)
	{
		return new Vector2(
			(float)Math.Floor((position.X + QuarterGridSize) / GridSize) * GridSize + HalfGridSize,
			(float)Math.Floor((position.Y + QuarterGridSize) / GridSize) * GridSize + HalfGridSize
		);
	}
}
