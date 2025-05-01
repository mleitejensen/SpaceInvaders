using Godot;
using System;

public partial class EnemyBullet : Bullet
{
	private Timer timer;
	[Export]
	private PackedScene explosion;

	public override void _Ready()
	{
		this.BodyEntered += OnBodyEntered;
		speed = -500;

		timer = GetNode<Timer>("%Timer");
		timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player2D player)
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
		var newExplosion = explosion.Instantiate<Node2D>();
		newExplosion.GlobalPosition = collisionPosition;
		GetParent().AddChild(newExplosion);
	}
}
