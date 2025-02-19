using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public PackedScene BulletScene;
	public const float Speed = 300.0f;
	private bool CanShoot = true;
	private Timer timer;
	Marker2D BulletSpawnPoint;

	public override void _Ready()
	{
		BulletSpawnPoint = GetNode<Marker2D>("%BulletSpawn");
		timer = GetNode<Timer>("%Timer");
		timer.Timeout += OnTimerTimeout;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_accept") && CanShoot)
		{
			Shoot();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		float direction = Input.GetAxis("ui_left", "ui_right");
		velocity.X = direction * Speed;

		Velocity = velocity;
		MoveAndSlide();
	}

	public void Shoot()
	{
		CanShoot = false;
		var bullet = BulletScene.Instantiate<Node2D>();
		bullet.GlobalPosition = BulletSpawnPoint.GlobalPosition;
		GetNode("/root/Game").AddChild(bullet);
		timer.Start();
	}

	private void OnTimerTimeout()
	{
		CanShoot = true;
	}

}
