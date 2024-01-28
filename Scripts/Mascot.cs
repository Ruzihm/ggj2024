using Godot;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class Mascot : Node2D {
	private AnimatedSprite2D _animatedSprite2D;
	private NinePatchRect _dialogBox;
	private Label _dialogText;
	
	[Export]
	private float FollowSpeed = 4.0f;
	
	private Vector2 TweenTarget;

	public override void _Ready() {
		_animatedSprite2D = GetNode<AnimatedSprite2D>("Sprite");
		_dialogBox = GetNode<NinePatchRect>("DialogBox");
		_dialogText = GetNode<Label>("DialogBox/DialogText");
		TweenTarget = Position;
	}

	public override void _Input(InputEvent @event) {
		//Just Debug to test stuff
		if (@event is InputEventKey keyEvent && keyEvent.Pressed) {
			if (keyEvent.Keycode == Key.Q) {
				GD.Print("Q was pressed");
				PlayAnimation("idle", 0f, 5f);
			}
			if (keyEvent.Keycode == Key.W) {
				GD.Print("W was pressed");
				Vector2 mousePos = GetGlobalMousePosition();
				SetTweenTarget(mousePos);
			}
			if (keyEvent.Keycode == Key.E) {
				GD.Print("E was pressed");
				PlayText("TEST text goes here stupid", 5f, 10f);
			}
		}
	}

	public override void _PhysicsProcess(double delta) {
		Position = Position.Lerp(TweenTarget, (float)delta * FollowSpeed);
	}

	public override void _Process(double delta) {
	}
	
	// Play Animation
	public async Task PlayAnimation(string animationName, float startDelaySeconds = -1f, float stopAfterSeconds = -1f) {
		if (startDelaySeconds > 0) {
			int startDelayMS = (int)(startDelaySeconds * 1000);
			await Task.Delay(startDelayMS);
		}
		_animatedSprite2D.Play(animationName);
		if (stopAfterSeconds > 0f) {
			int stopDelayMS = (int)(stopAfterSeconds * 1000);
			await Task.Delay(stopDelayMS);
			_animatedSprite2D.Stop();
		}
	}
	
	// Tween to location
	public void SetTweenTarget(Vector2 targetPosition) {
		TweenTarget = targetPosition;
	}
	
	// Play Text
	// -Needs to resize the text box to correct size before making the characters gradually visible
	// -Tween textbox size or alpha before text starts
	public async Task PlayText(string text, float textTimeSeconds, float hideTimeSeconds = 10f) {
		_dialogBox.Visible = true;
		_dialogText.VisibleCharacters = 0;
		_dialogText.Text = text;
		int delayTimeMS = (int)(textTimeSeconds * 1000) / text.Length;
		int hideTimeMS = (int)(hideTimeSeconds * 1000) / text.Length;
		for (int i = 1; i <= text.Length; i++) {
			_dialogText.VisibleCharacters = i;
			await Task.Delay(delayTimeMS);
		}
		await Task.Delay(hideTimeMS);
		_dialogBox.Visible = false;
	}
}
