using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public partial class replay_handler  : Node
{
	// frantovo space
	public static float last_time;

	// lukasovo space
	public static String _path = "user://replays/";
	private static String _filenameExtension = ".json";

	public static String lastPlayedMap = "";


	public override void _Ready(){
		DirAccess.MakeDirAbsolute(_path);
		DirAccess.MakeDirAbsolute(_path + "personal_bests/");
	}

	public void Play() {
		Play(lastPlayedMap);
	}

	public void Play(String name) {
		try {
			lastPlayedMap = name;
			GetTree().ChangeSceneToFile("res://scenes/maps/" + name + ".tscn");
			replay_component.PlayBack("personal_bests/" + name);
		}
		catch {
			GD.Print("Map replay does not exist");
		}
	}

	public static RecordFormat[] GetReplay() {
		return GetReplay(lastPlayedMap);
	}
	public static RecordFormat[] GetReplay(String map_name) {
		String replayJson = FileAccess.Open(_path + map_name + _filenameExtension, FileAccess.ModeFlags.Read).GetAsText();

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
	public static void AutoSave(RecordFormat[] replay) {
		if (GetReplay().Length > replay.Length) {
			GD.Print("New PB!");
			GD.Print("AutoSave");
			SaveReplay(replay,"personal_bests/" + lastPlayedMap);
		}

	}
	public static void SaveReplay(RecordFormat[] replay) {
		SaveReplay(replay, lastPlayedMap );
	}
	public static void SaveReplay(RecordFormat[] replay, String path) {
		GD.Print("Saving Replay");
		// :c its working, but the class is not "fency" (i had to remove arrays from "RecordFormat" that i used for "Vector2"s)
		String jsonString = JsonSerializer.Serialize(replay);

		using var file = FileAccess.Open(_path + path + _filenameExtension, FileAccess.ModeFlags.Write);
		file.StoreString(jsonString);

		GD.Print("Replay Saved");
	}
}
