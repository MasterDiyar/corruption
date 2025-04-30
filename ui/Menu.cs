using Godot;
using System;

public partial class Menu : Control
{
	Button playButton, quitButton;
	AnimatedSprite2D pl;
	private bool n, m;
	private ColorRect r1, r2, r3, r4;
	Timer timer;
	[Export]private float s = 12f;
	private int times = 2;
	

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		r1 = GetNode<ColorRect>("ColorRect");
		r2 = GetNode<ColorRect>("ColorRect2");
		r3 = GetNode<ColorRect>("ColorRect3");
		r4 = GetNode<ColorRect>("ColorRect4");
		pl = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		playButton = GetNode<Button>("Button");
		quitButton = GetNode<Button>("Button2");
		playButton.Pressed += strt;
		quitButton.Pressed += (() => { GetTree().Quit(); });
		timer.Timeout += readr;
		pl.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (n)
		{
			pl.Position += new Vector2(20, 0);
			if (pl.Position.X >= 1770)
			{
				pl.Position = new Vector2(1770, 850);
				n = false;
				pl.Rotation = Mathf.Pi/2f;
				timer.Start();
			} 
		}

		if (m)
		{
			
			r1.Position += new Vector2(-s, s);
			r2.Position += 0.2f * new Vector2(-s, -s);
			r4.Position += 1.3f * new Vector2(s, -s);
			r3.Position += 1.7f * new Vector2(s, s);
		}
	}

	public void strt()
	{
		
		pl.Animation="default";
		playButton.Disabled = true;
		quitButton.Disabled = true;
		n = true;
	}

	public void readr()
	{
		if (times == 2)
		{
			m = true;
			times--;
			timer.WaitTime = 0.2f;
		}else if (times == -1)
		{
					Node player = GD.Load<PackedScene>("res://player/zhukov.tscn").Instantiate();
					if (player is Zhukov a) {
						a.Position = new Vector2(1370, 0);
						if (a.GetNode("attack") is Attack b)
						{
							if (GetParent() is Main c) c.inventory = b.inventory;
						}}
					
         			GetParent().AddChild(GD.Load<PackedScene>("res://map/firstmap.tscn").Instantiate());
         			GetParent().AddChild(player);
         			QueueFree();
         		}
		else times --;
		
	}
	
}
