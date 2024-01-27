using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class cursor_controller : RigidBody2D
{
	Viewport vp;
	Rect2 hitbox;

	List<RigidBody2D> overlappeds_rbs;
	List<GameButton> overlappeds_buttons;

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
	}

	private void ClickMouse(InputEventMouseButton eventMouseButton)
	{
		if (eventMouseButton.IsPressed())
		{

		}
	}
}
