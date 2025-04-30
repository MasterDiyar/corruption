using Godot;
using System;

public partial class Infant : RigidBody2D
{
	private Node root;
	public Node2D player;
	public int diff = 400, aluis = 1200;
	[Export] public float speed = 100f;
	public override void _Ready()
	{
		root = GetParent();
		player = root.GetNode<Node2D>("Zhukov");
	}

	public override void _PhysicsProcess(double delta)
	{
		var difference = player.Position - Position;
		if (difference.X * difference.X + difference.Y * difference.Y < aluis * aluis&& difference.X * difference.X + difference.Y * difference.Y > diff*diff)
		{
			var angle = GetAngleTo(player.Position);
			Position += speed * (float)delta * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			SetMeta("ready", false);
		}
		else if(difference.X * difference.X + difference.Y * difference.Y > aluis * aluis){}
		else
		{
			SetMeta("ready", true);
		}
	}
}
