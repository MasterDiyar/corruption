using Godot;
using System;

public partial class Upgrader : Control
{
    Button upgrade1, upgrade2;
    public int type= 1;

    string[,] blocks={
        {"", "bullet speed + 200\ndamage + 1\nblast count - 4",
         "bullet speed - 150\ndamage - 1\nblast count + 5",
         "bullet count - 10\n damage +2", 
         "", "",
         "",
         ""},

        {"", "bullet + 1\nrazbros + 0.12f \ndamage - 0.5",
         "bullet - 1 \nrazbros - 0.07f \ndamage + 1",
         "bullet + 3\nrazbros - 0.3f \ndamage -1",

         "", "recharge - 0.15 blast count - 2",
         "recharge + 1 blast count + 10",
         "damage +1 blast count - 2"}
    };
    public override void _Ready()
    {
        upgrade1 = GetNode<Button>("grade1");
        upgrade2 = GetNode<Button>("grade2");
        upgrade1.Pressed += upress;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("esc")){
            foreach (Node a in upgrade1.GetChildren()){
                a.QueueFree();
            }
        }
    }

    public void enterWibor(){

    }

    public void upress(){
        for(int i = 0; i < 4; i++){
            Button alala = GD.Load<PackedScene>("res://player/card.tscn").Instantiate<Button>();
            alala.Text = blocks[type,i];
            alala.Position = new Vector2(160, -320 + 200* i);
            upgrade1.AddChild(alala);
        }
    }
}
