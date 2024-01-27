using Godot;
using System;

public partial class GagController : Timer
{
	[Export]
	public Godot.Collections.Array<PackedScene> gagScenes;

	[Export]
	public cursor_controller cursor;

	[Export]
	public Mascot mascot;

	private BaseGag currentGag;

	private double startTime;

	public override void _Ready()
	{
		base._Ready();
	}

	public void OnTimeout()
	{
		var newScene = gagScenes[(int)(GD.Randi() % gagScenes.Count)];
		currentGag = newScene.Instantiate() as BaseGag;
		currentGag.cursor = cursor;
		currentGag.mascot = mascot;
		currentGag.OnComplete += OnGagComplete;

		AddChild(currentGag);
		GD.Print("starting new gag");
	}

	private void OnGagComplete()
	{
		currentGag.OnComplete -= OnGagComplete;
		currentGag.QueueFree();
		currentGag = null;
		Start();
		GD.Print("Gag complete... waiting for new to begin.");
	}
}
