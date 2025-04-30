using Godot;
using System;

public partial class shade : Sprite2D
{
	private int lalka;
	private Timer timer;
	public override void _Ready()
	{
		lalka = 255;
		timer=GetNode<Timer>("Timer");
		timer.Start();
		timer.Timeout += () => {QueueFree();};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Rotate(-(float)delta);
		Position += Vector2.Up + Vector2.Right ;
		lalka -= 5;
		if (lalka >= 0)Modulate = new Color(1,1,1,lalka/255f);
	}
	
}
