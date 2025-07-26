using Godot;
using System;

public partial class Level : Node2D
{
	TileMapLayer mapLayer;
	SignalBus bus;
	Grid grid;
	Button button;

	public override void _Ready()
	{
		mapLayer = GetNode<TileMapLayer>("TileMapLayer");
		bus = GetNode<SignalBus>("/root/SignalBus");
		button = GetNode<Button>("TestIncreaseButton");
		button.ButtonUp += OnLevelExpanded;
		bus.LevelExpanded += OnLevelExpanded;
	}

	void OnLevelExpanded()
	{
		Console.WriteLine(nameof(OnLevelExpanded));
		grid.ExpandOneLevel(mapLayer);
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
}
