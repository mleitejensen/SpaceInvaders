using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export]
	public PackedScene EnemyScene;
	[Export]
	public int NumberOfEnemies = 11;
	private TileMapLayer tilemap;
	private Timer timer;
	private const int MaxNumberOfMoves = 6;
	private const int NumberOfRows = 6;
	private int numberOfMoves = 0;
	private bool MovingRight = true;
	private bool isNotFirstMove = false;
	public override void _Ready()
	{
		timer = GetNode<Timer>("%MoveEnemy");
		timer.Timeout += OnTimerTimeout;
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");

		for (int i = 1; i < NumberOfEnemies; i++)
		{
			for (int j = 1; j < NumberOfRows; j++)
			{
				var enemy = EnemyScene.Instantiate<CharacterBody2D>();
				var targetPosition = tilemap.MapToLocal(tilemap.LocalToMap(new Vector2(64 * i, 64 * j)));
				enemy.Position = targetPosition;
				AddChild(enemy);

			}
		}
	}

	public override void _Process(double delta)
	{

	}
	private void OnTimerTimeout()
	{
		if (numberOfMoves >= MaxNumberOfMoves || (numberOfMoves <= 0 && isNotFirstMove))
		{
			MovingRight = !MovingRight;
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemyChangeDirection);
		}


		if (MovingRight)
		{
			numberOfMoves++;
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemyMove);
		}
		else
		{
			numberOfMoves--;
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemyMove);
		}

		isNotFirstMove = true;
	}


}
