using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public partial class autoload : Node
{
	// frantovo space
	public static float last_time;

	// lukasovo space
	private String path = "user://replay_test.json";

	public override void _Ready(){

	}

	public RecordFormat[] GetReplay() {
		String replayJson = FileAccess.Open(path, FileAccess.ModeFlags.Read).GetAsText();

		JsonNode jsonNodeReplay = JsonNode.Parse(replayJson)!;
		// for some unknow reason i cant use length or any of these sorts of funcitons.... SO....
		// or idk, i am too lazy to put it back

		RecordFormat[] replay = {};
		for (int i = 0; true; i++) {
			try {
				var test = jsonNodeReplay[i];
			}
			catch (Exception e) {
				break;
			}
			Array.Resize(ref replay,i + 1);
			replay[i] = new RecordFormat(jsonNodeReplay[i]);
		}
		return replay;
	}
	public void SaveReplay(RecordFormat[] replay) {
		GD.Print("Saving Replay");
		// :c its working, but the class is not "fency" (i had to remove arrays from "RecordFormat" that i used for "Vector2"s)
		String jsonString = JsonSerializer.Serialize(replay);

		using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		file.StoreString(jsonString);

		GD.Print("Replay Saved");
	}
}
