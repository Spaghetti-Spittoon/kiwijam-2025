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
	public int Speed { get; set; } = 100;

	public Vector2 ScreenSize;

	Vector2 PlayerInput = Vector2.Zero;
	public Vector2 Direction = Vector2.Right;

	private TileMapLayer tileMapLayer;

	bool hasHitBoundary = false;

	private int score = 0;

	private Label scoreLabel;

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
		scoreLabel = GetNode<Label>("ScoreLabel");
		scoreLabel.Hide();
	}

	public override void _Process(double delta)
	{
		var oldPos = Position;
		Position += (float)delta * (Vector2)Direction * Speed;
		var tile = map.GetTileInfo(Position);

		if (tile.TileType == TileTypes.NoneGiven && hasHitBoundary == false)
		{
			hasHitBoundary = true;
			Direction = -Direction;
			GD.Print($"invalid type type. direction flipped to: {Direction}");
		}

		var isCentered = IsPlayerCentered(oldPos);
		var directionSet = false;
		var currentTilePos = map.SnapToHalfTile(Position);

		var enumType = tile.TileType.GetType();
		var enumName = Enum.GetName(enumType, tile.TileType);
		GD.Print($"{nameof(Player)}: oldpos: {oldPos}, Position: {Position}, {nameof(Player)}: snappedPosition: {Position}, tile: {enumName}");
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
				directionSet = true;
				PlayerInput = Vector2.Right + Vector2.Up;
				GD.Print($"player, move up right direction: {Direction}");
			}
			if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left + Vector2I.Up))
			{
				directionSet = true;
				PlayerInput = Vector2.Left + Vector2.Up;
				GD.Print($"player, move up left direction: {Direction}");
			}
		}

		else if (Input.IsActionPressed("move_down"))
		{
			if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right + Vector2I.Down))
			{
				directionSet = true;
				PlayerInput = Vector2.Right + Vector2.Down;
				GD.Print($"player, move down right direction: {Direction}");
			}

			if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left + Vector2I.Down))
			{
				directionSet = true;
				PlayerInput = Vector2.Left + Vector2.Down;
				GD.Print($"player, move down left direction: {Direction}");
			}
		}

		else
		{
			if (Input.IsActionPressed("move_right") && tile.Directions.Contains(Vector2I.Right))
			{
				directionSet = true;
				PlayerInput = Vector2.Right;
				GD.Print($"player, move right direction: {Direction}");
			}

			else if (Input.IsActionPressed("move_left") && tile.Directions.Contains(Vector2I.Left))
			{
				directionSet = true;
				PlayerInput = Vector2.Left;
				GD.Print($"player, move left direction: {Direction}");
			}
		}

		if (PlayerInput == -Direction && directionSet) //let the player move backwards at any time
		{
			hasHitBoundary = false;
			GD.Print($"flipped on user input to: {PlayerInput}");
			Direction = PlayerInput;
			return;
		}

		if (isCentered && directionSet)
		{
			hasHitBoundary = false;
			GD.Print("direction SET");
			Direction = PlayerInput;
			return;
		}

		//let the player keep moving without input
		if (tile.Directions.Contains((Vector2I)Direction))
		{
			GD.Print($"valid auto direction: {Direction}");
			hasHitBoundary = false;
			return;
		}

		if (hasHitBoundary)
		{
			GD.Print($"has hit boundary. current direction: {Direction}");
			return; //continue travelling in the opposite direction
		}
		hasHitBoundary = true;
		Direction = -Direction;
		GD.Print($"player has reached a boundary. Turning arround to direction: {Direction}");
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
		if (body is EnemyHandler)
		{
			Hide();
			EmitSignal(SignalName.Hit);

			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}

	public bool IsPlayerCentered(Vector2 oldPos, CanvasItem canvas = null)
	{
		const int boundaryOffset = 25;
		//Check that we weren't previously standing on the boundary
		var snapped = map.SnapToFullTiles(oldPos); //assume this is the center
		var offsetFifty = new Vector2I(snapped.X - 50, snapped.Y - 50);
		var diffX = Math.Abs(offsetFifty.X - oldPos.X);
		var diffY = Math.Abs(offsetFifty.Y - oldPos.Y);
		var crossedCenterX = diffX < boundaryOffset;
		var crossedCenterY = diffY < boundaryOffset;
		var isCentered = crossedCenterX && crossedCenterY;
		var message = $"snapped: {snapped}, offsetFifty: {offsetFifty}, diffX: {diffX}, diffY: {diffY}, crossedCenterX: {crossedCenterX}, crossedCenterY: {crossedCenterY}, isCentered: {isCentered}";
		// GD.Print(message);

		if (canvas != null) //print a string on screen
		{
			Font font = ThemeDB.FallbackFont;
			var position = new Vector2(-400, -200);
			canvas.DrawString(font, position, message, HorizontalAlignment.Left, -1, 24);
		}

		if (crossedCenterX && crossedCenterY)
		{
			return true;
		}
		return false;

	}

	public void AddPoint()
	{
		score++;
		scoreLabel.Text = $"{score}";
		scoreLabel.Show();
	}

	public int DumpPoints()
	{
		int currentPoints = score;
		score = 0;
		scoreLabel.Hide();
		return currentPoints;
	}

}
