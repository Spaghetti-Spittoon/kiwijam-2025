using Godot;
using System;

public partial class StartButton : Button
{
	PackedScene level;

	public override void _Ready()
	{
		level = GD.Load<PackedScene>("res://scenes/Level.tscn");
		ButtonUp += OnClick;
	}

	public void OnClick()
	{
		var instance = level.Instantiate<Level>();

		var parent = this.GetParent();
		var cast2D = (CanvasLayer)parent;
		cast2D.AddSibling(instance);
		cast2D.Hide();
	}
}
