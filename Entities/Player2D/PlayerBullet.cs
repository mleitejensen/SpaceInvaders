using Godot;
using System;

public partial class PlayerBullet2D : Bullet
{
	private Timer timer;

	public override void _Ready()
	{
		this.BodyEntered += OnBodyEntered;
		speed = 500;

		timer = GetNode<Timer>("%Timer");
		timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy2D enemy)
		{
			enemy.Die();
		}
		QueueFree();
	}
}
