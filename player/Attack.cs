using Godot;
using System;

public partial class Attack : Node
{
	public int clipSize = 30;
	public int totalAmmo = 90;
	public float hp = 10;
	public int currentClip;
	public bool isReloading = false;
	public int[] inventory = { 1, 2, 3 }, bulletSpeed = {3600, 3000, 0, 5000, 3200};
    public float[] damages = {3, 3, 4.5f, 10, 2.5f};
    public int tbf = 1;

	public int currentInv = 0;
	[Export]CharacterBody2D player;
    Timer reloadTimer, shootTimer;
	public override void _Ready()
	{
		currentClip = clipSize;

		reloadTimer = player.GetNode<Timer>("ReloadTimer");
		reloadTimer.Timeout += FinishReload;

		shootTimer = player.GetNode<Timer>("ShootTimer");
		shootTimer.Timeout += ShootAutomatically;
	}
	public override void _Process(double delta)
	{
        if (Input.IsActionPressed("attack") && shootTimer.TimeLeft <= 0)
        {
            shootTimer.Start();
        }

        if (Input.IsActionJustReleased("attack") && shootTimer.TimeLeft > 0)
        {
            shootTimer.Stop();
        }

        if (Input.IsActionJustPressed("attack"))
        {
            switch (inventory[currentInv])
            {
                case 1:attack(); break;
                case 2:shotgunAttack();break;
                case 3: knifeAttack();break;
            }
            
        }

        if (Input.IsActionJustPressed("reload"))  //reload is
        {
            StartReload();
        }

        if (Input.IsActionJustReleased("next"))
        {
            currentInv += (currentInv == 2) ? -2 : 1;
        }
	}

    public void shotgunAttack(int bullet_count = 4, float angle = 0.15f)
    {
        for (int i = 0; i < bullet_count; i++) {
            var sharp = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Area2D>();
            sharp.Position = player.Position;
            sharp.Rotation = sharp.GetAngleTo(player.GetGlobalMousePosition()) + i * angle - bullet_count/2f * angle;
            
                if (sharp is svinets a){
                    a.Speed = bulletSpeed[1];
                    a.damage = damages[1];
                    a.timesbefore = tbf;}
            GetParent().GetParent().AddChild(sharp);
        }
    }

    public void knifeAttack()
    {
        var knife = GD.Load<PackedScene>("res://player/Knife.tscn").Instantiate<Area2D>();
        knife.GlobalPosition = player.GlobalPosition;
        knife.Rotation = player.GetAngleTo(player.GetGlobalMousePosition());
        GetParent().GetParent().AddChild(knife);
    }
	
	public void attack()
        {
            if (isReloading)
                return;
    
            if (currentClip > 0)
            {
                currentClip--;
                var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate() as Node2D;
                bullet.Position = player.Position;
                bullet.LookAt(player.GetGlobalMousePosition());
    
                if (bullet is svinets svin){
                    svin.Speed = bulletSpeed[0];
                    svin.damage = damages[0];
                    svin.timesbefore = tbf;
                }
                GetParent().GetParent().AddChild(bullet);
            }
            else
                StartReload();
            
        }
    
        public void StartReload()
        {
            if (isReloading)
                return;
    
            if (currentClip == clipSize || totalAmmo <= 0)
                return;
            
            isReloading = true;
            reloadTimer.Start();
        }
    
        private void FinishReload()
        {
            if (totalAmmo > 0)
            {
                int neededAmmo = clipSize - currentClip;
                int reloadAmount = Mathf.Min(neededAmmo, totalAmmo);
    
                currentClip += reloadAmount;
                totalAmmo -= reloadAmount;
            }
    
            isReloading = false;
        }
    
        private void ShootAutomatically()
        {
            if (Input.IsActionPressed("attack"))
                switch (inventory[currentInv])
                {
                    case 1:attack(); break;
                    case 2:shotgunAttack();break;
                    case 3: knifeAttack();break;
                }
        }
}
