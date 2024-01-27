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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		vp = GetViewport();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
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
				if (button.buttonType == GameButton.ButtonType.File)
				{
					draggedButton = button;
					break;
				}
			}
		} else if (eventMouseButton.IsReleased())
		{

			if (draggedButton != null)
			{
				foreach (GameButton destButton in hoveredButtons)
				{
					if (destButton.buttonType == GameButton.ButtonType.Destination)
					{
						bool result = draggedButton.Deposit(destButton);
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
}
