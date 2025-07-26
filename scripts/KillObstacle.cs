using Godot;
using System;

public partial class KillObstacle : Area2D
{
	[Export]
	public AnimatedSprite2D animatedSprite;

	[Export]
	public string SafeAnimationName;

	[Export]
	public string DangerousAnimationName;

	[Export]
	public double SafeTime;

	[Export]
	public double DangerousTime;

	private bool _isDangerous = false;
	private double _remainingSafeTime = 0.0;
	private double _remainingDangerLife = 0.0;

	public void SetParameters(double safeTime, double dangerousTime)
	{
		SafeTime = safeTime;
		DangerousTime = dangerousTime;
	}

	public override void _Ready()
	{
		base._Ready();
		if (animatedSprite is null || DangerousAnimationName == "")
		{
			GD.PushError("KillObstacle not setup properly! (animatedSprite or DangerousAnimationName)");
		}
		_remainingSafeTime = SafeTime;
	}

	public void BecomeDangerous()
	{
		GD.Print("KillObstacle becoming dangerous");
		animatedSprite.Play(DangerousAnimationName);
		_isDangerous = true;
		_remainingDangerLife = DangerousTime;
		foreach (var body in GetOverlappingAreas())
		{
			DealWithCollidedNode(body);
		}
	}

	public void OnCollide(Node node)
	{
		if (!_isDangerous)
		{
			GD.Print("KillObstacle Collision but obstacle not dangerous yet");
			return;
		}
		DealWithCollidedNode(node);
	}

	// Assumed to be called when _isDangerous = true
	private void DealWithCollidedNode(Node node)
	{
		if (node.IsInGroup("words"))
		{
			GD.Print("Word collision with KillObstacle");
			node.QueueFree();
			QueueFree();
		}
		else
		{
			GD.Print("Something else collision with KillObstacle");
		}
	}

	public override void _Process(double delta)
	{
		if (!_isDangerous)
		{
			_remainingSafeTime -= delta;
			if (_remainingSafeTime <= 0.0)
			{
				BecomeDangerous();
			}
			return;
		}

		_remainingDangerLife -= delta;

		if (_remainingDangerLife < 0.0)
		{
			GD.Print("KillObstacle expiring");
			QueueFree();
		}
	}
}
