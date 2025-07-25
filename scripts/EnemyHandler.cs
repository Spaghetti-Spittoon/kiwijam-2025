using Godot;
using System;

public partial class EnemyHandler : Area2D
{
	[Export]
	public int GridSize = 8;

	[Export]
	public int MoveSpeed = 1;

	public override void _Process(double delta)
	{
		Position += (float)delta * MoveSpeed * Vector2.Right;
	}

}
