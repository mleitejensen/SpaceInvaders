using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public PackedScene BulletScene;
	public const float Speed = 300.0f;
	private bool CanShoot = true;
	private Timer timer;
	private Marker2D BulletSpawnPoint;
	private Area2D hitbox;

	public override void _Ready()
	{
		BulletSpawnPoint = GetNode<Marker2D>("%BulletSpawn");
		timer = GetNode<Timer>("%Timer");
		hitbox = GetNode<Area2D>("%Hitbox");

		timer.Timeout += OnTimerTimeout;
		hitbox.BodyEntered += OnBodyEntered;
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

	private void OnBodyEntered(Node2D body)
	{
		body.QueueFree();
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerHealthChange, 1);
	}

}
