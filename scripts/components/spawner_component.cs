using Godot;
using System;

public partial class spawner_component : Marker2D
{
	[Export]
	private PackedScene [] spawns;
	[Export(PropertyHint.Range, "0.1,10,0.1,or_greater")]
	private float _time = 1;
	private int sicle = 0;

	public override void _Ready(){
		GetNode<Timer>("Timer").WaitTime = _time;
	}
	private void _Spawn() {
		Node2D spawn = (Node2D)spawns[sicle].Instantiate();
		spawn.GlobalPosition = this.GlobalPosition;
		spawn.GlobalRotation = this.GlobalRotation;
		GetTree().Root.AddChild(spawn);
		sicle++;
		if(sicle >= spawns.Length)
			sicle = 0;
	}

}
