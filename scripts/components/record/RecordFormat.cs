using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class RecordFormat
{
	public int time {get; set;}
	public float dirX {get; set;}
	public float dirY {get; set;}

	public RecordFormat(JsonNode replay) {
		this.time = replay["time"].GetValue<int>(); 
		this.dirX = replay["dirX"].GetValue<float>(); 
		this.dirY = replay["dirY"].GetValue<float>(); 
	}
	public RecordFormat(int time, Vector2 dir) {
		this.time = time; 
		this.dirX = dir.Normalized().X;
		this.dirY = dir.Normalized().Y;
	}
}
