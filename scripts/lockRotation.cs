using Godot;
using System;

public partial class lockRotation : Node2D
{
	[Export]
	private int _degrees = 0;

	// yes, this is wery much silly but we had to use it xD
	public override void _PhysicsProcess(double delta){
		this.GlobalRotation = Godot.Mathf.DegToRad(_degrees); 
	}
}
