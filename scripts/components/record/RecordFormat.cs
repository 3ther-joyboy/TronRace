using Godot;
using System;

public class RecordFormat
{
	public int time = 0;
	public Vector2 dir = Vector2.Zero;

	public RecordFormat(int time, Vector2 dir) {
		this.time = time; 
		this.dir = dir.Normalized();
	}
}
