using Godot;
using System;

public partial class Attack : Node
{
	public int clipSize = 30;
    public int totalAmmo = 90, dullcount = 4;
	public float hp = 8000;
	public int currentClip;
	public bool isReloading = false;
     
	public int[] inventory = { 3, 5, 2 }, bulletSpeed = {3600, 3000, 0, 5000, 3200}, dispersion = [5, 45, 20];
    public float[] damages = {3, 4, 49.5f, 10, 2.5f};
    public int[] tbf = {1, 1, 1, 1};
    public bool obrez = false;
    
    public float  angle = 0.15f;
    public float kniferadius = 70;
	public int currentInv = 0;
    private double lastShotTime = 0;
    public float shotgunCooldown = 0.6f;
    public float rifleCooldown = 0.8f;
    public float pistolCooldown = 0.3f;
    public float defaultCooldown = 0.25f;
    public int[] Unbreaking = [100, 50, 90];
    
    public bool dead = false;
	[Export]CharacterBody2D player;
    Timer reloadTimer, shootTimer;
    Random random = new Random();
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
                case 2:shotgunAttack(dullcount, angle);break;
                case 3:knifeAttack();break;
                case 4:rifleAttack();break;
                case 5:pistolAttack();break;
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

        if (hp <= 0 && dead)
        {
            foreach (var nodes in GetParent().GetParent().GetChildren())
            {
                 nodes.QueueFree();
            }
            GetParent().GetParent().AddChild(GD.Load<PackedScene>("res://ui/Menu.tscn").Instantiate());
            
        }
        
        if (hp <= 0 && !dead )
        {
            dead = true;
            foreach (var nodes in GetParent().GetParent().GetChildren())
            {
                if (nodes.IsInGroup("enemies")) nodes.QueueFree();
            }

            GetParent().GetParent().GetChild(0).GetNode<Timer>("Timer").Stop();
            hp = 120;
            var death = GD.Load<PackedScene>("res://enemy/death.tscn").Instantiate<RigidBody2D>();
            death.Position = player.GlobalPosition + new Vector2(150, 0);
            if (death is Death deathg) deathg.hp = 900;
            GetParent().GetParent().AddChild(death);
        }
        
	}

    public void shotgunAttack(int bullet_count = 4, float angle = 0.15f)
    {
        if (isReloading || Time.GetTicksMsec() - lastShotTime < shotgunCooldown * 1000)
            return;

        lastShotTime = Time.GetTicksMsec();
        if (currentClip > 0)
        {
            currentClip -= (obrez) ? 2 * bullet_count : bullet_count;
            for (int i = 0; i < bullet_count; i++)
            {
                var sharp = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Area2D>();
                sharp.Position = player.Position + 50 * new Vector2(Mathf.Cos(sharp.Rotation), Mathf.Sin(sharp.Rotation));
                sharp.Rotation = sharp.GetAngleTo(player.GetGlobalMousePosition()) + i * angle -
                                 bullet_count / 2f * angle + random.Next(-dispersion[1], dispersion[1])/90f;
                if (sharp is svinets a)
                {
                    a.Speed = bulletSpeed[1];
                    a.damage = damages[1];
                    a.timesbefore = tbf[1];
                }

                GetParent().GetParent().AddChild(sharp);
            }
        }
        else StartReload();
    }

    public void knifeAttack()
    {
        var knife = GD.Load<PackedScene>("res://player/Knife.tscn").Instantiate<Area2D>();
        knife.Rotation = player.GetAngleTo(player.GetGlobalMousePosition());
        knife.Position =kniferadius * new Vector2(Mathf.Cos(knife.Rotation), Mathf.Sin(knife.Rotation));
        if (knife is Knife life)
        {
            life.damage = damages[2];
        }
        GetParent().AddChild(knife);
    }

    public void pistolAttack()
    {
        if (isReloading || Time.GetTicksMsec() - lastShotTime < pistolCooldown * 1000)
            return;

        lastShotTime = Time.GetTicksMsec();

        if (currentClip > 0)
        {
            currentClip-= (random.Next(100) < Unbreaking[1]) ? 1: 0;
            var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Area2D>();
            bullet.Rotation = player.GetAngleTo(player.GetGlobalMousePosition());
            bullet.Position = player.Position + 50 * new Vector2(Mathf.Cos(bullet.Rotation), Mathf.Sin(bullet.Rotation));

            if (bullet is svinets svin)
            {
                svin.Speed = bulletSpeed[4];
                svin.damage = damages[4];
                svin.timesbefore = tbf[3];
            }

            GetParent().GetParent().AddChild(bullet);
        }
        else StartReload();
    }

    public void rifleAttack()
    {
        if (isReloading || Time.GetTicksMsec() - lastShotTime < rifleCooldown * 1000)
            return;

        lastShotTime = Time.GetTicksMsec();

        if (currentClip > 0)
        {
            currentClip-= (random.Next(100) < Unbreaking[2]) ? 1: 0;
            var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Area2D>();

            bullet.Rotation = player.GetAngleTo(player.GetGlobalMousePosition());
            bullet.Position = player.Position + 50 * new Vector2(Mathf.Cos(bullet.Rotation), Mathf.Sin(bullet.Rotation));

            if (bullet is svinets svin)
            {
                svin.Speed = bulletSpeed[3];
                svin.damage = damages[3];
                svin.timesbefore = tbf[2];
            }

            GetParent().GetParent().AddChild(bullet);
        }
        else StartReload();
    }

	
	public void attack()
        {
            if (isReloading || Time.GetTicksMsec() - lastShotTime < defaultCooldown * 1000)
                return;

            lastShotTime = Time.GetTicksMsec();
    
            if (currentClip > 0)
            {
                currentClip-= (random.Next(100) < Unbreaking[0]) ? 1: 0;
                var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate() as Node2D;
                bullet.Rotation = player.GetAngleTo(player.GetGlobalMousePosition()) + random.Next(-dispersion[0], dispersion[0])/20f;
                bullet.Position = player.Position + 50 * new Vector2(Mathf.Cos(bullet.Rotation), Mathf.Sin(bullet.Rotation));
    
                if (bullet is svinets svin){
                    svin.Speed = bulletSpeed[0];
                    svin.damage = damages[0];
                    svin.timesbefore = tbf[0];
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
                    case 2:shotgunAttack(dullcount, angle);break;
                    case 3:knifeAttack();break;
                    case 4:rifleAttack();break;
                    case 5:pistolAttack();break;
                }
        }
}