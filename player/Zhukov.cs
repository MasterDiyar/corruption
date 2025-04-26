using Godot;
using System;

public partial class Zhukov : CharacterBody2D
{
    private Sprite2D arch;
    private int Speed = 400;
    private int RunSpeed = 600;
    private int currentSpeed;
    public int clipSize = 8;
    public int totalAmmo = 32;
    public float hp = 10;
    public int currentClip;
    private bool isReloading = false;
    public int[] inventory = { 1, 1, 1 };
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
        currentClip = clipSize;

        var reloadTimer = GetNode<Timer>("ReloadTimer");
        reloadTimer.Timeout += FinishReload;

        var shootTimer = GetNode<Timer>("ShootTimer");
        shootTimer.Timeout += ShootAutomatically;
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
        if (isReloading)
        {
            return;
        }

        if (currentClip > 0)
        {
            currentClip--;

            var bullet = GD.Load<PackedScene>("res://player/svinets.tscn").Instantiate() as Node2D;
            bullet.Position = Position;
            bullet.LookAt(GetGlobalMousePosition());

            if (bullet is svinets a)
            {
                a.Speed = 3600;
            }

            GetParent().AddChild(bullet);
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
}
