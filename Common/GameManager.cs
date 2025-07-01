using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export]
	public PackedScene EnemyScene;
	[Export]
	public int NumberOfEnemies = 5;
	private TileMapLayer tilemap;
	private const int NumberOfRows = 0;
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
		tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		enemyGroup = GetNode<Node>("%Enemies");
		textScore = GetNode<RichTextLabel>("%Score");
		healthSprite1 = GetNode<Sprite2D>("%Heart1");
		healthSprite2 = GetNode<Sprite2D>("%Heart2");
		healthSprite3 = GetNode<Sprite2D>("%Heart3");


		SignalBus.Instance.Connect(SignalBus.SignalName.ScoreUpdate, Callable.From<int>(OnScoreUpdate));
		SignalBus.Instance.Connect(SignalBus.SignalName.PlayerHealthChange, Callable.From<int>(OnHealthUpdate));

		for (int index = -11; index < NumberOfEnemies; index++)
		{
			for (int rowIndex = -7; rowIndex < NumberOfRows; rowIndex++)
			{
				var enemy = EnemyScene.Instantiate<CharacterBody2D>();
				var targetPosition = tilemap.MapToLocal(tilemap.LocalToMap(new Vector2(tilemap.TileSet.TileSize.X * index, tilemap.TileSet.TileSize.Y * rowIndex)));
				enemy.Position = targetPosition;
				enemyGroup.AddChild(enemy);
			}
		}
	}

	public override void _Process(double delta)
	{
	}

	private void OnTimerEnemyShootTimeout()
	{
		var list = enemyGroup.GetChildren();

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

		var list = enemyGroup.GetChildren();
		if (list.Count <= 40)
		{
			GD.Print("40 enemies left, multiplying by 4");
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemySpeedChange, 4);
		}
		else if (list.Count <= 80)
		{
			GD.Print("80 enemies left, multiplying by 2");
			SignalBus.Instance.EmitSignal(SignalBus.SignalName.EnemySpeedChange, 2);
		}
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
