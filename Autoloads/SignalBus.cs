using Godot;
using System;

public partial class SignalBus : Node
{
	private static SignalBus _instance;
	public static SignalBus Instance => _instance;

	[Signal]
	public delegate void PlayerHealthChangeEventHandler(int healthPoints);
	[Signal]
	public delegate void EnemyChangeDirectionEventHandler();
	[Signal]
	public delegate void EnemyMoveEventHandler();
	[Signal]
	public delegate void ScoreUpdateEventHandler(int addScore);

	public override void _Ready()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			QueueFree(); // Ensure only one instance exists
		}
	}
}
