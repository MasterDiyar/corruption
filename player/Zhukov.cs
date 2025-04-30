using Godot;
using System;

public partial class Zhukov : CharacterBody2D
{
    private Sprite2D arch;
    public float hp = 10;
    private AnimatedSprite2D icon;
    public int maxammo = 6, minammo = 1;
    Node attac, dash;
    Random random = new Random();
    public bool enter = false;
    public Camera2D camera;
    Area2D taker;
    private Attack taka;
    public override void _Ready()
    {
        camera = GetNode<Camera2D>("Camera2D");
        attac = GetNode("attack");
        taker = GetNode<Area2D>("takeitem");
        taker.AreaEntered += lol;
        icon = (AnimatedSprite2D)GetNode("Icon");
        icon.Play("idle");
        arch = GetNode<Sprite2D>("Arch");
        taka = attac as Attack;
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

        if (enter && Input.IsActionJustPressed("interract"))
        {
            var tvar = GD.Load<PackedScene>("res://player/upgrader.tscn").Instantiate<Control>();
            tvar.Position = Position;
            if (tvar is Upgrader re)
            {
                re.type = taka.inventory[taka.currentInv];
            }
            GetParent().AddChild(tvar);
            camera.Enabled =false;
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }

    void lol (Area2D a){
        if (a.HasMeta("type")) switch ((string)a.GetMeta("type")){
            case "patron":
                if (attac is Attack arc){
                    arc.totalAmmo += random.Next(minammo, maxammo);
                }
                a.QueueFree();
                
            break;
            case "table":
                enter = true;
                break;
        }
        
    }

    void nelol(Area2D a)
    {
        if (a.HasMeta("type"))
            if ((string)a.GetMeta("type") == "table")
            {
                enter = false;
            }
    }
    
}
