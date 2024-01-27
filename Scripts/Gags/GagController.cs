using Godot;
using System;

public partial class GagController : Timer
{
	[Export]
	public Godot.Collections.Array<PackedScene> gagScenes;

	[Export]
	public cursor_controller Cursor;

	private BaseGag currentGag;

	public void OnTimeout()
	{
		var newScene = gagScenes[(int)(GD.Randi() % gagScenes.Count)];
		currentGag = newScene.Instantiate() as BaseGag;
		currentGag.Cursor = Cursor;
		currentGag.OnComplete += OnGagComplete;

		AddChild(currentGag);
	}

	private void OnGagComplete()
	{
		currentGag.QueueFree();
		currentGag = null;
		Start();
	}
}
