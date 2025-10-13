using Godot;
using System;

public partial class PlayerBullet : Bullet
{
	private Timer _timer;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		Speed = 400;

		_timer = GetNode<Timer>("%Timer");
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.Die();
		}
		QueueFree();
	}
}
