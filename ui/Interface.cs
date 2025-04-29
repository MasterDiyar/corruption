using Godot;
using System;

public partial class Interface : Control
{
	public Label info;
	private CharacterBody2D root;
	private TextureRect inv, item;
	private Node attack;
	private AnimatedSprite2D sprite;
	private AnimatedSprite2D knife;
	private Sprite2D holditem;
	public override void _Ready() {
		inv = (TextureRect)GetNode("inv");
		item = (TextureRect)inv.GetNode("item");
		root = GetParent<CharacterBody2D>();
		info = GetNode<Label>("Gp");
		attack = root.GetNode("attack");
		sprite = GetNode<AnimatedSprite2D>("boot");
		holditem = root.GetNode("Icon").GetNode<Sprite2D>("weapon");
		knife = root.GetNode("Icon").GetNode<AnimatedSprite2D>("knife");
        
        knife.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (attack is Attack a)
		{
			info.Text = $" HP: {Mathf.RoundToInt(a.hp)}\n   x{a.totalAmmo/a.clipSize}\n   x{a.currentClip}";
			switch (a.inventory[a.currentInv])
			{
				case 1:
					knife.Visible = false;
					if (a.currentClip == 0) {
            			item.Texture = GD.Load<Texture2D>("res://enemy/ak47bez.png");
            			holditem.Texture = item.Texture;
        			}
        			else {
            			item.Texture = GD.Load<Texture2D>("res://enemy/ak47.png");
            			holditem.Texture = item.Texture;
        			}
        			holditem.Visible = true;
        			break;

    			case 2:
        			knife.Visible = false;
        			item.Texture = GD.Load<Texture2D>("res://enemy/shotgun.png");
        			holditem.Texture = item.Texture;
        			holditem.Visible = true;
        			break;

    			case 3:
        			holditem.Visible = false;
        			knife.Visible = true;
					item.Texture = GD.Load<Texture2D>("res://enemy/knife/Knife e1.png");
        			break;
			}
			holditem.LookAt(holditem.GetGlobalMousePosition());
			if (holditem.GetGlobalMousePosition().X < holditem.Position.X) {holditem.FlipV = true;}
			else holditem.FlipV = false;
		}

		if (root.GetNode("dash") is dash b)
		{
			if (b.dashCooldownLeft > 0)
			{
				sprite.Play();
			}
			else
			{
				sprite.Stop();
				sprite.Frame = 5;
			}
		}
	}
}
