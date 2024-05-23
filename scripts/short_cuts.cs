using Godot;
using System;

public partial class short_cuts : Node
{
	public override void _Input(InputEvent @event){

		if (@event is InputEventKey eventKey) {
			if (eventKey.Keycode == (Key)4194336) // f5 tho idk how to acces Key. object
				_Restart();
		}

		if (@event is InputEventScreenTouch eventTouch) {
			if (eventTouch.Index == 2) // starts from 0
				_Restart();
			
		}
	}

	private void _Restart() {
		GetTree().ReloadCurrentScene();
	}
}
