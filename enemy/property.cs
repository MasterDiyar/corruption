using Godot;
using System;

public partial class property : Node
{
	[Export] public float hp = 10;
	[Export] public float speed = 1000;
	[Export] public float damage = 1;
	
	private RigidBody2D root;
	Random random = new Random();
	public override void _Ready()
	{
		root = GetParent<RigidBody2D>();
	}

	public override void _Process(double delta)
	{
		if (hp <= 0)
		{
			if (root.GetParent() is Main nim)
			{
				nim.soul += 1;
			}
			var ranf = random.Next(4);
			var a= root.GetParent();
			var item= GD.Load<PackedScene>("res://player/items.tscn").Instantiate<Area2D>();
			if (ranf >= 3)
			{
				item.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://enemy/patron.png");
				item.SetMeta("type", "patron");
			}
			else if (ranf >= 1 && ranf < 3)
			{
				item.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://enemy/aidkit.png");
				item.SetMeta("type", "heal");
			}
			item.Position= root.Position;
			a.AddChild(item);
			root.QueueFree();
		}
	}
	
	//Ataka otdelno propisana v oruzhii lol
}
