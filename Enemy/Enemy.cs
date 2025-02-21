using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public AnimatedSprite2D animatedSprite2D;
	public Timer timer;
	public TileMapLayer tilemap;
	private int speed = 64;
	private bool movingRight = true;
	private Area2D hitbox;

	public override void _Ready()
	{
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyMove, Callable.From(OnMove));
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyChangeDirection, Callable.From(OnChangeDirection));

		animatedSprite2D = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		hitbox = GetNode<Area2D>("%Hitbox");

		hitbox.BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	private void OnMove()
	{
		var targetPosition = tilemap.MapToLocal(tilemap.LocalToMap(new Vector2(GlobalPosition.X + (movingRight ? 64 : -64), GlobalPosition.Y)));
		Position = targetPosition;

		if (animatedSprite2D.Animation == "first")
		{
			animatedSprite2D.Play("second");
		}
		else
		{
			animatedSprite2D.Play("first");

		}
	}

	private void OnChangeDirection()
	{
		movingRight = !movingRight;
	}

	private void OnBodyEntered(Node2D body)
	{
		body.QueueFree();
		QueueFree();
	}
}
