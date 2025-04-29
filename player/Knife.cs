using Godot;
using System;

public partial class Knife : Area2D
{
    public float damage = 4;
    public string team = "enemies";
    private Timer knifeTimer;

    public override void _Ready()
    {
        knifeTimer = GetNode<Timer>("Timer");
        GetNode<AnimatedSprite2D>("lox").Play();
        knifeTimer.Timeout += OnTimerTimeout;
        knifeTimer.Start();
        BodyEntered += _OnEnter;
    }

    private void _OnEnter(Node area)
    {
        
        
        if (area.IsInGroup(team) && area.HasNode("property"))
        {
            if (area.GetNode("property") is property a)
            {
                a.hp -= damage;
            }
            QueueFree();
        }

        if (area.IsInGroup(team) && area.HasNode("attack"))
        {
            if (area.GetNode("attack") is Attack a)
            {
                a.hp -= damage;
            }
            QueueFree();
        }

        
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}

