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
	private RichTextLabel textScore;
	private int score = 0;
	private int playerLives = 3;
	private Sprite2D healthSprite1;
	private Sprite2D healthSprite2;
	private Sprite2D healthSprite3;
	public override void _Ready()
	{
		timerMoveEnemy = GetNode<Timer>("%MoveEnemy");
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		enemyGroup = GetNode<Node>("%Enemies");
		timerEnemyShoot = GetNode<Timer>("%EnemyShoot");
		textScore = GetNode<RichTextLabel>("%Score");
		healthSprite1 = GetNode<Sprite2D>("%Heart1");
		healthSprite2 = GetNode<Sprite2D>("%Heart2");
		healthSprite3 = GetNode<Sprite2D>("%Heart3");

		timerMoveEnemy.Timeout += OnTimerMoveEnemyTimeout;
		timerEnemyShoot.Timeout += OnTimerEnemyShootTimeout;

		SignalBus.Instance.Connect(SignalBus.SignalName.ScoreUpdate, Callable.From<int>(OnScoreUpdate));
		SignalBus.Instance.Connect(SignalBus.SignalName.PlayerHealthChange, Callable.From<int>(OnHealthUpdate));

		for (int index = 1; index < NumberOfEnemies; index++)
		{
			for (int rowIndex = 1; rowIndex < NumberOfRows; rowIndex++)
			{
				var enemy = EnemyScene.Instantiate<CharacterBody2D>();
				var targetPosition = tilemap.MapToLocal(tilemap.LocalToMap(new Vector2(64 * index, 50 * rowIndex)));
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
		if (list.Count <= 40)
		{
			timerMoveEnemy.WaitTime = 0.8;
		}
		else if (list.Count <= 20)
		{
			timerMoveEnemy.WaitTime = 0.5;
		}

		if (list.Count <= 0)
		{
			return;
		}
		int randomIndex = new Random().Next(0, list.Count);
		Enemy enemy = (Enemy)enemyGroup.GetChildren()[randomIndex];
		enemy.Shoot();
	}

	private void OnScoreUpdate(int addScore)
	{
		score += addScore;
		textScore.Text = $"Score: {score}";
	}

	private void OnHealthUpdate(int damage)
	{
		playerLives -= damage;

		switch (playerLives)
		{
			case 2:
				healthSprite3.Visible = false;
				break;
			case 1:
				healthSprite2.Visible = false;
				break;
			case 0:
				healthSprite1.Visible = false;
				break;
		}

	}

}
