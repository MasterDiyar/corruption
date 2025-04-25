using Godot;
using System;

public partial class Firstmap : Node2D
{
	private Timer timer;
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += () => 
		GetParent().AddChild(
				GD.Load<PackedScene>("res://enemy/test_slime.tscn").Instantiate()
			);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
