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

	// variables
	private RecordFormat _current = new RecordFormat(0,0,false);

	public override void _Ready() {
		target = GetParent<move_component>();		

		recording = GetNode<autoload>("/root/Autoload").GetReplay();
		playBackMode = recording == null;
	}

	public override void _PhysicsProcess(double delta) {
		if (playBackMode)
			_Record();
		else
			if (recording.Length > _playBackTime)
				_Play();
	}

	private void _Play() {
		recording[_playBackTime].time--;
		if (recording[_playBackTime].time <= 0) 
			_playBackTime++;

		RecordFormat current = recording[_playBackTime];

		target.MovingStateSet(current.active);

		if (current.active) {
			GD.Print("for: " + current.time + "\t" + Mathf.RadToDeg(current.rotation) + "°");
			target.Rotation = current.rotation;
			target.AngularVelocity = 0f;
		}else{
			GD.Print("for: " + current.time + "\twaits");
		}


	}

	private void _Record() {
		if (_current.active != target.MovingStateGet() || (target.MovingStateGet() && _current.rotation != target.Rotation)) {

			Array.Resize(ref recording,recording.Length + 1);
			recording[recording.Length - 1] = _current;

			_current = new RecordFormat(1, target.Rotation, target.MovingStateGet());
		}
		_current.time++;
	}

	public RecordFormat[] GetRecording(){
		return recording;
	}
	public int Length(){
		int recordedTicks = 0;
		for (int i = 0; i < recording.Length; i++)
			recordedTicks += recording[i].time;
		return recordedTicks;
	}
}
