using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class EnemyManager : Node
{
	private Timer timerEnemyShoot;
	private Node enemyGroupNode;
	public List<Enemy> Enemies { get; set; }

	public override void _Ready()
	{
		enemyGroupNode = GetNode<Node>("%Enemies");

		timerEnemyShoot = GetNode<Timer>("%EnemyShoot");
		timerEnemyShoot.Timeout += OnEnemyShoot;
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

	private Enemy GetLowestEnemies()
	{
		var list = enemyGroupNode.GetChildren().ToList();

		if (list.Count == 0)
		{
			return null;
		}

		Enemies = GetEnemies(list);

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

			var foundEnemy = canShoot.Find((e) => e.Position.X == enemy.Position.X);
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


	private List<Enemy> GetEnemies(List<Node> list)
	{
		List<Enemy> enemies = new();

		foreach (var enemy in list)
		{
			if (enemy is Enemy e)
			{
				enemies.Add(e);
			}
		}

		return enemies;
	}
}
