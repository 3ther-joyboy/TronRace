using Godot;
using System;

public partial class push_feald_component : Area2D
{
	[Export]
	private bool _toPoint = true;
	[Export]
	private int _pushForce = 1000;
	[Export] bool _sight = true;

	private Vector2 _point = Vector2.Zero;
	private RayCast2D _ray;

	public override void _Ready(){
		if (HasNode("Marker2D"))
			_point = GetNode<Marker2D>("Marker2D").Position;
		_ray = GetNode<RayCast2D>("Ray");
	}

	public override void _PhysicsProcess(double delta){
		var push = this.GetOverlappingBodies();
		for (int i = 0; i < push.Count; i++)
			if (push[i].IsClass("RigidBody2D")) {
				RigidBody2D obj = (RigidBody2D)push[i];
				if (_sight) {
					_ray.TargetPosition = obj.GlobalPosition - this.GlobalPosition;
					if (_ray.GetCollider() == obj)
						obj.ApplyCentralForce( _pushForce * _GetDirection( push[i] ) * (float)delta);
				} else
					obj.ApplyCentralForce( _pushForce * _GetDirection( push[i] ) * (float)delta);
			}
	}

	private Vector2 _GetDirection(Node2D obj) {
		return (_toPoint ? obj.GlobalPosition : this.GlobalPosition ).DirectionTo(this.GlobalPosition + _point);
		// its clean... i knonw you cant read that.. but its clean
	}
}
