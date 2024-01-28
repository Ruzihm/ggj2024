using Godot;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

public partial class CaptchaGag : BaseGag
{
	bool attempted = false;
	bool inputPossible = true;

	[Export]
	public LineEdit inputText;

	public override void _Ready()
	{
		base._Ready();
		cursor.BeginDialogMode();
		_ = mascot.PlayText("Prove you are human. How many grains of sand are pictured in the shown image?", 3);
		inputText.GrabFocus();
	}

	public async Task Submit()
	{
		if (!inputPossible) return;

		inputPossible = false;
		string val = inputText.Text;

		if (val.Length == 0) {
			inputPossible = true;
			return;
		}

		if (attempted)
		{
			_ = mascot.PlayText("Eh, close enough.", 1);
			cursor.ResetControls();
			EmitSignal(BaseGag.SignalName.OnComplete);
		}
		else
		{
			inputText.Text = "";
			await mascot.PlayText("Incorrect. Please try again. Remaining attempts: 98432786318", 5);
			inputPossible = true;
			attempted = true;
		}
	}
	
	public void OnGUI(InputEvent @event)
	{
		if (@event.IsPressed() && @event is InputEventKey eventKey)
		{
			if (eventKey.Keycode == Key.Enter || eventKey.Keycode == Key.KpEnter)
			{
				Submit();
			}
		}
	}
}
