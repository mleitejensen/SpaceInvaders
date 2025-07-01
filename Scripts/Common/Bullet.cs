using Godot;
using System;

public partial class Bullet : Area2D
{
	public float speed = 500;

	public override void _PhysicsProcess(double delta)
	{
		Position = new Vector2(this.Position.X, this.Position.Y + ((float)delta * -speed));
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
