using Godot;
using System;

public partial class moving_camera : Camera2D
{
	[Export(PropertyHint.Range,"0,1000")]
	private int _speed = 100;
	[Export(PropertyHint.Range,"0,1")]
	private float _offset = 0.2f;
	[Export(PropertyHint.Range,"0,2,or_greater")]
	private float _zoom = 0.5f;

	private move_component _follower;

	private Vector2 _currentOffset = Vector2.Zero;

	public override void _Ready(){
		_follower = GetParent<move_component>();
	}

	public override void _PhysicsProcess(double delta) {
		GlobalPosition = _follower.GlobalPosition + _currentOffset;
		_currentOffset += _currentOffset.DirectionTo( _follower.LinearVelocity * _offset ) * _speed * (float)delta;

		this.Zoom = Vector2.One - ( Vector2.One * _currentOffset.Length()/_follower.GetAcceleration() ) * _zoom;
	}
}
