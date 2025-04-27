using Godot;
using System;

public partial class Menu : Control
{
	Button playButton, quitButton;

	public override void _Ready()
	{
		playButton = GetNode<Button>("Button");
		quitButton = GetNode<Button>("Button2");
		playButton.Pressed += () =>
		{
			
			GetParent().AddChild(GD.Load<PackedScene>("res://map/firstmap.tscn").Instantiate());
			GetParent().AddChild(GD.Load<PackedScene>("res://player/zhukov.tscn").Instantiate());
			QueueFree();
		};
		quitButton.Pressed += (() => { GetTree().Quit(); });
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
}
