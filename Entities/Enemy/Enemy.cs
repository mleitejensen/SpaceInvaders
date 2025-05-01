using Godot;

public partial class Enemy : CharacterBody3D
{
	public const float Speed = 5.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Vector3 direction = (Transform.Basis * new Vector3(1, 0, 0)).Normalized();
		// velocity.X = direction.X * Speed;
		// velocity.Z = direction.Z * Speed;

		Velocity = velocity;
		MoveAndSlide();
	}
}
