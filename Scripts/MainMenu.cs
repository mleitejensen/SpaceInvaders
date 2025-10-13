using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _playButton;
	private Button _quitButton;
	public override void _Ready()
	{
		_playButton = GetNode<Button>("%Play");
		_quitButton = GetNode<Button>("%Quit");

		_playButton.ButtonUp += OnPlayButtonClicked;
		_quitButton.ButtonUp += OnQuitButtonClicked;
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
