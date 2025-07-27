using Godot;
using Godot.Collections;
using System.Collections.Generic;


public partial class EnemyHandler : Area2D
{
	MapHandler map;

	//Temporarily using constants
	public const int GridSize = 100;
	public const int HalfGridSize = 50;
	public const int MoveSpeed = 100;
	public const int MaxTileX = 5;
	public const int MaxTileY = 5;

	public int XTile => map.PixelToTile(Position.X);
	public int YTile => map.PixelToTile(Position.Y);
	public Vector2I Direction = Vector2I.Right;

	public override void _Ready()
	{
		var parent = GetParent();
		var layer = parent.GetNode<TileMapLayer>("TileMapLayer");
		map = new MapHandler(layer);
		PickDirection();
	}

	public override void _Process(double delta)
	{
		var oldPosition = Position;
		Position += (Vector2)Direction * (float)delta * MoveSpeed;

		if (map.GetTileInfo(Position).TileType == TileTypes.NoneGiven)
		{
			// Turn around when we enter a non-navigation tile
			Direction = -Direction;
			Position += (Vector2)Direction * 0.01F;
		}
		if (map.HasPassedFullTile(oldPosition, Position))
		{
			// GD.Print("Enemy passed Fulltile");
			PickDirection();
		}
	}

	private void PickDirection()
	{
		var startPos = Position;
		Position = map.SnapToCenterOfTile(Position);
		// GD.Print($"Snapping from {startPos} to {Position}");
		var tile = map.GetTileInfo(Position);

		if (tile.TileType == TileTypes.NoneGiven)
		{
			// If we go off the map, Turn around!
			// GD.Print("Enemy not in the map; turning around!");
			Direction = -Direction;
		}
		else
		{
			var oldDirection = Direction;
			Direction = tile.Directions.PickRandom();
			// GD.Print($"Going from direction {oldDirection} to {Direction}");
		}
		// Also nudge enemy in its new direction to avoid re-triggering crossing the middle of a tile
		Position += (Vector2)Direction * 0.01F;
	}

	private void CenterInTile()
	{
		Position = new Vector2(XTile * GridSize + HalfGridSize, YTile * GridSize + HalfGridSize);
	}

}
