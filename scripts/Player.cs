using Godot;
using System;
using System.Text;

public partial class Player : Area2D
{
	public static Player Instance;
	MapHandler map;

	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;
	public Vector2 Direction = Vector2.Right;

	private TileMapLayer tileMapLayer;

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
		tileMapLayer = parent.GetNode<TileMapLayer>("TileMapLayer");
		map = new MapHandler(tileMapLayer);
		SetCameraTimeMapLayer();
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}

	public override void _Process(double delta)
	{
		var oldPos = Position;
		Position += (float)delta * (Vector2)Direction * Speed;
		var isCentered = map.IsCentered(oldPos);
		GD.Print($"{nameof(Player)}: oldpos: {oldPos}, Position: {Position}");

		if (isCentered)
		{
			var currentTilePos = map.SnapToHalfTile(Position);
			var tile = map.GetTileInfo(Position);
			var enumType = tile.TileType.GetType();
			var enumName = Enum.GetName(enumType, tile.TileType);
			GD.Print($"{nameof(Player)}: snappedPosition: {Position}, tile: {enumName}");
			var positionsString = new StringBuilder();

			for (int i = 0; i < tile.Directions.Count; i++)
			{
				if (i == 0)
				{
					positionsString.Append("player: ");
				}
				var currentDirection = tile.Directions[i];
				positionsString.Append($"{currentDirection}");

				if (i == tile.Directions.Count - 1)
				{
					positionsString.AppendLine();
				}
			}
			GD.Print(positionsString);

			if (Input.IsActionPressed("move_up"))
			{
				if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right + Vector2I.Up))
				{
					var destinationTilePos = new Vector2(currentTilePos.X + 100, currentTilePos.Y - 100);
					Direction = (destinationTilePos - Position).Normalized();
					GD.Print($"player, move up right direction: {Direction}");
				}
				if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left + Vector2I.Up))
				{
					Direction = Vector2I.Left + Vector2I.Up;
					GD.Print($"player, move up left direction: {Direction}");
				}
			}
			else if (Input.IsActionPressed("move_down"))
			{
				if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right + Vector2I.Down))
				{
					Direction = Vector2I.Right + Vector2I.Down;
					GD.Print($"player, direction: {Direction}");
				}
				if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left + Vector2I.Down))
				{
					Direction = Vector2I.Left + Vector2I.Down;
					GD.Print($"player, direction: {Direction}");
				}
			}
			else
			{
				if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right))
				{
					Direction = Vector2I.Right;
					GD.Print($"player, direction: {Direction}");
				}

				else if (Input.IsActionPressed("move_left"))
				{
					if (tile.Directions.Contains(Vector2I.Left))
					{
						Direction = Vector2I.Left;
						GD.Print($"player, direction: {Direction}");
					}
					else
					{
						GD.Print("Cannot move left at " + map.PixelToHalfTile(Position.X) + ":" + map.PixelToHalfTile(Position.Y));
					}
				}

				else
				{
					GD.Print($"automatically choosing direction");

					if (tile.Directions.Contains(Vector2I.Right))
					{
						GD.Print("automatically moved right");
						Direction = Vector2I.Right;
					}

					else if(tile.Directions.Contains(Vector2I.Left))
					{
						GD.Print("automatically moved left");
						Direction = Vector2I.Left;
					}
				}
			}
		}
	}

	private void SetCameraTimeMapLayer()
	{
		var camera = GetNode<Camera2D>("Camera2D") as CameraLimiter;

		if (tileMapLayer != null)
		{
			camera.SetCameraLimits(tileMapLayer);
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);

		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
