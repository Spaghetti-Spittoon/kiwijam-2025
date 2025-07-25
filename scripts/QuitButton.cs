using Godot;
using System;

public partial class QuitButton : Button
{
	public override void _Ready()
	{
		this.ButtonUp += OnClick;
	}

	void OnClick()
	{
		Console.WriteLine("Quit button clicked");
		var tree = this.GetTree();
		tree.Quit();
	}
}
