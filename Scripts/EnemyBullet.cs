using Godot;
using System;

public partial class EnemyBullet : Bullet
{
	private Timer _timer;
	[Export]
	private PackedScene _explosion;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		Speed = -500;

		_timer = GetNode<Timer>("%Timer");
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.TakeDamage();
		}

		Vector2 collisionPosition = GlobalPosition;

		CallDeferred(nameof(HandleBodyEntered), collisionPosition);

		QueueFree();
	}

	private void HandleBodyEntered(Vector2 collisionPosition)
	{
		SetDeferred("monitoring", false);
		var newExplosion = _explosion.Instantiate<Node2D>();
		newExplosion.GlobalPosition = collisionPosition;
		GetParent().AddChild(newExplosion);
	}
}
