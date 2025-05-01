using Godot;
using System;

public partial class Firstmap : Node2D
{
	private Timer timer;
	public bool siege;
	private Node root;
	private int times = 0;
	private TextureRect dialogue;
	private CharacterBody2D player;
	public bool deathfade = true, _b = false, sieger = false, sectime = true, portal = false;
	
	public override void _Ready()
	{
		root = GetParent();
		player = GD.Load<PackedScene>("res://player/zhukov.tscn").Instantiate<CharacterBody2D>();
		if (player is Zhukov a) {
			a.Position = new Vector2(1350, 10);
			if (a.GetNode("attack") is Attack b)
			{
				if (root is Main c) c.inventory = b.inventory;
			}}
		root.AddChild(player);
		
		player = root.GetNode<CharacterBody2D>("Zhukov");
		timer = GetNode<Timer>("Timer");
		timer.Timeout += timeout;
		
		dialogue = GD.Load<PackedScene>("res://ui/textpanel.tscn").Instantiate<TextureRect>();
		
	}
	

	public override void _Process(double delta)
	{
	}

	public void spawnstart()
	{
		_b = true;
		for (int i = 0; i < 11; i++)
		{
			RigidBody2D mob = GD.Load<PackedScene>("res://enemy/infant.tscn").Instantiate<RigidBody2D>();
			Node amn = GD.Load<PackedScene>("res://player/ak_47.tscn").Instantiate();
			mob.Position = 400 * new Vector2(Mathf.Cos(Mathf.Pi + Mathf.Pi * i / 10),
				Mathf.Sin(Mathf.Pi + Mathf.Pi * i / 10)) + new Vector2(1440, 900);
			mob.AddChild(amn);
			GetParent().AddChild(mob);
		}
	}
	public void timeout()
	{
		times++;
		if (times == 1)
		{
			/*
			if (dialogue is Textpanel nel)
			{
				
			}*/
			
			if (player is Zhukov lox)
			{
				lox.dialogue = true;
			}
			player.AddChild(dialogue);
			var death = GD.Load<PackedScene>("res://enemy/death.tscn").Instantiate<RigidBody2D>();
			death.GetNode<AnimatedSprite2D>("Icon").Play();
			death.Position = new Vector2(1460, 0);
			if (death is Death asa)
			{
				asa.npc = false;
			}
			root.AddChild(death);
		}
		if (player is Zhukov lokx)
		{
			if ( !lokx.dialogue && times != 0 && deathfade)
			{
				deathfade = false;
				var shade = GD.Load<PackedScene>("res://enemy/shade.tscn").Instantiate<Sprite2D>();
				shade.Position = new Vector2(1460, 0);
				root.AddChild(shade);
				root.GetNode<RigidBody2D>("death").QueueFree();
			}

			if (lokx.GetNode("attack") is Attack das)
			{
				if (das.hp <= 0)
				{
					timer.Stop();
				}
			}
		}
		if (!deathfade & !_b)spawnstart();
		if (sectime && sieger)siehe();
		
		

		if (portal)
		{
			dialogue = GD.Load<PackedScene>("res://ui/textpanel.tscn").Instantiate<TextureRect>();
			bool dasdsad = false;
			if (dialogue is Textpanel nel)
			{
				if (root is Main c)
				{
					if (c.soul >= 25)
					{
						dasdsad = false;
						nel.texts =
						[
							"Well well well...",
							"You can pass to next level"
						];
					}
					else
					{
						dasdsad = true;
						nel.texts = ["MUHAHAHAHAHAHA", "Now your soul will be MINE"];
					}
				}
					
			}
			
			if (player is Zhukov lox)
			{
				lox.dialogue = true;
			}
			player.AddChild(dialogue);
			var death = GD.Load<PackedScene>("res://enemy/death.tscn").Instantiate<RigidBody2D>();
			death.GetNode<AnimatedSprite2D>("Icon").Play();
			death.Position = new Vector2(-2850, -1650);
			if (death is Death asa)
			{
				asa.npc = dasdsad;
			}
			root.AddChild(death);

			foreach (var nodes in root.GetChildren())
			{
				nodes.QueueFree();
			}
			root.AddChild(GD.Load<PackedScene>("res://map/secondmap.tscn").Instantiate());
			
		}
	}

	public void siehe()
	{
		sectime = false;
		Vector2 bunkerpos = new Vector2(-300, 1600);
		for (int i = 0; i < 21; i++)
		{
			RigidBody2D mob = GD.Load<PackedScene>("res://enemy/infant.tscn").Instantiate<RigidBody2D>();
			Node amn = GD.Load<PackedScene>("res://player/ak_47.tscn").Instantiate();
			mob.Position = 1100 * new Vector2(Mathf.Cos(Mathf.Pi + Mathf.Pi * i / 10),
				Mathf.Sin(Mathf.Pi + Mathf.Pi * i / 10)) + bunkerpos;
			mob.AddChild(amn);
			GetParent().AddChild(mob);
		}
		
	}
}
