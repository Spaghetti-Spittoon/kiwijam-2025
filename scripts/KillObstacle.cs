using Godot;
using System;

public partial class KillObstacle : Area2D
{
	private enum State
	{
		Safe,
		Dangerous,
		Finished,
	}

	[Export]
	public AnimatedSprite2D animatedSprite;

	[Export]
	public string SafeAnimationName;

	[Export]
	public string DangerousAnimationName;

	[Export]
	public string FinishedAnimationName;

	[Export]
	public double SafeTime;

	[Export]
	public double DangerousTime;

	[Export]
	public double FinishTime;

	private State _state = State.Safe;
	private double _remainingSafeTime = 0.0;
	private double _remainingDangerTime = 0.0;
	private double _remainingFinishTime = 0.0;

	public void SetParameters(double safeTime, double dangerousTime)
	{
		SafeTime = safeTime;
		DangerousTime = dangerousTime;
	}

	public override void _Ready()
	{
		base._Ready();
		if (animatedSprite is null
		|| DangerousAnimationName == ""
		|| FinishedAnimationName == "")
		{
			GD.PushError("KillObstacle not setup properly! (animatedSprite or DangerousAnimationName)");
		}
		_remainingSafeTime = SafeTime;
	}

	public void BecomeDangerous()
	{
		GD.Print("KillObstacle becoming dangerous");
		animatedSprite.Play(DangerousAnimationName);
		_state = State.Dangerous;
		_remainingDangerTime = DangerousTime;
		foreach (var body in GetOverlappingAreas())
		{
			DealWithCollidedNode(body);
		}
	}

	public void OnCollide(Node node)
	{
		if (_state != State.Dangerous)
		{
			GD.Print("KillObstacle Collision but obstacle not dangerous");
			return;
		}
		DealWithCollidedNode(node);
	}

	// Assumed to be called when _isDangerous = true
	private void DealWithCollidedNode(Node node)
	{
		if (node.IsInGroup("words"))
		{
			if (node is not Word)
			{
				GD.PushError("Obstacle collided with 'words' group node that wasn't a word");
				return;
			}
			GD.Print("Word collision with KillObstacle");
			_state = State.Finished;
			_remainingFinishTime = FinishTime;
			animatedSprite.Play(FinishedAnimationName);
			(node as Word).Die();
		}
		else
		{
			GD.Print("Something else collision with KillObstacle");
		}
	}

	public override void _Process(double delta)
	{
		switch (_state)
		{
			case State.Safe:
				_remainingSafeTime -= delta;
				if (_remainingSafeTime < 0.0)
					BecomeDangerous();
				return;
			case State.Dangerous:
				_remainingDangerTime -= delta;
				if (_remainingDangerTime < 0.0)
				{
					GD.Print("Obstacle expired");
					QueueFree();
				}
				return;
			case State.Finished:
				_remainingFinishTime -= delta;
				if (_remainingFinishTime <= 0.0)
				{
					GD.Print("Obstacle 'finished'");
					QueueFree();
				}
				return;
		}
	}
}
