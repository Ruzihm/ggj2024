using Godot;
using System;

public partial class CursorGravityGag : BaseGag
{
	[Export]
	public Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ = mascot.PlayText("Engaging Free Trial of NASA systems compatibility. Simulated gravity: active.", 3, 5);
		cursor.EnableGravity();
	}

	private void _on_timer_timeout()
	{
		cursor.ResetControls();
		EmitSignal(BaseGag.SignalName.OnComplete);
	}
}
