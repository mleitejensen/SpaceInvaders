using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	private float speed = 200;

	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = new Vector2(this.Position.X, this.Position.Y + ((float)delta * -speed));
	}
}
