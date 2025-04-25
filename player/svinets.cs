using Godot;
using System;

public partial class svinets : Node2D
{
    public int Speed = 900;
    public float damage = 2;
    
    public override void _Ready()
    {
        var timer = GetNode<Timer>("Timer");
        timer.Timeout += _OnTimeout;
        Area2D collider = GetNode<Area2D>("collider");
        collider.AreaEntered += _OnEnter;
    }

    public override void _Process(double delta)
    {
        Position += Transform.X * Speed * (float)delta;
    }

    private void _OnTimeout()
    {
        QueueFree(); 
    }

    private void _OnEnter(Area2D area)
    {
        
        
        if (area.IsInGroup("enemies") && area.HasNode("property"))
        {
            if (area.GetNode("property") is property a)
            {
                a.hp -= damage;
            }
            
        }

        
    }
}
