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
	public override void _Ready()
	{
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyMove, Callable.From(OnMove));
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyChangeDirection, Callable.From(OnChangeDirection));

		animatedSprite2D = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		bulletSpawnPoint = GetNode<Marker2D>("%BulletSpawn");
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = new Vector2(Position.X + 1, Position.Y);
	}

	private void OnMove()
	{
		var targetPosition = tilemap.MapToLocal(tilemap.LocalToMap(new Vector2(GlobalPosition.X + (movingRight ? 32 : -32), GlobalPosition.Y)));
		Position = targetPosition;

		if (animatedSprite2D.Animation == "first")
		{
			animatedSprite2D.Play("second");
		}
		else
		{
			animatedSprite2D.Play("first");
		}
	}

	private void OnChangeDirection()
	{
		movingRight = !movingRight;
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
