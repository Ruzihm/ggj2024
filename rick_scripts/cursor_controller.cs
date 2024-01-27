using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class cursor_controller : RigidBody2D
{
	Viewport vp;
	Rect2 hitbox;

	GameButton draggedButton;
	HashSet<GameButton> hoveredButtons = new HashSet<GameButton>();

	private bool dialogMode = false;

	private bool controlsReversed = false;
	private bool gravityEnabled = false;
	
	[Signal]
	public delegate void FileDepositedEventHandler(bool correct);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		vp = GetViewport();
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (gravityEnabled && draggedButton != null) 
		{
			MoveAndCollide(new Vector2(0, 50));
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			MoveMouse(eventMouseMotion);
		}
		else if (@event is InputEventMouseButton eventMouseButton)
		{
			ClickMouse(eventMouseButton);
		}
	}

	private void MoveMouse(InputEventMouseMotion eventMouseMotion)
	{
		var relative = eventMouseMotion.Relative;

		if (controlsReversed)
		{
			relative = new Vector2(-relative.X, relative.Y);
		}

		MoveAndCollide(relative);
		if (draggedButton != null)
		{
			draggedButton.GlobalPosition = GlobalPosition;
		}
	}

	private void ClickMouse(InputEventMouseButton eventMouseButton)
	{
		if (eventMouseButton.IsPressed())
		{
			foreach(GameButton button in hoveredButtons)
			{
				switch (button.buttonType)
				{
					default:
						break;
					case GameButton.ButtonType.File:
						if (!dialogMode)
						{
							draggedButton = button;
						}
						break;
					case GameButton.ButtonType.Control:
						button.EmitSignal(GameButton.SignalName.OnClick);
						break;
				} 
			}
		}
		else if (eventMouseButton.IsReleased())
		{

			if (draggedButton != null)
			{
				foreach (GameButton destButton in hoveredButtons)
				{
					if (destButton.buttonType == GameButton.ButtonType.Destination)
					{
						bool result = draggedButton.Deposit(destButton);
						EmitSignal(SignalName.FileDeposited, result);
						GD.Print("drop correctly: " + result);
						hoveredButtons.Remove(draggedButton);
					}
				}
			}

			draggedButton = null;
		}
	}

	public void OnEnter(GameButton button)
	{
		hoveredButtons.Add(button);
	}

	public void OnExit(GameButton button)
	{
		hoveredButtons.Remove(button);
	}

	public void ReverseControls()
	{
		controlsReversed = true;
	}

	public void EnableGravity()
	{
		gravityEnabled = true;
	}

	public void BeginDialogMode()
	{
		dialogMode = true;
		draggedButton = null;
	}

	public void ResetControls()
	{
		gravityEnabled = false;
		controlsReversed = false;
	}
}
