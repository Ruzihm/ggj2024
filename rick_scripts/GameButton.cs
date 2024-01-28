using Godot;
using System;

public partial class GameButton : Area2D
{
	[Export]
	public cursor_controller cursor;
	
	[Export]
	private Sprite2D sprite;

	public enum ButtonType
	{
		File,
		Destination,
		Control
	}

	public enum DestType
	{
		Trash,
		Images,
		Marketing
	}

	[Export]
	public DestType destType;


	[Export]
	public ButtonType buttonType;

	[Export]
	public Label label;

	[Signal]
	public delegate void OnClickEventHandler();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(Node2D body)
	{
		cursor.OnEnter(this);
	}
	public void _on_body_exited(Node2D body)
	{
		cursor.OnExit(this);
	}

	public bool Deposit(GameButton dest)
	{
		QueueFree();
		return dest.destType == destType;
	}

	public void SetName(string name)
	{
		label.Text = name;
	}
	
	public void SetTexture(Texture2D newTexture)
	{
		sprite.Texture = newTexture;
	}
}
