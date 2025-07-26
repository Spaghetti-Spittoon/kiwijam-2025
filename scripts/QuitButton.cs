using Godot;
using System;

public partial class QuitButton : TextureButton
{
	public override void _Ready()
	{
		this.ButtonUp += OnClick;
	}

	void OnClick()
	{
		GD.Print("Quit button clicked");
		var tree = this.GetTree();
		tree.Quit();
	}
}
