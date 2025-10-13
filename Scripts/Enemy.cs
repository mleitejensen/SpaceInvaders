using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public PackedScene EnemyBulletScene;
	public Marker2D BulletSpawnPoint;
	public AnimatedSprite2D AnimatedSprite2D;
	public Timer Timer;
	public TileMapLayer Tilemap;
	private bool _movingRight = true;
	private float _baseMovementSpeed = 10;
	private float _movementSpeed;
	private int _touchedCorner = 0;
	public override void _Ready()
	{
		_movementSpeed = _baseMovementSpeed;

		SignalBus.Instance.Connect(SignalBus.SignalName.EnemySpeedChange, Callable.From<int>(OnSpeedChange));
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyChangeDirection, Callable.From(OnChangeDirection));

		AnimatedSprite2D = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		Tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		BulletSpawnPoint = GetNode<Marker2D>("%BulletSpawn");
	}

	public override void _Process(double delta)
	{
		Velocity = new Vector2(_baseMovementSpeed, 0) * (_movingRight ? _movementSpeed : -_movementSpeed);

		MoveAndSlide();
	}

	private void OnChangeDirection()
	{
		_movingRight = !_movingRight;
		_touchedCorner++;
		if (_touchedCorner >= 2)
		{
			MoveDown();
		}
	}

	private void OnSpeedChange(int multiplySpeedBy)
	{
		_movementSpeed = _baseMovementSpeed * multiplySpeedBy;
	}

	private void MoveDown()
	{

	}

	public void Die()
	{
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScoreUpdate, 20);
		QueueFree();

	}

	public void Shoot()
	{
		var bullet = EnemyBulletScene.Instantiate<Node2D>();
		bullet.GlobalPosition = BulletSpawnPoint.GlobalPosition;
		GetNode("/root/Game").AddChild(bullet);
	}
}
