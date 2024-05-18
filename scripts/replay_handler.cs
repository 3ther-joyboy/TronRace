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
	private static bool _replayRedy = false;

	public static RecordFormat[] bufferReplay;


	public override void _Ready(){
		DirAccess.MakeDirAbsolute(_path);
		DirAccess.MakeDirAbsolute(_path + "personal_bests/");
	}

	public void Play() {
		Play(lastPlayedMap);
	}

	public static bool TryReplay(){

		if (_replayRedy) {
			_replayRedy = false;
			return true;
		} else
			return false;


	}
	public void PlayBuffer() {
			_replayRedy = true;
			GetTree().ChangeSceneToFile("res://scenes/maps/" + lastPlayedMap + ".tscn");
	}
	public void Play(String name) {
		try {
			_replayRedy = true;
			lastPlayedMap = name;
			bufferReplay = GetReplay();
			GetTree().ChangeSceneToFile("res://scenes/maps/" + name + ".tscn");
		}
		catch {
			GD.Print("Map replay does not exist");
		}
	}

	public static RecordFormat[] GetReplay() {
		return GetReplay("personal_bests/" + lastPlayedMap);
	}
	public static RecordFormat[] GetReplay(String path) {
		if (FileAccess.FileExists(_path + path + _filenameExtension)) {
			String replayJson = FileAccess.Open(_path + path + _filenameExtension, FileAccess.ModeFlags.Read).GetAsText();

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
		} else {
			GD.Print("Replay does not exist");
			return new RecordFormat[]{};
		}
	}
	public static void AutoSave() {
		if (GetReplay().Length > bufferReplay.Length || !FileAccess.FileExists(_path + "personal_bests/" + lastPlayedMap + ".json")) {
			GD.Print("New PB!");
			GD.Print("AutoSave");
			SaveReplay("personal_bests/" + lastPlayedMap);
		}

	}
	public static void SaveReplay() {
		SaveReplay(lastPlayedMap);
	}
	public static void SaveReplay(String path) {
		GD.Print("Saving Replay");
		// :c its working, but the class is not "fency" (i had to remove arrays from "RecordFormat" that i used for "Vector2"s)
		String jsonString = JsonSerializer.Serialize(bufferReplay);

		using var file = FileAccess.Open(_path + path + _filenameExtension, FileAccess.ModeFlags.Write);
		file.StoreString(jsonString);

		GD.Print("Replay Saved");
	}
}
