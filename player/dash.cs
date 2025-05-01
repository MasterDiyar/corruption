using Godot;
using System;

public partial class dash : Node
{
	[Export] private CharacterBody2D player;
	
	private int Speed = 400;
	private int RunSpeed = 600;
	public int currentSpeed;
	[Export] private Camera2D camera;
	
	private bool isDashing = false;
	private float dashSpeed = 1200f;
	private float dashDuration = 0.15f; 
	private float dashCooldown = 1.0f; 
	private float dashTimeLeft = 0f;
	public float dashCooldownLeft = 0f;
	private Vector2 dashDirection = Vector2.Zero;
	public override void _Ready()
	{
		currentSpeed = Speed;
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
	}
	
	public void walk(double delta){
		Vector2 velocity = Vector2.Zero;

		if (Input.IsActionPressed("right") && player.Position.X + 960 <= camera.LimitRight)
			velocity.X += 1;
		if (Input.IsActionPressed("left")  && player.Position.X - 960 >= camera.LimitLeft)
			velocity.X -= 1;
		if (Input.IsActionPressed("down")  && player.Position.Y + 540 <= camera.LimitBottom)
			velocity.Y += 1;
		if (Input.IsActionPressed("up")  && player.Position.Y - 540 >= camera.LimitTop)
			velocity.Y -= 1;
		if (player.GetNode("attack") is Attack a){
			if (!a.isReloading && Input.IsActionPressed("running"))
				currentSpeed = RunSpeed;
			else currentSpeed = Speed;
		}
		
		player.Velocity = velocity.Normalized() * currentSpeed;
		//Position += velocity * (float)delta;
		player.MoveAndSlide();
		
	}
	
	private void StartDash()
	{
		isDashing = true;
		dashTimeLeft = dashDuration;
		dashCooldownLeft = dashCooldown;

		dashDirection = (player.GetGlobalMousePosition() - player.Position).Normalized();
	}

	private void DashMovement(double delta)
	{
		player.Velocity = dashDirection * dashSpeed;
		player.MoveAndSlide();

		dashTimeLeft -= (float)delta;
		if (dashTimeLeft <= 0)
		{
			isDashing = false;
			player.Velocity = Vector2.Zero;
		}
	}
}
