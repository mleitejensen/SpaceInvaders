using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	public PackedScene PlayerBulletScene;
	public const float Speed = 5.0f;
	public bool canShoot = true;
	private Timer ShootTimer;
	private Node3D GameScene;

	public override void _Ready()
	{
		GameScene = GetNode<Node3D>("/root/Game");
		ShootTimer = GetNode<Timer>("%ShootCooldown");

		ShootTimer.Timeout += OnTimerTimeout;
	}


	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_accept") && canShoot)
		{
			Shoot();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void Shoot()
	{
		canShoot = false;
		var bullet = PlayerBulletScene.Instantiate<PlayerBullet>();
		bullet.Position = Position + new Vector3(0, 2, 0);
		GameScene.AddChild(bullet);
		ShootTimer.Start();
	}

	private void OnTimerTimeout()
	{
		canShoot = true;
	}
}