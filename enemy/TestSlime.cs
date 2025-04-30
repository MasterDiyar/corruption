using Godot;
using System;

public partial class TestSlime : RigidBody2D
{
	private Node root;
	public Node2D player;
	Timer timer;
	public int diff = 400, aluis = 1200;
	[Export] public float speed = 100f;
	private bool read = false;
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += cout;
		root = GetParent();
		player = root.GetNode<Node2D>("Zhukov");
	}

	public void cout()
	{
		if (read)
		{
			var tear = GD.Load<PackedScene>("res://enemy/tear.tscn").Instantiate<Area2D>();
			if (tear is Tear e)
			{
				e.Position = Position;
				e.Rotation = GetAngleTo(player.Position);
				e.Speed = 4000;
			}
			GetParent().AddChild(tear);
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		var difference = player.Position - Position;
		if (difference.X * difference.X + difference.Y * difference.Y > diff*diff)
		{
			var angle = GetAngleTo(player.Position);
			Position += speed * (float)delta * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			read = false;
		}
		else if (difference.X * difference.X + difference.Y * difference.Y > aluis * aluis)
		{
			read = false;
		}
		else
		{
			read = true;
		}
	}
}
