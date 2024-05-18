using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class RecordFormat
{
	public int time {get; set;}
	public float rotation {get; set;}
	public bool active {get; set;}

	public RecordFormat(JsonNode replay) {
		this.time = replay["time"].GetValue<int>(); 
		this.rotation = replay["rotation"].GetValue<float>(); 
		this.active = replay["active"].GetValue<bool>();
	}
	public RecordFormat(int time, float rotation, bool active) {
		this.time = time; 
		this.rotation = rotation;
		this.active = active;
	}
}
