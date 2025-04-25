using Godot;
using System;

public partial class property : Node
{
	[Export] public float hp = 10;
	[Export] public float speed = 1000;
	[Export] public float damage = 1;
	
	private Node root;
	public override void _Ready()
	{
		root = GetParent();
	}

	public override void _Process(double delta)
	{
		if (hp <= 0)
		{
			root.QueueFree();
		}
	}
}
