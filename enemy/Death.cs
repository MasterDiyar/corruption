using Godot;
using System;

public partial class Death : RigidBody2D
{
	public AnimatedSprite2D Icon;
	private Sprite2D kosa;
	Timer timer, spawnTimer;
	public bool npc = true, atking = false;
	private float lela;
	CharacterBody2D player;
	private float maxhp = 400, hei, textureHeight, textureWidth;
	Line2D line;
	public float hp = 100;
	public override void _Ready()
	{
		
		line = GetNode<Line2D>("hpline");
		AnimatedSprite2D icor = GetNode<AnimatedSprite2D>("Icon");
		icor.Play("default");
		var rexture = icor.SpriteFrames.GetFrameTexture(icor.Animation, 0);
		textureHeight = rexture.GetHeight();
		textureWidth = rexture.GetWidth() * icor.Scale.X;
		hei = textureHeight / 2f + 30f;
		line.Position = new Vector2(0, -hei);
		
		player = GetParent().GetNode<CharacterBody2D>("Zhukov");
				
		if (player == null) player = GetParent().GetNode<CharacterBody2D>("Zhukov2");
		Icon = GetNode<AnimatedSprite2D>("Icon");
		kosa = GetNode<Sprite2D>("Icon/Kosa");
		timer = GetNode<Timer>("Timer");
		spawnTimer = GetNode<Timer>("spawnTimer");
		Icon.Play();
		if (npc)
		{
			timer.Start();
			spawnTimer.Start();
			timer.Timeout += Kattack;
			spawnTimer.Timeout += Spawn;
			GetNode<Area2D>("Icon/Kosa/Area2D").BodyEntered += Entered;
			ProcessMode = ProcessModeEnum.Inherit;
		}
		else
		{
			timer.Stop();
			spawnTimer.Stop();
			ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	
	public override void _Process(double delta)
	{
		line.ClearPoints();
		line.SetPoints(new Vector2[] {
			Vector2.Zero, new Vector2(textureWidth / 2 * hp / maxhp, 0)
		});
		line.Position = Position + new Vector2(-textureWidth/4, -hei);
		
		
		lela += (float)delta;
		if (atking)
		{
			kosa.Rotate(2 * lela);
			kosa.Scale = new Vector2(4, 4);
		}
		else
		{
			kosa.Scale = new Vector2(2, 2);
			kosa.Rotation = 0;
		}
		var angle = GetAngleTo(player.Position);
		Position += new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

		if (hp <= 0)
		{
			foreach (var nodes in GetParent().GetParent().GetChildren())
			{
				nodes.QueueFree();
			}
			GetParent().GetParent().AddChild(GD.Load<PackedScene>("res://ui/Menu.tscn").Instantiate());
			GetParent().GetParent().GetNode<Label>("Menu/Label").Text = "YOU WINNER";
		}
		
	}

	public void Kattack()
	{
		atking = !atking;
	}

	public void Spawn()
	{
		for (int i = 0; i < 10; i++)
		{
			RigidBody2D slime = GD.Load<PackedScene>("res://enemy/test_slime.tscn").Instantiate<RigidBody2D>();
			slime.Position =
				Position + 300 * new Vector2(Mathf.Cos(2 * i * Mathf.Pi / 10) , Mathf.Sin(2 * i * Mathf.Pi / 10));
			GetParent().AddChild(slime);
			
		}
	}

	public void Entered(Node area)
	{
		if (area.IsInGroup("player") && area.HasNode("attack")){
			if (area.GetNode("attack") is Attack a)
			{
				a.hp -= 5;
				a.totalAmmo -= 1;
			}
		}
	}
}
