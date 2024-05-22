using Godot;
using System;

public partial class path_follower_comonent : PathFollow2D
{
	[Export]
	private float _time = 1;
	private Timer _timer;

	public override void _Ready(){
		_timer = GetNode<Timer>("Timer");
		_timer.WaitTime = _time;
		_timer.OneShot = !this.Loop;
		_timer.Start();
	}
	public override void _PhysicsProcess(double delta){
		this.ProgressRatio = 1-((float)(_timer.TimeLeft / _timer.WaitTime));
	}
}
