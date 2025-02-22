using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export]
	public PackedScene EnemyScene;
	[Export]
	public int NumberOfEnemies = 11;
	private TileMapLayer tilemap;
	private Timer timerMoveEnemy;
	private const int MaxNumberOfMoves = 6;
	private const int NumberOfRows = 6;
	private int numberOfMoves = 0;
	private bool MovingRight = true;
	private bool isNotFirstMove = false;
	private Node enemyGroup;
	private Timer timerEnemyShoot;
	public override void _Ready()
	{
		timerMoveEnemy = GetNode<Timer>("%MoveEnemy");
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		enemyGroup = GetNode<Node>("%Enemies");
		timerEnemyShoot = GetNode<Timer>("%EnemyShoot");

		timerMoveEnemy.Timeout += OnTimerMoveEnemyTimeout;
		timerEnemyShoot.Timeout += OnTimerEnemyShootTimeout;

		for (int index = 1; index < NumberOfEnemies; index++)
		{
			for (int rowIndex = 1; rowIndex < NumberOfRows; rowIndex++)
			{
				var enemy = EnemyScene.Instantiate<CharacterBody2D>();
				var targetPosition = tilemap.MapToLocal(tilemap.LocalToMap(new Vector2(64 * index, 64 * rowIndex)));
				enemy.Position = targetPosition;
				enemyGroup.AddChild(enemy);
			}
		}
	}

	public override void _Process(double delta)
	{

	}
	private void OnTimerMoveEnemyTimeout()
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

	private void OnTimerEnemyShootTimeout()
	{
		var list = enemyGroup.GetChildren();
		int randomIndex = new Random().Next(0, list.Count);
		Enemy enemy = (Enemy)enemyGroup.GetChildren()[randomIndex];
		enemy.Shoot();
	}

}
