using Godot;
using System;

public partial class moving_camera : Camera2D
{
	[Export(PropertyHint.Range,"0,10,or_grater")]
	private int _speed = 100;
	[Export(PropertyHint.Range,"0,2,or_greater")]
	private float _zoom = 1f;

	private move_component _follower;

	public override void _Ready(){
		_follower = GetParent<move_component>();
	}

	public override void _PhysicsProcess(double delta) {
		this.Position += this.Position.DirectionTo( _follower.LinearVelocity/10 ) * _speed * (float)delta;

		this.Zoom = Vector2.One - ( Vector2.One * this.Position.Length()/_follower.GetAcceleration() ) * _zoom;
	}
}
