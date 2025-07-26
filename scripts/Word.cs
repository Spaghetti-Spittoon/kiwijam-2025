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
		Position = map.SnapToHalfTile(Position);
	}

	public override void _Process(double delta)
	{
		//Too close to the player
		if ((Position - Player.Instance.Position).Length() < 50)
		{
			return;
		}

		var oldPos = Position;
		Position += (float)delta * (Vector2)Direction * Player.Instance.Speed;

		if (map.IsCentered(oldPos))
		{
			//We want to try and chase the player
			Position = map.SnapToHalfTile(Position);
			var tile = map.GetTileInfo(Position);
			Direction = TryGetDirection(tile);
		}
			
	}

	private Vector2I TryGetDirection(TileDefinition tile)	
	{		
		var playerIsRight = Player.Instance.Position.X > Position.X;
		var playerIsLeft = Player.Instance.Position.X < Position.X;
		var playerIsDown = Player.Instance.Position.Y > Position.Y;
		var playerIsUp = Player.Instance.Position.Y < Position.Y;

		if (playerIsUp)
		{
			if(playerIsRight && tile.Directions.Contains(Vector2I.Up + Vector2I.Right))
			{
				return Vector2I.Up + Vector2I.Right;
			}
			if (playerIsLeft && tile.Directions.Contains(Vector2I.Up + Vector2I.Left))
			{
				return Vector2I.Up + Vector2I.Left;
			}
		}

		if (playerIsDown)
		{
			if (playerIsRight && tile.Directions.Contains(Vector2I.Down + Vector2I.Right))
			{
				return Vector2I.Down + Vector2I.Right;
			}
			if (playerIsLeft && tile.Directions.Contains(Vector2I.Down + Vector2I.Left))
			{
				return Vector2I.Down + Vector2I.Left;
			}
		}

		if (playerIsRight && tile.Directions.Contains(Vector2I.Right))
		{
			return Vector2I.Right;
		}
		if (playerIsLeft && tile.Directions.Contains(Vector2I.Left))
		{
			return Vector2I.Left;
		}

		if (tile.Directions.Count > 0)
		{
			return tile.Directions.PickRandom();
		}
		return Vector2I.Zero;
	}

	public void OnTextResized()
	{
		if (Collision.Shape is RectangleShape2D)
		{
			GD.Print("Updated collision shape size");
			(Collision.Shape as RectangleShape2D).Size = Text.GetMinimumSize();
		}
	}

	public void SetText(string text)
	{
		Text.Text = text;
		// OnTextResized();
	}

	public void Die()
	{
		EmitSignal(SignalName.WordDestroyed, this);
		QueueFree();
	}
}
