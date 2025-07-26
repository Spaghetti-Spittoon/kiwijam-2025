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

		if (map.HasPassedHalfTile(oldPosition, Position, Direction))
		{
			PickDirection();
		}

	}

	private void PickDirection()
	{
		Position = map.SnapToHalfTile(Position);
		Direction = map.GetDirections(Position).PickRandom();
	}

	private void CenterInTile()
	{
		Position = new Vector2(XTile * GridSize + HalfGridSize, YTile * GridSize + HalfGridSize);
	}

}
