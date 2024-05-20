using Godot;
using System;

public partial class restart_menu : Control
{
	private void _Restart() {
	GetTree().ChangeSceneToFile("res://scenes/maps/" + replay_handler.lastPlayedMap + ".tscn");
	}
}
