using Godot;
using System;

public partial class GameManager : Control {
	private TextureProgressBar _progressBar;
	private Timer _timeLimit;
	private TextureButton _progressButton;
	private GameButton _progressGameButton;
	private Label _progressLabel;
	
	[Export]
	private FileSpawner _fileSpawner;
	
	[Export]
	private GagController _gagController;
	
	[Export]
	private Mascot _mascot;
	
	private Label _timerLabel;
	
	[Export]
	private int StartingNumFiles = 1;
	
	[Export]
	private float FileSpawnInterval = 5f;
	
	[Export]
	private float CorrectValue = 10f;
	
	[Export]
	private float IncorrectPenalty = 0f;
	
	[Export]
	// 0 or less for no time limit
	private float TimeLimit = 0f;
	
	private bool InProgress = false;
	private float ElapsedTime = 0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_progressBar = GetNode<TextureProgressBar>("TextureProgressBar");
		_timeLimit = GetNode<Timer>("Timer");
		_timerLabel = GetNode<Label>("TimerLabel");
		_progressButton = GetNode<TextureButton>("Background/Area2D/TextureButton");
		_progressLabel = GetNode<Label>("Background/Area2D/TextureButton/ProgressLabel");
		_progressGameButton = GetNode<GameButton>("Background/Area2D");
		
		StartGame(StartingNumFiles, FileSpawnInterval, CorrectValue, IncorrectPenalty, TimeLimit);
	}
	
	public void StartGame(int startingNumFiles, float fileSpawnInterval, float correctValue, float incorrectPenalty, float timeLimit) {
		StartingNumFiles = startingNumFiles;
		FileSpawnInterval = fileSpawnInterval;
		CorrectValue = correctValue;
		IncorrectPenalty = incorrectPenalty;
		TimeLimit = timeLimit;
		
		InProgress = true;
		ElapsedTime = 0f;
		
		if (TimeLimit > 0f)
			_timerLabel.Text = string.Format("{0:00.00}", _timeLimit.TimeLeft);
		else
			_timerLabel.Text = string.Format("{0:00.00}", ElapsedTime);
		
		_progressBar.Value = 0f;
		_progressButton.Disabled = true;
		_progressLabel.Text = string.Format("{0}%", _progressBar.Value);
		
		for (int i = 0; i < startingNumFiles; i++)
			_fileSpawner.OnTimeout();
			
		_fileSpawner.Start(FileSpawnInterval);
		_gagController.Start();
		
		if (TimeLimit > 0f)
			_timeLimit.Start(TimeLimit);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Q)
			{
				GD.Print("Q was pressed");
				//DEBUG
				OpenExit();
			}
			
			if (keyEvent.Keycode == Key.W)
			{
				GD.Print("W was pressed");
				EndGame(false);
			}
		}
	}
	
	public void WinGame()
	{
		if (_progressButton.Disabled) return;

		GD.Print("WIN GAME");
		EndGame(true);
	}
	
	public void EndGame(bool win)
	{
		GD.Print("GAME OVER");
		_gagController.Stop();
		_timeLimit.Stop();
		_fileSpawner.Stop();
		InProgress = false;
		
		if (win)
		{
			_mascot.PlayText("NO, THE FIRE!!!!\nIT BURNS...", 5f, 10f);
			_mascot.PlayAnimation("Descend", 6f, 0f);
		}
		else
		{
			_mascot.PlayText("Oh no... Looks like you'll have to restart your shift. Guess you're stuck with me a little longer!", 5f, 10f);
			_mascot.PlayAnimation("Chomp", 6f, 0f);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (InProgress) {
			ElapsedTime += (float)delta;
			
			if (TimeLimit > 0f)
				_timerLabel.Text = string.Format("{0:00.00}", _timeLimit.TimeLeft);
			else
				_timerLabel.Text = string.Format("{0:00.00}", ElapsedTime);
			
			if (_progressButton.Disabled && _progressBar.Value >= _progressBar.MaxValue) {
				OpenExit();
			}
		}
	}
	
	private void OpenExit()
	{
		GD.Print("OpenExit");
		_progressButton.Disabled = false;
		_progressLabel.Text = "EXIT";
	}
	
	private void _on_timer_timeout()
	{
		EndGame(false);
	}
	
	private void _on_cursor_file_deposited(bool correct)
	{
		if (_progressBar.Value < _progressBar.MaxValue)
		{
			_progressBar.Value += correct ? CorrectValue : IncorrectPenalty;
			_progressLabel.Text = string.Format("{0}%", _progressBar.Value);
		}
	}
}
