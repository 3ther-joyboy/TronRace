using Godot;
using System;

public partial class replay : Node
{

	[Export(PropertyHint.Enum, "Record,Play Back")]
	private int Mode = 1;
	[Export]
	private bool print = false;

	private move target;
	// true -> move.direction  false -> Rotation
	private bool recordMode;

	public RecordFormat[] recording = {new RecordFormat(0,Vector2.Zero) };

	// variables
	private Vector2 currentDir = Vector2.Zero;
	private int timeOfDir = 1;

	public override void _Ready() {
		target = GetParent<move>();		
		recordMode = !target.GetRotationMode();
	}

	public override void _PhysicsProcess(double delta) {
		
		if (currentDir != GetRotation() ) {
			// GD.Print(currentDir + " || " + GetRotation() + " -> " + timeOfDir);
			Array.Resize(ref recording,recording.Length + 1);
			recording[recording.Length - 1] = new RecordFormat(timeOfDir,currentDir);

			timeOfDir = 1;
		} else
			timeOfDir++;

		currentDir = GetRotation();

		if (print){
			String randomString = "";
			for (int i = 0; i < recording.Length; i++)
				randomString += recording[i].time + " " + recording[i].dir + ", ";
			GD.Print(randomString + timeOfDir + " " + currentDir);
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
