using Godot;
using System;

public partial class dead_player : Node2D
{
	private void dead(){
		GetTree().ChangeSceneToFile("res://scenes/ui/lvl_restart.tscn");
	}
}
