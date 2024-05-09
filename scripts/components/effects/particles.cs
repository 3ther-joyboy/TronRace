using Godot;
using System;

public partial class particles : GpuParticles2D
{
	public override void _PhysicsProcess(double delta){
		this.Emitting = GetParent().GetParent<move_component>().MovingStateGet();
	}	
}
