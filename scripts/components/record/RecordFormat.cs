using Godot;
using System;

public class RecordFormat
{
	public int time = 0;
	public Vector2 dir = Vector2.Zero;

	public RecordFormat(int time, Vector2 dir) {
		this.time = time; 
		// if something dont work, remove the .Normalized() part, tho it should work
		this.dir = dir.Normalized();
	}
}
