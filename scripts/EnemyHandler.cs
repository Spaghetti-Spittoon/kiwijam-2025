using Godot;
using Godot.Collections;
using System.Collections.Generic;


public partial class EnemyHandler : Area2D
{
	//Temporarily using constants
	public const int GridSize = 100;
	public const int HalfGridSize = 50;
	public const int MoveSpeed = 100;
	public const int MaxTileX = 5;
	public const int MaxTileY = 5;

	[Export]
	public TileMapLayer Map;

	public int XTile => MapHandler.PixelToTile(Position.X);
	public int YTile => MapHandler.PixelToTile(Position.Y);
	public Vector2 Direction = Vector2.Right;

	public override void _Ready()
	{
		PickDirection();
	}

	public override void _Process(double delta)
	{
		var oldPosition = Position;
		Position += Direction * (float)delta * MoveSpeed;

		if (MapHandler.HasPassedTileCenter(oldPosition, Position, Direction))
		{
			PickDirection();
		}

	}

	private void PickDirection()
	{
		CenterInTile();
		Direction = MapHandler.Instance.GetDirectionsAtNodeCenter(Position).PickRandom();
	}

	private void CenterInTile()
	{
		Position = new Vector2(XTile * GridSize + HalfGridSize, YTile * GridSize + HalfGridSize);
	}

}
