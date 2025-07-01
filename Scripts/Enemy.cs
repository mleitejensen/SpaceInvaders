using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public PackedScene enemyBullet;
	public Marker2D bulletSpawnPoint;
	public AnimatedSprite2D animatedSprite2D;
	public Timer timer;
	public TileMapLayer tilemap;
	private bool movingRight = true;
	private float baseMovementSpeed = 1;
	private float movementSpeed;
	public override void _Ready()
	{
		movementSpeed = baseMovementSpeed;

		SignalBus.Instance.Connect(SignalBus.SignalName.EnemySpeedChange, Callable.From<int>(OnSpeedChange));
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyChangeDirection, Callable.From(OnChangeDirection));

		animatedSprite2D = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		bulletSpawnPoint = GetNode<Marker2D>("%BulletSpawn");
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = new Vector2(Position.X + (movingRight ? movementSpeed : -movementSpeed), Position.Y);
	}

	private void OnChangeDirection()
	{
		movingRight = !movingRight;
	}

	private void OnSpeedChange(int multiplySpeedBy)
	{
		movementSpeed = baseMovementSpeed * multiplySpeedBy;
	}

	public void Die()
	{
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScoreUpdate, 20);
		QueueFree();

	}

	public void Shoot()
	{
		enemyBullet.Instantiate();
		var bullet = enemyBullet.Instantiate<Node2D>();
		bullet.GlobalPosition = bulletSpawnPoint.GlobalPosition;
		GetNode("/root/Game").AddChild(bullet);
	}
}
