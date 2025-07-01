using Godot;
using System;

public partial class MainMenu : Control
{
	private Button playButton;
	private Button quitButton;
	public override void _Ready()
	{
		playButton = GetNode<Button>("%Play");
		quitButton = GetNode<Button>("%Quit");

		playButton.ButtonUp += OnPlayButtonClicked;
		quitButton.ButtonUp += OnQuitButtonClicked;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayButtonClicked()
	{
		GetTree().ChangeSceneToFile("res://game.tscn");
	}
	private void OnQuitButtonClicked()
	{
		GetTree().Quit();
	}
}
