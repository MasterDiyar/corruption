using Godot;
using System;

public partial class TestSlime : RigidBody2D
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
		if (difference.X * difference.X + difference.Y * difference.Y > diff*diff)
		{
			var angle = GetAngleTo(player.Position);
			Position += speed * (float)delta * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			SetMeta("ready", false);
			GD.Print("dvigaus");
		}
		else if (difference.X * difference.X + difference.Y * difference.Y > aluis * aluis)
		{
			GD.Print("Ne dvigaus");
		}
		else
		{
			SetMeta("ready", true);
		}
	}
}
