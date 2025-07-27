using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Level : Node2D
{
	private Godot.Collections.Dictionary<PackedScene, float> WeightedObstacles;

	private PackedScene enemyScene;
	private PackedScene wordScene;

	private string[] words;

	TileMapLayer mapLayer;
	SignalBus bus;
	Grid grid;
	Button button;
	RandomNumberGenerator rng;
	List<Node2D> spawnedObstacles;
	MapHandler mapHandler;

	public override void _Ready()
	{
		enemyScene = ResourceLoader.Load("res://scenes/enemy.tscn") as PackedScene;
		wordScene = ResourceLoader.Load("res://scenes/word.tscn") as PackedScene;
		mapLayer = GetNode<TileMapLayer>("TileMapLayer");
		bus = GetNode<SignalBus>("/root/SignalBus");
		button = GetNode<Button>("TestIncreaseButton");
		grid = GetNode<Grid>("/root/Grid");
		button.ButtonUp += OnLevelExpanded;
		bus.LevelExpanded += OnLevelExpanded;

		var wordsFile = FileAccess.Open("res://assets/phone_words.json", FileAccess.ModeFlags.Read);
		var wordsString = wordsFile.GetAsText();
		var parsedJson = Json.ParseString(wordsString);
		var jsonAsDictionary = parsedJson.AsGodotDictionary();
		words = (string[]) jsonAsDictionary["phone_words"];

		wordsFile.Close();

		rng = new RandomNumberGenerator();
		spawnedObstacles = new List<Node2D>();
		mapHandler = new MapHandler(mapLayer);

		WeightedObstacles = new Godot.Collections.Dictionary<PackedScene, float>
		{
			{GD.Load<PackedScene>("res://scenes/obstacles/obstacle_line_cut.tscn"), 1.0F},
		};

		AddEnemy();
	}

	void OnLevelExpanded()
	{
		GD.Print(nameof(OnLevelExpanded));
		int sourceId = mapLayer.TileSet.GetSourceId(0);
		grid.ExpandOneLevel(mapLayer, sourceId);
	}

	TileSetSource GetNamedTile(string tileName)
	{
		var tileset = mapLayer.TileSet;
		var numTiles = tileset.GetSourceCount();

		for (int i = 0; i < numTiles; i++)
		{
			var current = tileset.GetSource(i);

			if (current.ResourceName == tileName)
			{
				return current;
			}
		}
		return null;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("add_coin"))
		{
			AddEnemy();
			const uint numObstacles = 3;
			for (int i = 0; i < numObstacles; ++i)
			{
				AddObstacle();
			}
		}
	}

	private void AddEnemy()
	{
		Area2D enemy = enemyScene.Instantiate<Area2D>();

		var enemySpawnLocation = GetNode<Marker2D>("EnemySpawnLocation");

		enemy.Position = enemySpawnLocation.Position;

		AddChild(enemy);
	}

	private void OnWordTimerTimeout()
	{
		Word word = wordScene.Instantiate<Word>();

		var phoneLocation = GetNode<Node2D>("Phone");

		string nextWord = words[GD.Randi() % words.Length];

		word.SetText(nextWord);

		word.Position = phoneLocation.Position;

		AddChild(word);
	}

	private void AddObstacle()
	{
		// Already occupied spots
		var occupiedSpots = spawnedObstacles.Where(IsInstanceValid).Select(
			obstacle => obstacle.Position
		);

		// Find available spots
		const float halfEdgeLength = 50.0F;

		var possibleSpots = mapLayer.GetUsedCells().SelectMany((position) =>
		{
			var center = mapLayer.ToGlobal(mapLayer.MapToLocal(position));
			return mapHandler.GetTileAtCellCoords(position).Directions.Select(
				(direction) => (center + (Vector2)direction * halfEdgeLength).Round()
			);
		}).Distinct().Except(occupiedSpots).ToArray();

		if (possibleSpots.Length < 1)
		{
			GD.Print("No possible spots for an obstacle");
			return;
		}

		var rng = new RandomNumberGenerator();

		if (possibleSpots.Length < 1)
		{
			GD.Print("No spots to spawn an obstacle");
			return;
		}

		var spot = possibleSpots[rng.RandiRange(0, possibleSpots.Length - 1)];

		var obstacleScene = pickRandomObstacle();

		var newObstacle = obstacleScene.Instantiate();
		if (newObstacle is not Node2D)
		{
			GD.PushError("Picked an Obstacle Scene that wasn't a Node2D!");
			return;
		}
		var newObstacle2D = newObstacle as Node2D;
		AddChild(newObstacle);
		newObstacle2D.Position = spot;
		spawnedObstacles.Add(newObstacle2D);
		GD.Print($"Created Obstacle at: {spot}");
	}

	private PackedScene pickRandomObstacle()
	{
		if (WeightedObstacles.Count < 1)
		{
			GD.PushError("No obstacles available to pick from");
			return null;
		}
		var totalWeight = WeightedObstacles.Select((scene_weight) => scene_weight.Value).Sum();

		var randomWeight = rng.RandfRange(0.0F, totalWeight);

		foreach (var scene_weight in WeightedObstacles)
		{
			if (randomWeight <= scene_weight.Value)
			{
				return scene_weight.Key;
			}
			randomWeight -= scene_weight.Value;
		}
		return WeightedObstacles.First().Key;
	}
}
