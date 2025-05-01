using Godot;
using System;

public partial class Secondmap : Node2D
{
	private CharacterBody2D player;
	Timer timer;
	private int times = 0; 
	Random random = new Random(); 
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		player = GD.Load<PackedScene>("res://player/zhukov.tscn").Instantiate<CharacterBody2D>();
		player.Name = "Zhukov";
		Camera2D asd = player.GetNode<Camera2D>("Camera2D");
		asd.LimitRight = 6300;
		asd.LimitBottom = 3200;
		asd.LimitTop = 0;
		asd.LimitLeft = 0;
		player.Position = new Vector2(5200, 700);
		if (GetParent() is Main ma)
		{
			ma.inventory = [3, 1, 2];
		}
		if (player is Zhukov lox)
		{
			lox.dialogue = false;
		}

		if (player.GetNode("attack") is Attack sa) sa.inventory = [3, 1, 2];
		GetParent().AddChild(player);
		timer.Start();
		timer.Timeout += Lel;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void Lel()
	{
		times++;

		if (times >= 1 && times <= 4){
			for (int i = 0; i < 11; i++)
			{
				RigidBody2D mob = GD.Load<PackedScene>("res://enemy/infant.tscn").Instantiate<RigidBody2D>();
				Node amn = GD.Load<PackedScene>("res://player/ak_47.tscn").Instantiate();
				if (mob is Infant da)
				{
					da.typ = (random.Next(1) == 1) ? "skel" : "eye";
					da.Position = player.Position + new Vector2(800 * Mathf.Cos(i /10f+ Mathf.Pi/2), 800 * Mathf.Sin(i /10f * Mathf.Pi/2));
					
					if (da.GetNode("property") is property la)
					{
						la.hp = 15;
					}
				}

				mob.AddChild(amn);
				GetParent().AddChild(mob);
			}
				
			
		}
		else if (times == 5)
		{
			var death = GD.Load<PackedScene>("res://enemy/death.tscn").Instantiate<RigidBody2D>();
			death.GetNode<AnimatedSprite2D>("Icon").Play();
			death.Position = new Vector2(-2850, -1650);
			if (death is Death asa)
			{
				asa.npc = true;
			}
			GetParent().AddChild(death);
		}
	}
}
