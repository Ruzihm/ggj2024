using Godot;
using System;

public partial class GameManager : Control {
	private ProgressBar _progressBar;
	private Timer _timeLimit;
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
		_progressBar = GetNode<ProgressBar>("ProgressBar");
		_timeLimit = GetNode<Timer>("Timer");
		_timerLabel = GetNode<Label>("TimerLabel");
	}
	
	public override void _Input(InputEvent @event) {
		//Just Debug to test stuff
		if (@event is InputEventKey keyEvent && keyEvent.Pressed) {
			if (keyEvent.Keycode == Key.Z) {
				GD.Print("Z was pressed");
				StartGame(StartingNumFiles, FileSpawnInterval, CorrectValue, IncorrectPenalty, 0f);
			}
			if (keyEvent.Keycode == Key.X) {
				GD.Print("X was pressed");
				StartGame(StartingNumFiles, FileSpawnInterval, CorrectValue, IncorrectPenalty, 600f);
			}
			if (keyEvent.Keycode == Key.M) {
				GD.Print("M was pressed");
				EndGame();
			}
		}
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
			_timeLimit.Start(TimeLimit);
	}
	
	public void EndGame()
	{
		GD.Print("GAME OVER");
		_timeLimit.Stop();
		InProgress = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (InProgress) {
			ElapsedTime += (float)delta;
			
			if (TimeLimit > 0f)
				_timerLabel.Text = string.Format("{0:00.00}", _timeLimit.TimeLeft);
			else
				_timerLabel.Text = string.Format("{0:00.00}", ElapsedTime);
			
			if (_progressBar.Value >= _progressBar.MaxValue) {
				EndGame();
			}
		}
	}
	
	private void _on_timer_timeout()
	{
		EndGame();
	}
}
