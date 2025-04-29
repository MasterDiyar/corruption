using Godot;
using System;

public partial class KnifeAttack : Area2D
{
    public float damage = 4;
    public string team = "enemies";
    private Timer knifeTimer;

    public override void _Ready()
    {
        knifeTimer = GetNode<Timer>("Timer");
        knifeTimer.Timeout += OnTimerTimeout;
        knifeTimer.Start();
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup(team))
        {
            if (body.HasNode("property") && body.GetNode("property") is property a)
                a.hp -= damage;

            if (body.HasNode("attack") && body.GetNode("attack") is Attack a2)
                a2.hp -= damage;
        }
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}

