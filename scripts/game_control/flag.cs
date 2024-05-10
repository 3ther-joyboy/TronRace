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
		bool placeholder = _touched;
		_touched = true;
		if (!placeholder && collider.HasNode("Player"))
			par.Count();
	}
	public void Restart(){
		_touched = false;
	}
}
