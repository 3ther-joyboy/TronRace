using Godot;
using System;

public partial class replay_component : Node
{
	private bool _mode = false;

	private move_component target;

	public RecordFormat[] recording = {};
	// true = record, false = playback
	public bool playBackMode = true;
	private int _playBackTime = 0;

	public override void _Ready() {
		recording = new RecordFormat[]{};
		playBackMode = true;
		target = GetParent<move_component>();		

		if (replay_handler.TryReplay())
			PlayBack();			
	}

	public void PlayBack() {
		recording = replay_handler.bufferReplay;
		playBackMode = false;
		
		if (target.HasNode("Player"))
			target.GetNode("Player").SetScript("");
	}

	public override void _PhysicsProcess(double delta) {
		if (playBackMode)
			_Record();
		else
			if (recording.Length > _playBackTime + 1)
				_Play();
			else
				GD.Print("end of replay");

	}

	private void _Play() {
		_playBackTime++;
		target.MovingStateSet(recording[_playBackTime].active);

		target.Rotation = recording[_playBackTime].rotation;
		target.AngularVelocity = recording[_playBackTime].rotationVel;

		target.Position = new Vector2(recording[_playBackTime].positionX,recording[_playBackTime].positionY);

		target.LinearVelocity = new Vector2(recording[_playBackTime].velocityX,recording[_playBackTime].velocityY);
	}

	private void _Record() {
			Array.Resize(ref recording,recording.Length + 1);
			recording[recording.Length - 1] = new RecordFormat(target.MovingStateGet(),target.Rotation,target.AngularVelocity,target.Position.X,target.Position.Y,target.LinearVelocity.X,target.LinearVelocity.Y);
	}

	public RecordFormat[] GetRecording(){
		return recording;
	}
	public int Length(){
		return recording.Length;
	}
}
