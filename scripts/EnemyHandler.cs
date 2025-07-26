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
		Direction = GetTileDirections().PickRandom();
	}

	private void CenterInTile()
	{
		Position = new Vector2(XTile * GridSize + HalfGridSize, YTile * GridSize + HalfGridSize);
	}

	private Array<Vector2> GetTileDirections()
	{
		var result = new Array<Vector2>();
		var tileSource = Map.GetCellSourceId(new Vector2I(XTile, YTile));

		switch(tileSource)
		{
			case 0:
				if(YTile > 0)
				{
					result.Add(Vector2.Up + Vector2.Right);
				}
				if (XTile > 0)
				{
					result.Add(Vector2.Down + Vector2.Left);
				}
				break;
		}

		return result;
	}
}
