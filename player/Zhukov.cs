using Godot;
using System;

public partial class Zhukov : Node2D
{
    private Sprite2D arch;
    private int Speed = 400;
    private int RunSpeed = 600;
    private int currentSpeed;
    public override void _Ready()
    {
        arch = GetNode<Sprite2D>("Arch");
        currentSpeed = Speed;
    }

    public override void _Process(double delta)
    {
        walk(delta);


        if (Input.IsActionJustPressed("attack")){
            attack();
        }
        
    }

    public void walk(double delta){
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("right"))
            velocity.X += 1;
        if (Input.IsActionPressed("left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("up"))
            velocity.Y -= 1;

        if (Input.IsActionPressed("running"))
        {
            currentSpeed = RunSpeed;
        }
        else
        {
            currentSpeed = Speed;
        }

        velocity = velocity.Normalized() * currentSpeed;
        Position += velocity * (float)delta;
        
        var angle = GetAngleTo(GetGlobalMousePosition());
        arch.Position = 180 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        arch.Rotation = angle + Mathf.Pi / 2;
    }

    public void attack(){
        var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate() as Node2D;
        bullet.Position = Position;
        bullet.LookAt(GetGlobalMousePosition());
        if (bullet is svinets a)
        {
            a.Speed = 3600;
            
        }
        GetParent().AddChild(bullet);

    }
}
