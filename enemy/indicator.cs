using Godot;
using System;

public partial class indicator : Node
{
	private Node root, property;
	private float maxhp, hp, hei, textureHeight, textureWidth;
	Line2D line;
	
	public override void _Ready()
	{
		line = GetNode<Line2D>("hpline");
		root = GetParent();
		property = root.GetNode("property");
		if (property is property a) maxhp = a.hp;
		hp = maxhp;
		Sprite2D icon = root.GetNode<Sprite2D>("Icon");
		textureHeight = icon.Texture.GetHeight();
		textureWidth = icon.Texture.GetWidth();
		hei = textureHeight / 2f + 30f;
		line.Position = new Vector2(0, -hei);
		
	}


	public override void _Process(double delta)
	{
		
		if (property is property a) hp = a.hp;
		line.ClearPoints();
		line.SetPoints(new Vector2[] {
			Vector2.Zero, new Vector2(textureWidth / 2 * hp / maxhp, 0)
		});
		if (root is RigidBody2D rootArea)
			line.Position = rootArea.Position + new Vector2(-textureWidth/4, -hei);
	}
}
