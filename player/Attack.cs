using Godot;
using System;

public partial class Attack : Node
{
	public int clipSize = 8;
	public int totalAmmo = 32;
	public float hp = 10;
	public int currentClip;
	public bool isReloading = false;
	public int[] inventory = { 1, 1, 1 };
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
            attack();
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
	
	public void attack()
        {
            if (isReloading)
            {
                return;
            }
    
            if (currentClip > 0)
            {
                currentClip--;
    
                var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate() as Node2D;
                bullet.Position = player.Position;
                bullet.LookAt(player.GetGlobalMousePosition());
    
                if (bullet is svinets a)
                {
                    a.Speed = 3600;
                }
    
                GetParent().GetParent().AddChild(bullet);
            }
            else
            {
                StartReload();
            }
        }
    
        public void StartReload()
        {
            if (isReloading)
            {
                return;
            }
    
            if (currentClip == clipSize || totalAmmo <= 0)
            {
                return;
            }
    
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
            {
                attack();
            }
        }
}
