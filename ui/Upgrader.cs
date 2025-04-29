using Godot;
using System;

public partial class Upgrader : Control
{
    Button upgrade1, upgrade2;
    public int type= 1;
    Vector2 campos = new Vector2(960, 540);
    Camera2D me;
    bool[] an = {false, false};
    bool obratno = false, exit = false;
    Timer timer;


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
        me = GetNode<Camera2D>("Camera2D");
        upgrade1 = GetNode<Button>("grade1");
        upgrade2 = GetNode<Button>("grade2");
        upgrade1.Pressed += upress;
        upgrade2.Pressed += downpress;
        timer = GetNode<Timer>("Timer");
        timer.Timeout += () =>{
            an = [false, false];
            if (!an[0] & !an[1] & obratno){
                me.Position = campos;
            }
            timer.Stop();
        };
    }

    public override void _Process(double delta)
    {
        if (an[1]){
                var abl = me.GetAngleTo(upgrade1.Position);
                me.Position +=10* new Vector2(Mathf.Cos(abl), Mathf.Sin(abl));
        }
        if (an[0]){
            var abl = me.GetAngleTo(upgrade2.Position);
                me.Position +=12* new Vector2(Mathf.Cos(abl), Mathf.Sin(abl));
        }
        if (!an[0] & !an[1] & obratno & me.Position!=campos){
            var abl = me.GetAngleTo(campos);
            me.Position += 10* new Vector2(Mathf.Cos(abl), Mathf.Sin(abl));
        }
        if (Input.IsActionJustPressed("esc")){
            timer.Start();
            obratno = true;
            an = [false, false];
            foreach (Node a in upgrade1.GetChildren()){
                a.QueueFree();
            }
            foreach (Node b in upgrade2.GetChildren()){
                b.QueueFree();
            }
        }
    }

    public void enterWibor(){

    }

    public void upress(){
        foreach (Node a in upgrade2.GetChildren()){
                a.QueueFree();
            }
        timer.Start();
        obratno = false;
        an[0] = false;
        an[1] = true;
        for(int i = 0; i < 4; i++){
            Button alala = GD.Load<PackedScene>("res://player/card.tscn").Instantiate<Button>();
            alala.Text = blocks[type,i];
            alala.Position = new Vector2(160, -320 + 200* i);
            upgrade1.AddChild(alala);
        }
    }
    public void downpress(){
        foreach (Node a in upgrade1.GetChildren()){
                a.QueueFree();
            }
        timer.Start();
        obratno = false;
        an[0] = true;
        an[1] = false;
        for(int i = 4; i < 8; i++){
            Button alala = GD.Load<PackedScene>("res://player/card.tscn").Instantiate<Button>();
            alala.Text = blocks[type,i];
            alala.Position = new Vector2(160, -320 + 200* (i-4));
            upgrade2.AddChild(alala);
        }
    }
}
