using Godot;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class Mascot : Node2D {
	private AnimatedSprite2D _animatedSprite2D;
	private NinePatchRect _dialogBox;
	private Label _dialogText;
	
	[Export]
	private float FollowSpeed = 4.0f;
	
	// Super hacky shit
	[Export(PropertyHint.MultilineText)]
	private string taunt0;
	
	[Export(PropertyHint.MultilineText)]
	private string taunt1;
	
	[Export(PropertyHint.MultilineText)]
	private string taunt2;
	
	[Export(PropertyHint.MultilineText)]
	private string taunt3;
	
	private Godot.Collections.Array<string> _taunts;
	
	private Vector2 TweenTarget;
	private bool playingText = false;

	public override void _Ready() {
		_animatedSprite2D = GetNode<AnimatedSprite2D>("Sprite");
		_dialogBox = GetNode<NinePatchRect>("Node2D/DialogBox");
		_dialogText = GetNode<Label>("DialogText");
		TweenTarget = Position;
		_dialogText.VisibleCharacters = 0;
		_dialogBox.Visible = false;
		_taunts = new Godot.Collections.Array<string>{ taunt0 };//, taunt1, taunt2, taunt3 };
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
			_animatedSprite2D.Play("Idle");
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
		GD.Print("attempt text");
		if (!playingText)
		{
			GD.Print("play text");
			playingText = true;
			_dialogBox.Visible = true;
			_dialogText.VisibleCharacters = 0;
			_dialogText.Text = text;
			int delayTimeMS = (int)(textTimeSeconds * 1000) / text.Length;
			int hideTimeMS = (int)(hideTimeSeconds * 1000);
			PlayAnimation("Chatter", 0f, 0f);
			for (int i = 1; i <= text.Length; i++) {
				_dialogText.VisibleCharacters = i;
				await Task.Delay(delayTimeMS);
			}
			PlayAnimation("Left", 0f, 0f);
			await Task.Delay(hideTimeMS);
			_dialogText.VisibleCharacters = 0;
			_dialogBox.Visible = false;
			playingText = false;
		}
	}
	
	private void _on_sprite_animation_finished()
	{
		if(_animatedSprite2D.Animation == "Ascend")
		{
			_animatedSprite2D.Play("Idle");
			PlayText(_taunts[0], 5f, 2.5f);
		}
		if(_animatedSprite2D.Animation == "Left")
		{
			_animatedSprite2D.Play("Idle");
		}
		if(_animatedSprite2D.Animation == "Descend")
		{
			_animatedSprite2D.Visible = false;
		}
	}
}
