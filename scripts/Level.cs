using Godot;
using System;

public partial class Level : Node2D
{
    TileMapLayer mapLayer;
    SignalBus bus;

    public override void _Ready()
    {
        mapLayer = GetNode<TileMapLayer>("TileMapLayer");
        bus = GetNode<SignalBus>("SignalBus");
        bus.LevelExpanded += OnLevelExpanded;
    }

    void OnLevelExpanded()
    {
        mapLayer.
    }
}
