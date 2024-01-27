using Godot;
using System;

public partial class ReverseControlsGag : BaseGag
{
	[Export]
	public Timer timer ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Cursor.ReverseControls();
	}

	private void _on_timer_timeout()
	{
		Cursor.ResetControls();
		EmitSignal(BaseGag.SignalName.OnComplete);
	}
}
