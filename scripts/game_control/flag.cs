using Godot;
using System;

public partial class flag : Area2D
{
	private flag_control par;
	private bool _touched = false;	

	public override void _Ready(){
		par = GetParent<flag_control >();
	}

	private void BodyColl(Node2D collider) {
		if (!_touched && collider.HasNode("Player"))
			par.Count();
		_touched = true;
	}
	public void Restart(){
		GD.Print("owo");
		_touched = false;
	}
}
