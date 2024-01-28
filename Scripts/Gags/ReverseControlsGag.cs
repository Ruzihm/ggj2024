using Godot;
using System;

public partial class ReverseControlsGag : BaseGag
{
	[Export]
	public Timer timer ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//"Let's make sure you aren't working under the influence. Engaging sobriety test mode."
		_ = mascot.PlayText("I noticed you're not going as fast as you could be. Let me turn on inverted controls for you, did that help?", 3, 5);
		cursor.ReverseControls();
	}

	private void _on_timer_timeout()
	{
		cursor.ResetControls();
		EmitSignal(BaseGag.SignalName.OnComplete);
	}
}

