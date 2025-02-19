using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	private float speed = 200;
	private Timer timer;

	public override void _Ready()
	{
		timer = GetNode<Timer>("%Timer");
		timer.Timeout += OnTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = new Vector2(this.Position.X, this.Position.Y + ((float)delta * -speed));
	}

	private void OnTimerTimeout()
	{
		QueueFree();
	}
}
