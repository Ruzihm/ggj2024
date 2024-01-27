using Godot;
using System;

public partial class MenuController : Node
{
	[Export]
	public PackedScene gameScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnStart()
	{
		GetTree().ChangeSceneToPacked(gameScene);
	}

	public void OnQuit()
	{
		GD.Print("attempting to quit...");
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
		GetTree().Quit();
	}
}
