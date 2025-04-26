using Godot;
using System;

public partial class Zhukov : CharacterBody2D
{
    private Sprite2D arch;
    private int Speed = 400;
    private int RunSpeed = 600;
    private int currentSpeed;
    public int rifleClipSize = 8;
    public int rifleTotalAmmo = 32;

    public int shotgunClipSize = 6;
    public int shotgunTotalAmmo = 24;
    public float shotgunFireRate = 0.8f;
    public float shotgunReloadTime = 4.0f;
    public float hp = 10;
    public int clipSize;
    public int totalAmmo;
    public int currentClip;
    private int rifleCurrentAmmo;
    private int rifleCurrentClip;
    private int shotgunCurrentAmmo;
    private int shotgunCurrentClip;
    private bool isReloading = false;
    private bool isShotgunCoolingDown = false;
    public int[] inventory = { 1, 2, 1 };
    public int currentInv = 0;

    private bool isDashing = false;
    private float dashSpeed = 1200f;
    private float dashDuration = 0.15f; 
    private float dashCooldown = 1.0f; 
    private float dashTimeLeft = 0f;
    private float dashCooldownLeft = 0f;
    private Vector2 dashDirection = Vector2.Zero;
    
    public override void _Ready()
    {
        arch = GetNode<Sprite2D>("Arch");
        currentSpeed = Speed;
        rifleCurrentAmmo = rifleTotalAmmo;
        rifleCurrentClip = rifleClipSize;

        shotgunCurrentAmmo = shotgunTotalAmmo;
        shotgunCurrentClip = shotgunClipSize;

        clipSize = rifleClipSize;
        totalAmmo = rifleCurrentAmmo;
        currentClip = rifleCurrentClip;

        var reloadTimer = GetNode<Timer>("ReloadTimer");
        reloadTimer.Timeout += FinishReload;

        var shootTimer = GetNode<Timer>("ShootTimer");
        shootTimer.Timeout += ShootAutomatically;

        var shotgunCooldownTimer = GetNode<Timer>("ShotgunCooldownTimer");
        shotgunCooldownTimer.Timeout += OnShotgunCooldownTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (dashCooldownLeft > 0)
            dashCooldownLeft -= (float)delta;

        if (isDashing)
        {
            DashMovement(delta);
        }
        else
        {
            if (Input.IsActionJustPressed("dash") && dashCooldownLeft <= 0)
                StartDash();
            walk(delta);
        }

        var shootTimer = GetNode<Timer>("ShootTimer");
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
            SwitchWeapon();
        }
    }

    public void walk(double delta){
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("right"))
            velocity.X += 1;
        if (Input.IsActionPressed("left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("up"))
            velocity.Y -= 1;

        if (!isReloading && Input.IsActionPressed("running"))
        {
            currentSpeed = RunSpeed;
        }
        else
        {
            currentSpeed = Speed;
        }

        Velocity = velocity.Normalized() * currentSpeed;
        //Position += velocity * (float)delta;
        MoveAndSlide();
        var angle = GetAngleTo(GetGlobalMousePosition());
        arch.Position = 180 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        arch.Rotation = angle + Mathf.Pi / 2;
    }

    public void attack()
    {
        if (isReloading || currentClip == 0)
    {
        if (totalAmmo > 0)
        {
            StartReload();
        }
        return;
    }

    if (inventory[currentInv] == 1) // AK-47
        {
            ShootSingleBullet();
            currentClip--;
        }
        else if (inventory[currentInv] == 2 && !isShotgunCoolingDown) // Shotgun
        {
            ShootShotgun();
            currentClip--;
            isShotgunCoolingDown = true;
            GetNode<Timer>("ShotgunCooldownTimer").Start(shotgunFireRate);
        }
    }

    private void ShootSingleBullet()
    {
        var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Node2D>();
        bullet.Position = Position;
        bullet.LookAt(GetGlobalMousePosition());

        if (bullet is svinets b)
        {
            b.Speed = 3600;
        }

        GetParent().AddChild(bullet);
    }

    private void ShootShotgun()
    {
        int pelletCount = 6; // количество дробинок
        float spreadAngle = 15f; // угол разброса дробинок

        for (int i = 0; i < pelletCount; i++)
        {
            var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate<Node2D>();
            bullet.Position = Position;

            Vector2 target = GetGlobalMousePosition();
            float baseAngle = GetAngleTo(target);
            float randomSpread = (float)GD.RandRange(-spreadAngle, spreadAngle) * Mathf.DegToRad(1);

            bullet.Rotation = baseAngle + randomSpread;

            if (bullet is svinets b)
            {
                b.Speed = 3000;
            }

            GetParent().AddChild(bullet);
        }
    }

   private void OnShotgunCooldownTimeout()
    {
        isShotgunCoolingDown = false;
    }

    public void StartReload()
    {
        if (isReloading || totalAmmo == 0 || currentClip == clipSize || clipSize == 0)
        {
            return;
        }

        isReloading = true;
        var reloadTimer = GetNode<Timer>("ReloadTimer");
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

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownLeft = dashCooldown;

        dashDirection = (GetGlobalMousePosition() - Position).Normalized();
    }

    private void DashMovement(double delta)
    {
        Velocity = dashDirection * dashSpeed;
        MoveAndSlide();

        dashTimeLeft -= (float)delta;
        if (dashTimeLeft <= 0)
        {
            isDashing = false;
            Velocity = Vector2.Zero;
        }
    }

    private void SwitchWeapon()
    {

        if (inventory[currentInv] == 1)
        {
            rifleCurrentAmmo = totalAmmo;
            rifleCurrentClip = currentClip;
        }
        else if (inventory[currentInv] == 2)
        {
            shotgunCurrentAmmo = totalAmmo;
            shotgunCurrentClip = currentClip;
        }

        currentInv = (currentInv + 1) % inventory.Length;

        if (inventory[currentInv] == 1)
        {
            clipSize = rifleClipSize;
            totalAmmo = rifleCurrentAmmo;
            currentClip = rifleCurrentClip;
        }
        else if (inventory[currentInv] == 2)
        {
            clipSize = shotgunClipSize;
            totalAmmo = shotgunCurrentAmmo;
            currentClip = shotgunCurrentClip;
        }
    }


    private void UpdateWeaponAmmo()
    {
        if (inventory[currentInv] == 1) // AK-47
        {
            rifleCurrentClip = currentClip;
            rifleTotalAmmo = totalAmmo;
        }
        else if (inventory[currentInv] == 2) // Shotgun
        {
            shotgunCurrentClip = currentClip;
            shotgunTotalAmmo = totalAmmo;
        }
    }
}
