using Godot;
using System;

public partial class svinets : Sprite2D
{
    public int Speed = 900;
    public override void _Ready()
    {
        var timer = GetNode<Timer>("Timer");
        timer.Timeout += _OnTimeout;
    }

    public override void _Process(double delta)
    {
        Position += Transform.X * Speed * (float)delta;
    }

    private void _OnTimeout()
    {
        GD.Print("Bullet destroyed!"); 
        QueueFree(); 
    }
}
