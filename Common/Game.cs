using Godot;

public partial class Game : Node3D
{
	[Export]
	public PackedScene EnemyScene;
	private Node Enemies;
	private int NumberOfEnemies = 14;
	private int NumberOfRows = 4;

	public override void _Ready()
	{
		Enemies = GetNode<Node>("%Enemies");

		Vector3 basePosition = GlobalPosition + new Vector3(-15, 10, 0);
		for (int row = 1; row < NumberOfRows; row++)
		{

			var targetPosition = basePosition + new Vector3(0, row * 2, 0);
			for (int index = 0; index < NumberOfEnemies; index++)
			{

				var enemy = EnemyScene.Instantiate<Enemy>();

				targetPosition += new Vector3(2, 0, 0);
				enemy.Position = targetPosition;
				Enemies.AddChild(enemy);

			}
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
