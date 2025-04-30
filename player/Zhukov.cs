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
    public bool dialogue = true;
    private TextureRect inv;
    public override void _Ready()
    {
        dash = GetNode("dash");
        camera = GetNode<Camera2D>("Camera2D");
        attac = GetNode("attack");
        taker = GetNode<Area2D>("takeitem");
        taker.AreaEntered += lol;
        icon = (AnimatedSprite2D)GetNode("Icon");
        icon.Play("idle");
        arch = GetNode<Sprite2D>("Arch");
        taka = attac as Attack;

        inv = GetNode<TextureRect>("Interface/inv");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("run") && arch.Visible)
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
            tvar.Position = new Vector2(40000, 10000);
            if (tvar is Upgrader re)
            {
                re.type = taka.inventory[taka.currentInv];
            }
            GetParent().AddChild(tvar);
            camera.Enabled =false;
            ProcessMode = ProcessModeEnum.Disabled;
        }

        if (dialogue)
        {
            arch.Visible = false;
            inv.Visible = false;
            icon.GetNode<Sprite2D>("weapon").Visible = false;
            icon.GetNode<AnimatedSprite2D>("knife").Visible = false;
            dash.ProcessMode = ProcessModeEnum.Disabled;
            attac.ProcessMode = ProcessModeEnum.Disabled;
        }
        else {
            arch.Visible = true;
            inv.Visible = true;
            icon.GetNode<Sprite2D>("weapon").Visible = true;
            icon.GetNode<AnimatedSprite2D>("knife").Visible = true;
            dash.ProcessMode = ProcessModeEnum.Inherit;
            attac.ProcessMode = ProcessModeEnum.Inherit;
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
            case "heal":
                
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
