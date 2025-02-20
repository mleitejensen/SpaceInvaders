using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public Timer timer;
	public Sprite2D sprite2D;
	public AnimatedSprite2D animatedSprite2D;
	public TileMapLayer tilemap;
	private int speed = 64;
	private bool movingRight = true;

	public override void _Ready()
	{
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyMove, Callable.From(OnMove));
		SignalBus.Instance.Connect(SignalBus.SignalName.EnemyChangeDirection, Callable.From(OnChangeDirection));

		animatedSprite2D = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
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
}
