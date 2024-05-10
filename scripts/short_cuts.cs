using Godot;
using System;

public partial class short_cuts : Node
{
	public override void _UnhandledInput(InputEvent @event){
		if (@event is InputEventKey eventKey && eventKey.Pressed){
			if (eventKey.Keycode == (Key)4194336) // f5 tho idk how to acces Key. object
				GetTree().ReloadCurrentScene();
		}
	}
}
