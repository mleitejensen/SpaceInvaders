using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class EnemyManager : Node
{
	public List<Enemy> Enemies { get; set; }
	private Timer _timerEnemyShoot;
	private Node _enemyGroupNode;
	private float _waitTime = 1;

	public override void _Ready()
	{
		_enemyGroupNode = GetNode<Node>("%Enemies");
		AddNewTimer();
	}

	public override void _Process(double delta)
	{

	}

	private void OnEnemyShoot()
	{
		var enemy = GetLowestEnemies();
		if (enemy == null)
		{
			return;
		}

		enemy.Shoot();
	}

	public void AddNewTimer()
	{
		Timer timer = new();
		timer.Timeout += OnEnemyShoot;
		timer.WaitTime = _waitTime;
		timer.Autostart = true;
		timer.OneShot = false;
		AddChild(timer);
	}

	private Enemy GetLowestEnemies()
	{
		var children = _enemyGroupNode.GetChildren().ToList();

		if (children.Count == 0)
		{
			return null;
		}

		Enemies = GetEnemies(children);

		if (Enemies is null)
		{
			return null;
		}

		List<Enemy> canShoot = new();
		List<Godot.Vector2> positions = new();

		foreach (var enemy in Enemies)
		{
			if (!positions.Contains(enemy.Position))
			{
				positions.Add(enemy.Position);
			}
		}

		foreach (var enemy in Enemies)
		{
			if (canShoot == null)
			{
				GD.Print("first");
				canShoot.Add(enemy);
			}

			var foundEnemy = canShoot.Find(e => e.Position.X == enemy.Position.X);
			if (foundEnemy == null)
			{
				// First enemy at this X position, add it
				canShoot.Add(enemy);
			}
			else if (enemy.Position.Y > foundEnemy.Position.Y)
			{
				// If this enemy is lower (greater Y), replace
				int index = canShoot.IndexOf(foundEnemy);
				canShoot[index] = enemy;
			}
		}

		foreach (var e in canShoot)
		{
			e.Modulate = new Color(1, 0.1f, 0.07f);
		}

		Random random = new();
		return canShoot[random.Next(canShoot.Count - 1)];
	}


	private List<Enemy> GetEnemies(List<Node> nodes)
	{
		List<Enemy> enemies = new();

		foreach (var node in nodes)
		{
			if (node is Enemy enemy)
			{
				enemies.Add(enemy);
			}
		}

		return enemies;
	}
}
