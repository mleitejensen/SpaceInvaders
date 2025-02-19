using Godot;
using System;

public partial class SignalBus : Node
{
	private static SignalBus _instance;
	public static SignalBus Instance => _instance;

	[Signal]
	public delegate void PlayerHealthChangeEventHandler(int healthPoints);

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
