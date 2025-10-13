using Godot;
using System;

public partial class Bullet : Area2D
{
	public float Speed = 500;

	public override void _PhysicsProcess(double delta)
	{
		Position = new Vector2(Position.X, Position.Y + ((float)delta * -Speed));
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
