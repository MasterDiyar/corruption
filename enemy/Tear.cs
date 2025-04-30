using Godot;
using System;

public partial class Tear : Area2D
{
	public int Speed = 900;
	public float damage = 2;
	public string team = "player";
	public float wait = 1;
	public int timesbefore = 1;
    
	public override void _Ready()
	{
		var timer = GetNode<Timer>("Timer");
		timer.Timeout += _OnTimeout;
		BodyEntered += _OnEnter;
		timer.WaitTime = wait;
	}

	public override void _Process(double delta)
	{
		Position += Transform.X * Speed * (float)delta;
	}

	private void _OnTimeout()
	{
		QueueFree(); 
	}

	private void _OnEnter(Node area)
	{

		if (area.IsInGroup(team) && area.HasNode("attack"))
		{
			timesbefore--;
			if (area.GetNode("attack") is Attack a)
			{
				a.hp -= damage;
			}
            
			if (timesbefore <= 0)QueueFree();
		}
	}
}
