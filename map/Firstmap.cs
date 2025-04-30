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


	public override void _Process(double delta)
	{
	}

	public void timeout()
	{
		RigidBody2D mob = GD.Load<PackedScene>("res://enemy/infant.tscn").Instantiate<RigidBody2D>();
		Node amn = GD.Load<PackedScene>("res://player/ak_47.tscn").Instantiate();
		mob.AddChild(amn);
		GetParent().AddChild(mob);
	}
}
