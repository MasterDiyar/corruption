using Godot;
using System;

public partial class Ak47 : Sprite2D
{
	private Node2D player, mob;
	private Node root;
	Timer timer;
	private int strelnul = 0;
	public override void _Ready()
	{
		mob = GetParent<Node2D>();
		root = mob.GetParent<Node>();
		player = root.GetNode<Node2D>("Zhukov");
		timer = GetNode<Timer>("Timer");
		LookAt(player.Position);
		timer.Timeout += _attack;
	}

	public override void _Process(double delta)
	{
		LookAt(player.Position);
		Position =  100 * new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
		if ((bool)mob.GetMeta("ready")) {
			if (strelnul == 0) {
				strelnul = 3;
				timer.Start();
			}
		}
		else
		{
			strelnul = 0;
			timer.Stop();
		}
	}

	public void _attack()
	{
		strelnul--;
		var sharp = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Area2D>();
		sharp.Position = GlobalPosition + new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * 10f;
		sharp.Rotation = Rotation;
		if (sharp is svinets a) a.team = "player";
		root.AddChild(sharp);
	}
}
