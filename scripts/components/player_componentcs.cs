using Godot;
using System;

public partial class player_componentcs : Node
{
	[Export]
	bool drawSircle = false;
	[Export]
	int _maxDragLenght = 100;

	private bool _firstTap = true; 

	private Vector2 start = new Vector2(0,0);
	private Vector2 current = new Vector2(0,0);
	private Vector2 dir = Vector2.Right;

	public override void _Ready() {}

	public override void _PhysicsProcess(double delta) {

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
		if ((Input.IsActionPressed("tap") && (start - current).Length() > 10) || other != Vector2.Zero) {

			GetParent<Node2D>().Rotation = dir.Angle();    
			GetParent<RigidBody2D>().AngularVelocity = 0f;

		}
	}

	private void DrawDrag() {
		// sory Franto za hnusně pojemnovane proměne
		var node = GetNode<CanvasLayer>("drag");
		node.Visible = true;
		Vector2 offset = new Vector2(-20,-20);
		Panel _target = node.GetNode<Panel>("start");
	 	_target.Position = start + offset;
		node.GetNode<Panel>("start").GetNode<Label>("Label").Text = (int)(Mathf.RadToDeg(dir.Angle() )) + "°";
		Panel _target2 = node.GetNode<Panel>("cur");
		if ((start - current).Length() < _maxDragLenght)
			_target2.Position = current + offset;
		else
			_target2.Position = start.DirectionTo(current) * _maxDragLenght + offset + start;

	}
} 
