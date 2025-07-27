using Godot;
using System;

public partial class Level : Node2D
{
	private PackedScene enemyScene;

	TileMapLayer mapLayer;
	SignalBus bus;
	Grid grid;
	Button button;

	public override void _Ready()
	{
		enemyScene = ResourceLoader.Load("res://scenes/enemy.tscn") as PackedScene;
		mapLayer = GetNode<TileMapLayer>("TileMapLayer");
		bus = GetNode<SignalBus>("/root/SignalBus");
		button = GetNode<Button>("TestIncreaseButton");
		grid = GetNode<Grid>("/root/Grid");
		button.ButtonUp += OnLevelExpanded;
		bus.LevelExpanded += OnLevelExpanded;
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
		}
	}

	private void AddEnemy()
	{
		Area2D enemy = enemyScene.Instantiate<Area2D>();

		var enemySpawnLocation = GetNode<Marker2D>("EnemySpawnLocation");

		enemy.Position = enemySpawnLocation.Position;

		AddChild(enemy);
	}
}
