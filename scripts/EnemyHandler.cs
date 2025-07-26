using Godot;
using Godot.Collections;


public partial class EnemyHandler : Area2D
{
	//Temporarily using constants
	public const int GridSize = 100;
	public const int HalfGridSize = 50;
	public const int MoveSpeed = 100;
	public const int MaxTileX = 5;
	public const int MaxTileY = 5;

	public static int PixelToTile(float pos) => Mathf.FloorToInt(pos / GridSize);
	public static int PixelInTile(float pos) => Mathf.FloorToInt(pos % GridSize);

	public int XTile => PixelToTile(Position.X);
	public int YTile => PixelToTile(Position.Y);
	public Vector2 Direction = Vector2.Right;

	public override void _Ready()
	{
		CenterInTile();
	}

	public override void _Process(double delta)
	{
		var oldPosition = Position;
		Position += Direction * (float)delta * MoveSpeed;

		if (HasPassedTileCenter(oldPosition, Position, Direction))
		{
			CenterInTile();
			Direction = PossibleDirections().PickRandom();
		}

	}

	private bool HasPassedTileCenter(Vector2 oldPosition, Vector2 newPosition, Vector2 direction)
	{
		if (PixelToTile(oldPosition.X) != PixelToTile(Position.X))
		{
			//Different tiles
			return false;
		}

		if (PixelToTile(oldPosition.Y) != PixelToTile(Position.Y))
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
		else if (Direction.X < 0)
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
		else if (Direction.Y < 0)
		{
			//Moving up
			if (PixelInTile(oldPosition.Y) > HalfGridSize && PixelInTile(newPosition.Y) <= HalfGridSize)
			{
				return true;
			}
		}

		return false;
	}

	private void CenterInTile()
	{
		Position = new Vector2(XTile * GridSize + HalfGridSize, YTile * GridSize + HalfGridSize);
	}

	public Array<Vector2> PossibleDirections()
	{
		var list = new Array<Vector2>();

		//Possible to move left
		if (XTile > 0)
		{
			list.Add(Vector2.Left);
			if (YTile > 0)
			{
				list.Add(Vector2.Up + Vector2.Left);
			}
			if (YTile < MaxTileY)
			{
				list.Add(Vector2.Down + Vector2.Left);
			}
		}

		if (XTile < MaxTileX)
		{
			list.Add(Vector2.Right);
			if (YTile > 0)
			{
				list.Add(Vector2.Up + Vector2.Right);
			}
			if (YTile < MaxTileY)
			{
				list.Add(Vector2.Down + Vector2.Right);
			}
		}

		return list;
	}


}
