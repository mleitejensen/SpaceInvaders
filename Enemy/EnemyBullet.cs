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
		if (body is Player player)
		{
			player.TakeDamage();
		}

		CallDeferred(nameof(HandleBodyEntered), body);

		QueueFree();
	}

	private void HandleBodyEntered(Node2D body)
	{
		SetDeferred("monitoring", false);
		var newExplosion = explosion.Instantiate<Node2D>();
		newExplosion.GlobalPosition = this.GlobalPosition;
		GetParent().AddChild(newExplosion);
	}
}
