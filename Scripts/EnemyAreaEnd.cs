using Godot;
using System;

public partial class EnemyAreaEnd : Area2D
{
	public override void _Ready()
	{
		this.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print(body.GetType());
		if (body is Enemy)
		{
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemyChangeDirection);
		}
	}

}
