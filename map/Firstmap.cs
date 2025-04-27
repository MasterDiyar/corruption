using Godot;
using System;

public partial class Firstmap : Node2D
{
	private Timer timer;
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += timeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void timeout()
	{
		Node mob = GD.Load<PackedScene>("res://enemy/test_slime.tscn").Instantiate();
		Node amn = GD.Load<PackedScene>("res://player/ak_47.tscn").Instantiate();
		mob.AddChild(amn);
		GetParent().AddChild(mob);
	}
}
