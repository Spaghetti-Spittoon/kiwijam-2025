using Godot;
using System;
using static Godot.TextServer;

public partial class Word : Area2D
{
	private MapHandler map;

	[Signal]
	public delegate void WordDestroyedEventHandler(Word whatWasDestroyed);

	[Export]
	public CollisionShape2D Collision { get; set; }

	[Export]
	public RichTextLabel Text { get; set; }

	[Export]
	public int MoveSpeed { get; set; } = 100;

	Vector2I Direction;

	public override void _Ready()
	{
		base._Ready();
		if (Text is null || Collision is null)
		{
			GD.PushError("Word instance doesn't have Collision or Text set");
		}

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
		Position = map.SnapToCenterOfTile(Position);

		var tile = map.GetTileInfo(Position);

		if (tile.TileType == TileTypes.NoneGiven)
		{
			// If we go off the map, Turn around!
			Direction = -Direction;
		}
		else
		{
			Direction = tile.Directions.PickRandom();
		}
		// Also nudge enemy in its new direction to avoid re-triggering crossing the middle of a tile
		Position += (Vector2)Direction * 0.01F;
	}


	public void SetText(string text)
	{
		Text.Text = text;
	}

	public void Die()
	{
		EmitSignal(SignalName.WordDestroyed, this);
		QueueFree();
	}

	private void OnAreaEntered(Area2D area2D)
	{
		Die();
		if (area2D is Player)
		{
			(area2D as Player).AddPoint();
		}
	}
}
