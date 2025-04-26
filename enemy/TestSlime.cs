using Godot;
using System;

public partial class TestSlime : RigidBody2D
{
	private Node root;
	private Node2D player;
	public override void _Ready()
	{
		root = GetParent();
		player = root.GetNode<Node2D>("Zhukov");
	}

	
	public override void _PhysicsProcess(double delta)
	{
		var difference = player.Position - Position;
		if (difference.X * difference.X + difference.Y + difference.Y > 40000)
		{
			var angle = GetAngleTo(player.Position);
			Position += 5 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}
