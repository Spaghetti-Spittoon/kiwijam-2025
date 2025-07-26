using Godot;
using System;

public partial class Word : Area2D
{
	[Signal]
	public delegate void WordDestroyedEventHandler(Word whatWasDestroyed);

	[Export]
	public CollisionShape2D Collision { get; set; }

	[Export]
	public RichTextLabel Text { get; set; }

	public override void _Ready()
	{
		base._Ready();
		if (Text is null || Collision is null)
		{
			GD.PushError("Word instance doesn't have Collision or Text set");
		}
	}

	public void OnTextResized()
	{
		if (Collision.Shape is RectangleShape2D)
		{
			GD.Print("Updated collision shape size");
			(Collision.Shape as RectangleShape2D).Size = Text.GetMinimumSize();
		}
	}

	public void SetText(string text)
	{
		Text.Text = text;
		// OnTextResized();
	}

	public void Die()
	{
		EmitSignal(SignalName.WordDestroyed, this);
		QueueFree();
	}
}
