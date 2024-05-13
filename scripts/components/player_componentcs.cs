using Godot;
using System;

public partial class player_componentcs : Node
{
	[Export]
	bool drawSircle = false;

	private bool _firstTap = true; 

	private Vector2 start = new Vector2(0,0);
	private Vector2 current = new Vector2(0,0);
	private Vector2 dir = Vector2.Right;

	private bool _fixedRotation = false;

	public override void _Ready(){
		_fixedRotation = ((move_component)GetParent()).GetRotationMode();
	}
	public override void _PhysicsProcess(double delta){

		if(Input.IsActionJustPressed("tap")) start = GetViewport().GetMousePosition();		
		if(Input.IsActionPressed("tap")) current = GetViewport().GetMousePosition();		
		if(Input.IsActionJustReleased("tap")) start = current = Vector2.Zero;
		Vector2 other = Input.GetVector("left","right","up","down");

		dir = start.DirectionTo(current);

		if (drawSircle) {
			if (Input.IsActionPressed("tap"))
				DrawDrag();
			else
				GetNode<CanvasLayer>("drag").Visible = false;
		}

		if (other != Vector2.Zero) 
			dir = other;

		GetParent().Call("MovingStateSet",Input.IsActionPressed("tap") || dir != Vector2.Zero);
		if (Input.IsActionPressed("tap") || other != Vector2.Zero) {
			if (_fixedRotation) {
				GetParent().Call("DirectionSet",dir.Angle());	
				GetParent().GetNode<Node2D>("visual").Call("RotateDir",dir);
			} else
				GetParent<Node2D>().Rotation = dir.Angle();    

		}
	}

	private void DrawDrag() {
		var node = GetNode<CanvasLayer>("drag");
		node.Visible = true;
		Vector2 offset = new Vector2(-20,-20);
		node.GetNode<Panel>("start").Position = start + offset;
		node.GetNode<Panel>("start").GetNode<Label>("Label").Text = (int)(Mathf.RadToDeg(dir.Angle() )) + "°";
		node.GetNode<Panel>("cur").Position = current + offset;
	}
}
