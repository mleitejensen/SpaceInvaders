using Godot;
using System;
using System.Collections.Generic;

public partial class Explosion : Area2D
{
	private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");

		this.BodyEntered += OnBodyEntered;
		animationPlayer.AnimationFinished += OnAnimationFinished;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is TileMapLayer tilemap)
		{
			List<Vector2I> cells = GetCellsInCircularArea2D(tilemap);

			foreach (var cell in cells)
			{
				tilemap.SetCell(cell, -1);
			}
		}
	}

	private void OnAnimationFinished(StringName anim_name)
	{
		QueueFree();
	}

	public List<Vector2I> GetCellsInCircularArea2D(TileMapLayer tileMap)
	{
		List<Vector2I> cells = new List<Vector2I>();

		// Step 1: Get the global position and radius of the circle
		Vector2 circleGlobalPosition = this.GlobalPosition;
		float circleRadius = ((CircleShape2D)this.GetChild<CollisionShape2D>(0).Shape).Radius;

		// Step 2: Convert the global position to the TileMap's local coordinate system
		Vector2 circleLocalPosition = tileMap.ToLocal(circleGlobalPosition);

		// Step 3: Convert the local position to cell coordinates
		Vector2I circleCellPosition = tileMap.LocalToMap(circleLocalPosition);

		// Step 4: Determine the rectangular bounds of the circle in cell coordinates
		int radiusInCells = (int)Mathf.Ceil(circleRadius / tileMap.TileSet.TileSize.X); // Assuming square tiles
		Vector2I topLeft = new Vector2I(circleCellPosition.X - radiusInCells, circleCellPosition.Y - radiusInCells);
		Vector2I bottomRight = new Vector2I(circleCellPosition.X + radiusInCells, circleCellPosition.Y + radiusInCells);

		// Step 5: Iterate over the range of cells and check if they are within the circle's radius
		for (int x = topLeft.X; x <= bottomRight.X; x++)
		{
			for (int y = topLeft.Y; y <= bottomRight.Y; y++)
			{
				Vector2I cellPosition = new Vector2I(x, y);

				// Get the center of the cell in local coordinates
				Vector2 cellCenterLocal = tileMap.MapToLocal(cellPosition) + tileMap.TileSet.TileSize / 2;

				// Check if the cell's center is within the circle's radius
				if (cellCenterLocal.DistanceTo(circleLocalPosition) <= circleRadius)
				{
					cells.Add(cellPosition);
				}
			}
		}

		return cells;
	}
}
