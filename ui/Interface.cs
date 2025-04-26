using Godot;
using System;

public partial class Interface : Control
{
	public Label info;
	private CharacterBody2D root;
	private TextureRect inv, item;
	public override void _Ready()
	{
		inv = (TextureRect)GetNode("inv");
		item = (TextureRect)inv.GetNode("item");
		root = GetParent<CharacterBody2D>();
		info = GetNode<Label>("Gp");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (root is Zhukov a)
		{
			info.Text = $" HP: {Mathf.RoundToInt(a.hp)}\n   x{a.totalAmmo/a.clipSize}\n   x{a.currentClip}";
			switch (a.inventory[a.currentInv])
			{
				case 1:
					if (a.currentClip == 0)item.Texture = GD.Load<Texture2D>("res://enemy/ak47bez.png");
					else item.Texture = GD.Load<Texture2D>("res://enemy/ak47.png");
					break;
			}
		}
		
	}
}
