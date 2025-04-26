using Godot;
using System;

public partial class indicator : Node
{
	private Node root, property;
	private float maxhp, hp, hei;
	Line2D line;
	
	public override void _Ready()
	{
		line = GetNode<Line2D>("hpline");
		root = GetParent();
		property = root.GetNode("property");
		if (property is property a) maxhp = a.hp;
		hp = maxhp;
		Sprite2D icon = root.GetNode<Sprite2D>("Icon");
		float textureHeight = icon.Texture.GetHeight();
		hei = textureHeight / 2f + 30f;
		line.Position = new Vector2(0, -hei);
		
	}


	public override void _Process(double delta)
	{
		
		if (property is property a) hp = a.hp;
		line.ClearPoints();
		line.SetPoints(new Vector2[] {
			Vector2.Zero, new Vector2(150f * hp / maxhp, 0)
		});
		if (root is RigidBody2D rootArea)
			line.Position = rootArea.Position + new Vector2(-100, -hei);
	}
}
