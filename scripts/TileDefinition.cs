
using Godot;
using Godot.Collections;

public class TileDefinition
{
    public bool NotPopulated { get; private set; } = true;
    public Vector2I AtlasCoordinates { get; set; }

	public Array<Vector2I> Directions = new Array<Vector2I>();

	public TileDefinition(bool topRight = false, bool right = false, bool bottomRight = false, bool bottomLeft = false, bool left = false, bool topLeft = false)
	{
        NotPopulated = false;
        
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