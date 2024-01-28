using Godot;
using System;
using System.Collections.Generic;

public partial class FileSpawner : Timer
{
	[Export]
	public cursor_controller Cursor { get; set; }

	[Export]
	public Godot.Collections.Array<string> TrashFilenames;

	[Export]
	public Godot.Collections.Array<string> ArchiveFilenames;

	[Export]
	public Godot.Collections.Array<string> SpamFilenames;

	[Export]
	public Godot.Collections.Array<Texture2D> FileIcons;

	[Export]
	public PackedScene fileScene;

	[Export]
	public Rect2 spawnRect { get; set; }

	public void OnTimeout()
	{
		uint randomType = GD.Randi() % 3;
		GameButton.DestType destType;
		Godot.Collections.Array<string> nameArray;

		switch (randomType)
		{
			case 0:
			default:
				destType = GameButton.DestType.Trash;
				nameArray = TrashFilenames;
				break;
			case 1:
				destType = GameButton.DestType.Archive;
				nameArray = ArchiveFilenames;
				break;
			case 2:
				destType = GameButton.DestType.Spam;
				nameArray = SpamFilenames;
				break;
		}

		GameButton newFile = (GameButton)fileScene.Instantiate();
		newFile.cursor = Cursor;

		var X = GD.RandRange(spawnRect.Position.X, spawnRect.Position.X + spawnRect.Size.X);
		var Y = GD.RandRange(spawnRect.Position.Y, spawnRect.Position.Y + spawnRect.Size.Y);

		newFile.GlobalPosition = new Vector2((float)X, (float)Y);

		string randName = nameArray[(int)(GD.Randi() % nameArray.Count)];
		newFile.SetName(randName);

		newFile.destType = destType;
		
		newFile.SetTexture(FileIcons[(int)(GD.Randi() % FileIcons.Count)]);

		GetParent().AddChild(newFile);
	}
}
