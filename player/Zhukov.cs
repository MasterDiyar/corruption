using Godot;
using System;

public partial class Zhukov : Node2D
{
    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_select")){
            attack();
        }
        Position = GetGlobalMousePosition();
    }

    public void walk(){

    }

    public void attack(){
        var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate() as Node2D;
        bullet.Position = Position;
        bullet.Rotation = GetAngleTo(GetGlobalMousePosition());
        bullet.GetNode<Timer>("Timer").WaitTime = 1;
        bullet.GetNode<Timer>("Timer").Timeout += () => {QueueFree();};
        GetParent().AddChild(bullet);

    }
}
