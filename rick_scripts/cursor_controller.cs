using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class cursor_controller : Node2D
{
	Viewport vp;
	Vector2 startPos;
	Rect2 hitbox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vp = GetViewport();
		startPos = GlobalPosition;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdatePosition();
		MoveMouse();
	}

	private void UpdatePosition()
	{
		var time = Time.GetTicksMsec()/1000.0;
		var posX = Mathf.PingPong(time, 2) * 100 + startPos.X;
		var curPos = GlobalPosition;
		curPos.X = (float)posX;
		GlobalPosition = curPos;
		hitbox = new Rect2(GlobalPosition, 500, 500);
	}

	private void MoveMouse()
	{
		var position = vp.GetMousePosition();

		if (!hitbox.HasPoint(position)) return;

		var leftLimit = GlobalPosition.X;

		var rightLimit = GlobalPosition.X + 500;

		var topLimit = GlobalPosition.Y;

		var bottomLimit = GlobalPosition.Y + 500;

		var offset = GlobalPosition + new Vector2(250, 250) - position;

		GD.Print("offset: " + offset);

		if (offset.Y>offset.X)
		{
			GD.Print("y>x");
			if (offset.Y < -offset.X)
			{
				position.X = rightLimit;
			}
			else
			{
				position.Y = topLimit;
			}
		}
		else
		{
			GD.Print("y<=x");
			if (offset.Y < -offset.X)
			{
				position.Y = bottomLimit;
			}
			else
			{
				position.X = leftLimit;
			}
		}

		vp.WarpMouse(position);
	}
}
