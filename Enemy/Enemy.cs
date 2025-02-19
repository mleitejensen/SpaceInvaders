using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public Timer timer;
	public Sprite2D sprite2D;
	public AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		timer = GetNode<Timer>("%Timer");
		timer.Timeout += OnTimerTimeout;
		animatedSprite2D = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{

	}

	private void OnTimerTimeout()
	{
		Position = new Vector2(Position.X + 40, Position.Y);

		if (animatedSprite2D.Animation == "first")
		{
			animatedSprite2D.Play("second");
		}
		else
		{
			animatedSprite2D.Play("first");

		}
	}
}
