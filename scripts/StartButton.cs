using Godot;
using System;
public partial class StartButton : TextureButton
{
	PackedScene level;
	public override void _Ready()
	{
		level = GD.Load<PackedScene>("res://scenes/Level.tscn");
		ButtonUp += OnClick;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("add_coin"))
		{
			OnClick();
		}
	}
	
	public void OnClick()
	{
	GetTree().ChangeSceneToFile("res://scenes/Level.tscn");
	}
}
