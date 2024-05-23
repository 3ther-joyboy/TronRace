using Godot;
using System;

public partial class flag : Area2D
{
	private flag_control par;
	private bool _touched = false;	
	public Color blink = new Color(1,0,0);

	public override void _Ready(){
		par = GetParent<flag_control>();
	}

	private void BodyColl(Node2D collider) {
		bool placeholder = _touched;
		_touched = true;
		changeCollor();
		if (!placeholder && collider.HasNode("Player"))
			par.Count();
	}
	public void Restart(){
		_touched = false;
	}
	public void changeCollor(){
		GD.Print("debug msg");

		if (this.HasNode("visual") && blink != null)
			GetNode<visual_component>("visual").ToggleColor(blink);
	}
}
