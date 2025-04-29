using Godot;
using System;

public partial class property : Node
{
	[Export] public float hp = 10;
	[Export] public float speed = 1000;
	[Export] public float damage = 1;
	
	private RigidBody2D root;
	public override void _Ready()
	{
		root = GetParent<RigidBody2D>();
	}

	public override void _Process(double delta)
	{
		if (hp <= 0)
		{
			var a= root.GetParent();
			var item= GD.Load<PackedScene>("res://player/items.tscn").Instantiate<Area2D>();

			item.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://enemy/patron.png");
			item.SetMeta("type", "patron");
			item.Position= root.Position;
			a.AddChild(item);
			root.QueueFree();
		}
	}
	
	//Ataka otdelno propisana v oruzhii lol
}
