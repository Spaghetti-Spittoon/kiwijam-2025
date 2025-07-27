
using System.Dynamic;
using System.Linq.Expressions;
using Godot;
using Godot.Collections;

public class TileDefinition
{
	public Vector2I AtlasCoordinates { get; set; }

	public Array<Vector2I> Directions { get; private set; } = new Array<Vector2I>();
	public TileTypes TileType { get; private set; } = TileTypes.NoneGiven;

	public TileDefinition(TileTypes inputType = TileTypes.NoneGiven)
	{
		TileType = inputType;

		if (TileType == TileTypes.NoneGiven)
		{
			return; //don't populate the directions array
		}
		bool topLeft = false,
			topRight = false,
			left = false,
			right = false,
			bottomLeft = false,
			bottomRight = false;

		switch (inputType)
		{
			case TileTypes.SailTopLeft:
				right = true;
				topRight = true;
				bottomRight = true;
				bottomLeft = true;
				break;

			case TileTypes.SailTopRight:
				topLeft = true;
				left = true;
				bottomRight = true;
				bottomLeft = true;
				break;

			case TileTypes.DownEnd:
				topLeft = true;
				topRight = true;
				break;

			case TileTypes.LeftEnd:
				topRight = true;
				right = true;
				bottomRight = true;
				break;

			case TileTypes.RightEnd:
				topLeft = true;
				left = true;
				bottomRight = true;
				break;

			case TileTypes.SailBotLeft:
				topLeft = true;
				topRight = true;
				bottomRight = true;
				right = true;
				break;

			case TileTypes.SailBotRight:
				topRight = true;
				left = true;
				topLeft = true;
				bottomLeft = true;
				break;

			case TileTypes.UpEnd:
				bottomLeft = true;
				bottomRight = true;
				break;

			case TileTypes.Straight:
				left = true;
				right = true;
				break;

			case TileTypes.Middle:
				topLeft = true;
				topRight = true;
				left = true;
				right = true;
				bottomLeft = true;
				bottomRight = true;
				break;

			default:
				GD.PrintErr($"unaccounted for tiletype: {inputType}");
				return;
		}

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

public enum TileTypes
{
	NoneGiven,
	SailTopLeft,
	SailTopRight,
	DownEnd,
	LeftEnd,
	RightEnd,
	SailBotLeft,
	SailBotRight,
	UpEnd,
	Straight,
	Middle
}
