using Godot;
using System;

public partial class Zhukov : Node2D
{
    private Sprite2D arch;
    public override void _Ready()
    {
        arch =GetNode<Sprite2D>("Arch");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_select")){
            attack();
        }
        Position = GetGlobalMousePosition() - 100 * Vector2.Up;
        
        var angle = GetAngleTo(GetGlobalMousePosition());
        arch.Position = Position + 100 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
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
