using Godot;
using System;

public partial class EnemyAreaEnd : Area2D
{
	private float _debounceTime = 0.5f;
	private Timer _debounceTimer = new();

	public override void _Ready()
	{
		_debounceTimer.WaitTime = _debounceTime;
		_debounceTimer.OneShot = true;
		AddChild(_debounceTimer);

		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy && _debounceTimer.IsStopped())
		{
			_debounceTimer.Start();
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemyChangeDirection);
		}
	}

}
