using Godot;
using System;

public partial class EnemyBullet : Bullet
{
	private Timer timer;

	public override void _Ready()
	{
		speed = -500;

		timer = GetNode<Timer>("%Timer");
		timer.Timeout += OnTimerTimeout;
	}
}
