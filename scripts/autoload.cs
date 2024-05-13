using Godot;
using System;
using System.Text.Json;

public partial class autoload : Node
{
	private String path = "res://replay_test.json";
	public String replayJson = FileAccess.Open("res://replay_test.json", FileAccess.ModeFlags.Read).GetAsText();

	public override void _Ready(){

	}

	public RecordFormat[] GetReplay() {

//		RecordFormat[] kys = JsonSerializer.Deserialize<RecordFormat[]>(replayJson);
//		GD.Print(kys[0].time);

		return null;
	}
	public void SaveReplay(RecordFormat[] replay) {
		GD.Print("Saving Replay");

		using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		String objs = "";
		for (int i = 0; i < replay.Length; i++)
			objs += "{\"time\":" + replay[i].time + ", \"dir\": [" + replay[i].dir.X + "," + replay[i].dir.Y + "]},";
		file.StoreString("[" + objs + "]");

		GD.Print("Replay Saved");


	}
}
