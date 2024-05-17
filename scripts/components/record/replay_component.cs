using Godot;
using System;

public partial class replay_component : Node
{
	private bool _mode = false;

	private move_component target;
	// true -> move.direction  false -> Rotation
	private bool recordMode;

	public RecordFormat[] recording = {};
	// true = record, false = playback
	public bool playBackMode = true;
	private int _playBackTime = 0;

	// variables
	private Vector2 currentDir = Vector2.Zero;
	private int timeOfDir = 0;

	public override void _Ready() {
		target = GetParent<move_component>();		
		recordMode = !target.GetRotationMode();

		recording = GetNode<autoload>("/root/Autoload").GetReplay();
		playBackMode = recording == null;
	}

	public override void _PhysicsProcess(double delta) {
		if (playBackMode)
			_Record();
		else
			_Play();
	}

	private void _Play() {
		if (recording.Length > _playBackTime) {
			if (recording[_playBackTime].time <= 0) 
				_playBackTime++;

			Vector2 newDir = new Vector2(recording[_playBackTime].dirX,recording[_playBackTime].dirY);
			recording[_playBackTime].time--;

			if (newDir != GetRotation())
				SetRotation(newDir);


		}
	}

	private void _Record() {
		if (currentDir != GetRotation() ) {
			Array.Resize(ref recording,recording.Length + 1);
			recording[recording.Length - 1] = new RecordFormat(timeOfDir,currentDir);

			timeOfDir = 1;
		} else
			timeOfDir++;
		currentDir = GetRotation();
	}
	private void SetRotation (Vector2 dir) {
		GD.Print(dir + " for : " + recording[_playBackTime].time);

		target.MovingStateSet(dir != Vector2.Zero);
		if (dir != Vector2.Zero) {
			target.AngularVelocity = 0f;
			if (!recordMode)
				target.DirectionSet(dir);
			else
				target.Rotation = dir.Angle();
		}

	}
	private Vector2 GetRotation() {
		if (target.MovingStateGet() )
			if (recordMode)
				return target.DirectionGet();
			else
				return Vector2.FromAngle(target.Rotation);
		else
			return Vector2.Zero;    
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
