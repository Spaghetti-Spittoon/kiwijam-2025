using Godot;
using System;

public partial class Player : Area2D
{
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
		ScreenSize = GetViewportRect().Size;
	}
	
	public override void _Process(double delta)
	{

		var oldPos = Position;
		Position += (float)delta * (Vector2)Direction * Speed;

		if(MapHandler.HasPassedTileCenter(oldPos,Position,Direction) || Direction == Vector2.Zero)
		{
			var directions = MapHandler.Instance.GetDirectionsAtNodeCenter(Position);

			if (Input.IsActionPressed("move_up"))
			{
				if (Input.IsActionPressed("move_right") && directions.Contains(Vector2I.Right + Vector2I.Up))
				{
					Direction = Vector2I.Right + Vector2I.Up;
				}
				if (Input.IsActionPressed("move_left") && directions.Contains(Vector2I.Left + Vector2I.Up))
				{
					Direction = Vector2I.Left + Vector2I.Up;
				}
			}
			else if (Input.IsActionPressed("move_down"))
			{
				if (Input.IsActionPressed("move_right") && directions.Contains(Vector2I.Right + Vector2I.Down))
				{
					Direction = Vector2I.Right + Vector2I.Down;
				}
				if (Input.IsActionPressed("move_left") && directions.Contains(Vector2I.Left + Vector2I.Down))
				{
					Direction = Vector2I.Left + Vector2I.Down;
				}
			}
			else
			{
				if (Input.IsActionPressed("move_right") && directions.Contains(Vector2I.Right))
				{
					Direction = Vector2I.Right;
				}
				if (Input.IsActionPressed("move_left") && directions.Contains(Vector2I.Left))
				{
					Direction = Vector2I.Left;
				}
			}

			//Stop if we're at a dead end
			if(directions.Contains(Direction) == false)
			{
				Direction = Vector2I.Zero;
			}

		}


		/*
		var velocity = Vector2.Zero;
		
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		} else
		{
			animatedSprite2D.Stop();
		}
		
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		*/
	}
	
	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);

		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
