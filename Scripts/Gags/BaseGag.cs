using Godot;
using System;
using System.Threading;

public partial class BaseGag : Node
{
	[Signal]
	public delegate void OnCompleteEventHandler();

	public cursor_controller cursor { get; set; }

	public Mascot mascot { get; set; }
}
