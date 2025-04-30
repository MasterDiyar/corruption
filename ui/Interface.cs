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


	public override void _Process(double delta)
	{
		if (attack is Attack a)
		{
			info.Text = $"\n\n  x{Mathf.RoundToInt(a.hp)}\n   x{a.totalAmmo/a.clipSize}\n   x{a.currentClip} from {a.totalAmmo}";
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
			    
				case 4:
					knife.Visible = false;
					item.Texture = GD.Load<Texture2D>("res://enemy/rifle.png");
					holditem.Texture = item.Texture;
					holditem.Visible = true;
					break;

				case 5:
					knife.Visible = false;
					item.Texture = GD.Load<Texture2D>("res://enemy/pistol.png");
					holditem.Texture = item.Texture;
					holditem.Visible = true;
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
