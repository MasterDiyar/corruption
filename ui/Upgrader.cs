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
    private TextureRect weapin;


    string[,] blocks={
        {"","","","","","","",""},
        {"default", "bullet speed + 200\ndamage + 1\nclip count - 4",
         "bullet speed - 150\ndamage - 0.5\npierce + 1",
         "clip count - 10\n damage +2\npierce + 1 disperce + 5", 
         
         "default", "damage x0.5\npierce + 4\nchance to not waste = 90%",
         "damage x2\nbullet speed + 400\n disperce + 10",
         "disperce = 0\n chance to not waste = 50%\n pierce + 1"},//avtomat

        {"default", "bullet + 1\nrazbros + 0.12f \ndamage - 0.5",
         "bullet - 1 \nrazbros - 0.07f dispersion -10% \ndamage + 1.5",
         "bullet + 3\ndispersion- 30% \ndamage -1",

         "default", "upgrades to obrez\ndispersion + 40% \ndamage & bullet x2",
         "pierce + 2\nbullet - 1 speed + 400\nrazbros -0.1f",
         "pierce + 1\ndamage + 2\n "},//drobash
        
        {"default","","","","default","","",""},//knife
        {"default","","","","default","","",""},//pistol
        {"default","","","","default","","",""}//rifle
        
        
    };

    private Rect2[,] offset = {
        {Ee(0, 0, 0, 0)}
        
    }; 
    static Rect2 Ee(float a, float b, float c, float d){return new Rect2(a,b,c,d);}
   
    
    public override void _Ready()
    {
        me = GetNode<Camera2D>("Camera2D");
        upgrade1 = GetNode<Button>("grade1");
        upgrade2 = GetNode<Button>("grade2");
        weapin = GetNode<TextureRect>("weapon");
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
        switch (type)
        {
            case 1:
                weapin.Texture = GD.Load<Texture2D>("res://enemy/ak47.png");
                break;
            case 2:
                weapin.Texture = GD.Load<Texture2D>("res://enemy/shotgun.png");
                break;
            case 3:
                weapin.Texture = GD.Load<Texture2D>("res://enemy/knife/Knife e1.png");
                break;
            case 4:
                weapin.Texture = GD.Load<Texture2D>("res://enemy/rifle.png");
                break;
            case 5:
                weapin.Texture = GD.Load<Texture2D>("res://enemy/pistol.png");
                break;
        }
    }

    public override void _Process(double delta)
    {
        if (an[1]){
                var abl = me.GetAngleTo(upgrade1.GlobalPosition);
                me.Position +=10* new Vector2(Mathf.Cos(abl), Mathf.Sin(abl));
        }
        if (an[0]){
            var abl = me.GetAngleTo(upgrade2.GlobalPosition);
                me.Position +=12* new Vector2(Mathf.Cos(abl), Mathf.Sin(abl));
        }
        if (!an[0] & !an[1] & obratno & me.Position!=campos){
            var abl = me.GetAngleTo(GlobalPosition + campos);
            me.Position += 10* new Vector2(Mathf.Cos(abl), Mathf.Sin(abl));
        }

        if (Input.IsActionJustPressed("esc") && obratno)
        {
            
            GetParent().GetNode<CharacterBody2D>("Zhukov").AddChild(
                GD.Load<PackedScene>("res://player/grader.tscn").Instantiate()
                );
            if (GetParent().GetNode("Zhukov") is Zhukov zhukov)
            {
                zhukov.ProcessMode = ProcessModeEnum.Inherit;
                zhukov.camera.Enabled = true;
            }
            GetParent().RemoveChild(this);
            return;
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
    

    public void upress(){
        foreach (Node a in upgrade2.GetChildren()){
                a.QueueFree();
            }
        timer.Start();
        obratno = false;
        an[0] = false;
        an[1] = true;
        for(int i = 0; i < 4; i++){
            int capture = i;
            Button alala = GD.Load<PackedScene>("res://player/card.tscn").Instantiate<Button>();
            alala.Text = blocks[type,i];
            alala.Position = new Vector2(160, -320 + 200* i);
            upgrade1.AddChild(alala);
            alala.Pressed += () =>
            {
                if (GetParent() is Main m) m.upgrades[type-1, 0] = capture;
            };
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
            int capture = i;
            Button alala = GD.Load<PackedScene>("res://player/card.tscn").Instantiate<Button>();
            alala.Text = blocks[type,i];
            alala.Position = new Vector2(160, -320 + 200* (i-4));
            upgrade2.AddChild(alala);
            alala.Pressed += () =>
            {
                if (GetParent() is Main m)
                {
                    m.upgrades[type - 1, 1] = capture;
                }
            };
        }
    }
}
