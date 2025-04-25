using Godot;
using System;

public partial class Menu : Control
{
	Button playButton;
	[Export] private string playNode;
	public override void _Ready()
	{
		playButton = GetNode<Button>("Button");
		playButton.Pressed += () =>
		{
			GetParent().AddChild(GD.Load<PackedScene>($"{playNode}").Instantiate());
			QueueFree();
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
}
