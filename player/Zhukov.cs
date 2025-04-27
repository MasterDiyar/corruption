using Godot;
using System;

public partial class Zhukov : CharacterBody2D
{
    private Sprite2D arch;
    public float hp = 10;
    private AnimatedSprite2D icon;
    public override void _Ready()
    {
        icon = (AnimatedSprite2D)GetNode("Icon");
        icon.Play("idle");
        arch = GetNode<Sprite2D>("Arch");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("run"))
        {
            if(Input.IsActionJustPressed("left"))icon.Scale = 2.4f * new Vector2(-1, 1);
            else if(Input.IsActionJustPressed("right"))icon.Scale = 2.4f * Vector2.One;
            icon.Animation = "run";
        }
        else
        {
            icon.Animation = "idle";
        }
        
        var angle = GetAngleTo(GetGlobalMousePosition());
        arch.Position = 80 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        arch.Rotation = angle + Mathf.Pi / 2;
    }
    
}
