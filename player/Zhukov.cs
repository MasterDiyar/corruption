using Godot;
using System;

public partial class Zhukov : CharacterBody2D
{
    private Sprite2D arch;
    public float hp = 10;
    public override void _Ready()
    {
        arch = GetNode<Sprite2D>("Arch");
    }

    public override void _PhysicsProcess(double delta)
    {
        var angle = GetAngleTo(GetGlobalMousePosition());
        arch.Position = 180 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        arch.Rotation = angle + Mathf.Pi / 2;
    }
    
}
