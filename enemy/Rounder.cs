using Godot;
using System;

public partial class Rounder : Node
{
	private Sprite2D s1, s2;
	[Export]Vector2 size = Vector2.One;
	[Export] private Texture2D pl1, pl2;
	public bool iskazhen = false;
	[Export] private Color m1, m2;
	[Export] private RigidBody2D root;
	public override void _Ready()
	{
		
		s1 = GetNode<Sprite2D>("Eblota");
		s2 = GetNode<Sprite2D>("Eblota2");
		if (pl1 != null && pl2 != null)
		{
			s1.Texture = pl1;
			s2.Texture = pl2;
		}
		s1.Modulate = m1;
		s2.Modulate = m2;
		s1.Scale = size;
		s2.Scale = size;

		if (iskazhen)
		{
			s1.Visible = false;
			s2.Visible = false;
		}
		else
		{
			s1.Visible = true;
			s2.Visible = true;
		}
		
	}
	private float time = 0f;

	public override void _Process(double delta)
	{
		time += (float)delta;

		s1.Position = root.Position + new Vector2(10 * Mathf.Cos(time), 15 * Mathf.Sin(time));
		s2.Position = root.Position + new Vector2(10 * Mathf.Cos(time + Mathf.Pi), 15 * Mathf.Sin(time + Mathf.Pi));
	}
}
