using Godot;
using System;

public partial class Barrier : Area2D
{
	private Sprite2D sprite;
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("./Sprite2D");
		var bitmap = new Bitmap();
		bitmap.CreateFromImageAlpha(sprite.Texture.GetImage());

		this.BodyEntered += OnBodyEntered;

		var polys = bitmap.OpaqueToPolygons(new Rect2I(Vector2I.Zero, (Vector2I)sprite.Texture.GetSize()));
		foreach (var poly in polys)
		{
			var collisionPolygon = new CollisionPolygon2D();
			collisionPolygon.Polygon = poly;
			AddChild(collisionPolygon);

			collisionPolygon.Scale = new Vector2(2, 2);
			if (sprite.Centered)
			{
				collisionPolygon.Position -= bitmap.GetSize();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("test");
		var position = body.Position;
		var image = Image.CreateFromData(32, 32, false, Image.Format.Rgba8, sprite.Texture.GetImage().GetData());
		image.SetPixel((int)16, (int)16, Colors.Red);

		sprite.Texture = ImageTexture.CreateFromImage(image);
	}
}
