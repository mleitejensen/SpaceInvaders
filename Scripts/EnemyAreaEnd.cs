using Godot;
using System;

public partial class EnemyAreaEnd : Area2D
{
	private float debounceTime = 0.5f;
	private Timer debounceTimer = new();


	public override void _Ready()
	{
		debounceTimer.WaitTime = debounceTime;
		debounceTimer.OneShot = true;
		AddChild(debounceTimer);

		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy && debounceTimer.IsStopped())
		{
			debounceTimer.Start();
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemyChangeDirection);
		}
	}


}
