using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public PackedScene BulletScene;
	public const float Speed = 200.0f;
	private bool _canShoot = true;
	private Timer _timer;
	private Marker2D _bulletSpawnPoint;
	private Area2D _hitbox;

	public override void _Ready()
	{
		_bulletSpawnPoint = GetNode<Marker2D>("%BulletSpawn");
		_timer = GetNode<Timer>("%Timer");

		_timer.Timeout += OnTimerTimeout;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_accept") && _canShoot)
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
		if (BulletScene == null)
		{
			GD.PrintErr("BulletScene is not assigned in the inspector!");
			return;
		}
		_canShoot = false;
		var bullet = BulletScene.Instantiate<Node2D>();
		bullet.GlobalPosition = _bulletSpawnPoint.GlobalPosition;
		GetNode("/root/Game").AddChild(bullet);
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		_canShoot = true;
	}

	public void TakeDamage()
	{
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.PlayerHealthChange, 1);
	}

}
