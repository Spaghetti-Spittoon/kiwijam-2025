using Godot;
using System;

public partial class Phone : Node2D
{
	[Export]
	public bool target { get; set; } = false;

	public override void _Ready()
	{
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = !target;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			(GetParent() as Level).DropPoints();
		}
	}
}
