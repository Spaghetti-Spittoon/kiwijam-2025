using Godot;
using System;

public partial class Player : Area2D
{
	public static Player Instance;
	MapHandler map;

	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;
	public Vector2I Direction = Vector2I.Right;

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public override void _Ready()
	{
		Instance = this;
		ScreenSize = GetViewportRect().Size;
		var parent = GetParent();
		var layer = parent.GetNode<TileMapLayer>("TileMapLayer");
		map = new MapHandler(layer);
	}

	public override void _Process(double delta)
	{
		var oldPos = Position;
		Position += (float)delta * (Vector2)Direction * Speed;

		if (map.HasPassedHalfTile(oldPos, Position, Direction) || Direction == Vector2.Zero)
		{
			Position = map.SnapToHalfTile(Position);
			var tile = map.GetTileInfo(Position);

			if (Input.IsActionPressed("move_up"))
			{
				if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right + Vector2I.Up))
				{
					Direction = Vector2I.Right + Vector2I.Up;
				}
				if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left + Vector2I.Up))
				{
					Direction = Vector2I.Left + Vector2I.Up;
				}
			}
			else if (Input.IsActionPressed("move_down"))
			{
				if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right + Vector2I.Down))
				{
					Direction = Vector2I.Right + Vector2I.Down;
				}
				if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left + Vector2I.Down))
				{
					Direction = Vector2I.Left + Vector2I.Down;
				}
			}
			else
			{
				if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right))
				{
					Direction = Vector2I.Right;
				}

				if (Input.IsActionPressed("move_left"))
				{
					if (tile.Directions.Contains(Vector2I.Left))
					{
						Direction = Vector2I.Left;
					}
					else
					{
						GD.Print("Cannot move left at " + map.PixelToHalfTile(Position.X) + ":" + map.PixelToHalfTile(Position.Y));
					}
				}
			}

			//Stop if we're at a dead end
			if (tile.Directions.Contains(Direction) == false)
			{
				Direction = Vector2I.Zero;
			}

		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (Direction.Length() > 0)
		{
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);

		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
