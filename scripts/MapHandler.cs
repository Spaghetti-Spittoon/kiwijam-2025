using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class MapHandler : Node
{
	[Export]
	public TileMapLayer TileMap;

	public const int GridSize = 100;
	public const int HalfGridSize = GridSize / 2;

	public static MapHandler Instance;
	public List<TileDefinition> TileDefinitions = new List<TileDefinition>();

	public static int PixelToTile(float pos) => Mathf.FloorToInt(pos / GridSize);
	public static int PixelInTile(float pos) => Mathf.FloorToInt(pos % GridSize);

	public override void _Ready()
	{
		Instance = this;
		TileDefinitions.Add(new TileDefinition(true, true, true, true, false, false));
		TileDefinitions.Add(new TileDefinition(false, false, true, true, true, true));
		TileDefinitions.Add(new TileDefinition(true, false, false, false, false, true));
		TileDefinitions.Add(new TileDefinition(true, true, true, false, false, false));
		TileDefinitions.Add(new TileDefinition(true, true, true, true, true, true));
		TileDefinitions.Add(new TileDefinition(true, true, true, false, true, true));
		TileDefinitions.Add(new TileDefinition(true, true, false, true, true, true));
		TileDefinitions.Add(new TileDefinition(false, false, true, true, false, false));
		TileDefinitions.Add(new TileDefinition(false, false, false, true, true, true));
		TileDefinitions.Add(new TileDefinition(false, true, false, false, true, false));
	}

	public Array<Vector2I> GetDirectionsAtNodeCenter(Vector2 position)
	{
		return GetDirectionsAtNodeCenter(PixelToTile(position.X), PixelToTile(position.Y));
	}

	public Array<Vector2I> GetDirectionsAtNodeCenter(int x, int y)
	{
		var atlasPos = TileMap.GetCellAtlasCoords(new Vector2I(x, y));
		var id = atlasPos.X + (atlasPos.Y * 5);

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
			return TileDefinitions[id].Directions;
		}

		return new Array<Vector2I>();
	}

	public static bool HasPassedTileCenter(Vector2 oldPosition, Vector2 newPosition, Vector2 direction)
	{
		if (PixelToTile(oldPosition.X) != PixelToTile(newPosition.X))
		{
			//Different tiles
			return false;
		}

		if (PixelToTile(oldPosition.Y) != PixelToTile(newPosition.Y))
		{
			//Different tiles
			return false;
		}

		if (direction.X > 0)
		{
			//Moving right
			if (PixelInTile(oldPosition.X) < HalfGridSize && PixelInTile(newPosition.X) >= HalfGridSize)
			{
				return true;
			}
		}
		else if (direction.X < 0)
		{
			//Moving left
			if (PixelInTile(oldPosition.X) > HalfGridSize && PixelInTile(newPosition.X) <= HalfGridSize)
			{
				return true;
			}
		}

		if (direction.Y > 0)
		{
			//Moving down
			if (PixelInTile(oldPosition.Y) < HalfGridSize && PixelInTile(newPosition.Y) >= HalfGridSize)
			{
				return true;
			}
		}
		else if (direction.Y < 0)
		{
			//Moving up
			if (PixelInTile(oldPosition.Y) > HalfGridSize && PixelInTile(newPosition.Y) <= HalfGridSize)
			{
				return true;
			}
		}

		return false;
	}
}

public class TileDefinition
{
	public Array<Vector2I> Directions = new Array<Vector2I>();

	public TileDefinition(bool topRight, bool right, bool bottomRight, bool bottomLeft, bool left, bool topLeft)
	{
		if (topRight)
		{
			Directions.Add(Vector2I.Right + Vector2I.Up);
		}
		if (right)
		{
			Directions.Add(Vector2I.Right);
		}
		if (bottomRight)
		{
			Directions.Add(Vector2I.Right + Vector2I.Down);
		}
		if (topLeft)
		{
			Directions.Add(Vector2I.Left + Vector2I.Up);
		}
		if (left)
		{
			Directions.Add(Vector2I.Left);
		}
		if (bottomLeft)
		{
			Directions.Add(Vector2I.Left + Vector2I.Down);
		}
	}
}