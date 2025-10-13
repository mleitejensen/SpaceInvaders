using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node2D
{
	[Export]
	public EnemyManager EnemyManager { get; set; }
	[Export]
	public PackedScene EnemyScene;
	[Export]
	public int NumberOfEnemies = 5;
	private TileMapLayer _tilemap;
	private const int NumberOfRows = 0;
	private Node _enemyGroup;
	private RichTextLabel _textScore;
	private int _score = 0;
	private int _playerLives = 3;
	private Sprite2D _healthSprite1;
	private Sprite2D _healthSprite2;
	private Sprite2D _healthSprite3;
	public override void _Ready()
	{
		_tilemap = GetNode<TileMapLayer>("/root/Game/TileMapLayer");
		_enemyGroup = GetNode<Node>("%Enemies");
		_textScore = GetNode<RichTextLabel>("%Score");
		_healthSprite1 = GetNode<Sprite2D>("%Heart1");
		_healthSprite2 = GetNode<Sprite2D>("%Heart2");
		_healthSprite3 = GetNode<Sprite2D>("%Heart3");


		SignalBus.Instance.Connect(SignalBus.SignalName.ScoreUpdate, Callable.From<int>(OnScoreUpdate));
		SignalBus.Instance.Connect(SignalBus.SignalName.PlayerHealthChange, Callable.From<int>(OnHealthUpdate));

		for (int index = -11; index < NumberOfEnemies; index++)
		{
			for (int rowIndex = -7; rowIndex < NumberOfRows; rowIndex++)
			{
				var enemy = EnemyScene.Instantiate<CharacterBody2D>();
				var targetPosition = _tilemap.MapToLocal(_tilemap.LocalToMap(new Vector2(_tilemap.TileSet.TileSize.X * index, _tilemap.TileSet.TileSize.Y * rowIndex)));
				enemy.Position = targetPosition;
				_enemyGroup.AddChild(enemy);
			}
		}

	}

	public override void _Process(double delta)
	{

	}

	private void OnScoreUpdate(int addScore)
	{
		_score += addScore;
		_textScore.Text = $"Score: {_score}";

		var list = _enemyGroup.GetChildren().ToList();
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
		_playerLives -= damage;

		switch (_playerLives)
		{
			case 2:
				_healthSprite3.Visible = false;
				break;
			case 1:
				_healthSprite2.Visible = false;
				break;
			case 0:
				_healthSprite1.Visible = false;
				break;
		}

	}

}
