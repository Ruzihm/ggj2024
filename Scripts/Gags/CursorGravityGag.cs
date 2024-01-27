using Godot;
using System;

public partial class CursorGravityGag : BaseGag
{
	[Export]
	public Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Cursor.EnableGravity();
	}

	private void _on_timer_timeout()
	{
		Cursor.ResetControls();
		EmitSignal(BaseGag.SignalName.OnComplete);
	}
}
