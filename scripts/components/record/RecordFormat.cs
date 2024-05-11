using Godot;
using System;
using System.Text.Json;

public class RecordFormat
{
	public int time = 0;
	public Vector2 dir = Vector2.Zero;

	public RecordFormat(int time, Vector2 dir) {
		this.time = time; 
		this.dir = dir.Normalized();
	}
	public RecordFormat(JsonElement json) {
		time = json.GetProperty("time").GetInt32();
		dir.X = (float) json.GetProperty("dir")[0].GetDouble();
		dir.Y = (float) json.GetProperty("dir")[1].GetDouble();
	}

}
