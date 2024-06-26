using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public partial class replay_handler : Node
{
	// frantovo space
	public static float last_time;

	// lukasovo space
	public static String _path = "user://replays/";
	public static String _filenameExtension = ".json";

	public static String lastPlayedMap = "";
	private static bool _replayRedy = false;

	public static RecordFormat[] bufferReplay;

	public override void _Ready(){
		DirAccess.MakeDirAbsolute(_path);
		DirAccess.MakeDirAbsolute(_path + "personal_bests/");

		String[] dirs = main_menu.dirs;
		for (int i = 0; i < dirs.Length; i++)
			DirAccess.MakeDirAbsolute(_path + "personal_bests/" + dirs[i] + "/");
		for (int i = 1; i < dirs.Length; i++) {
			using var file = FileAccess.Open(_path + dirs[i], FileAccess.ModeFlags.Write);
			file.StoreString("uwu");
		}
		mapUnlocak();
	}

	private static void mapUnlocak() {
		for (int dirsectories = 1; dirsectories < main_menu.dirs.Length; dirsectories++) {
			String[] jsonReplays = DirAccess.GetFilesAt(_path + "personal_bests/" + main_menu.dirs[dirsectories - 1] + "/");
			int count = 0;
			for (int files = 0; files < main_menu.maps.GetLength(1); files++){
				if (main_menu.maps[dirsectories - 1,files] != null)
					count++;

			}
			if (jsonReplays.Length >= count) {
				try {DirAccess.RemoveAbsolute(_path + main_menu.dirs[dirsectories]);}
				catch{}
			}
		}
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

	public void Play() {
		GD.Print("personal_bests/" + lastPlayedMap);
		Play("personal_bests/" + lastPlayedMap);
	}
	public void Play(String name) {
		_replayRedy = true;
		JsonNode rep = GetJson(name);
		lastPlayedMap = (String)(rep["map"]);
		bufferReplay = GetReplay(name);

		GD.Print();
		GD.Print("User: \t\t" + (String)rep["user"]);
		GD.Print("User ID: \t" + (int)rep["user_id"]);
		GD.Print("Map: \t\t" + lastPlayedMap);
		GD.Print();
		GD.Print("Time: \t\t" + ((float)rep["ticks"] / (float)rep["tps"]));
		GD.Print("Ticks: \t\t" + (int)rep["ticks"]);
		GD.Print("TPS: \t\t" + (float)rep["tps"]);
		GD.Print();
		GD.Print("Date: \t\t" + rep["date"].ToString());
		GD.Print();


		GetTree().ChangeSceneToFile("res://scenes/maps/" + lastPlayedMap + ".tscn");
	}

	public static JsonNode GetJson(String path) {
		GD.Print(_path + path + _filenameExtension);
		if (FileAccess.FileExists(_path + path + _filenameExtension)) {
			string replayJson = FileAccess.Open(_path + path + _filenameExtension, FileAccess.ModeFlags.Read).GetAsText();
			JsonNode jsonNodeReplay = JsonNode.Parse(replayJson)!;
			return jsonNodeReplay;
		} else {
			GD.Print("Replay does not exist");
			return JsonNode.Parse("");
		}
	}

	public static RecordFormat[] GetReplay() {
		return GetReplay("personal_bests/" + lastPlayedMap);
	}
	public static RecordFormat[] GetReplay(String path) {
		JsonNode jsonNodeReplay = GetJson(path);;
		// for some unknow reason i cant use length or any of these sorts of funcitons.... SO....
		// or idk, i am too lazy to put it back

		RecordFormat[] replay = {};
		for (int i = 0; true; i++) {
			try {
				var test = jsonNodeReplay["replay"][i];
			}
			catch {break;}
			Array.Resize(ref replay,i + 1);
			replay[i] = new RecordFormat(jsonNodeReplay["replay"][i]);
		}
		return replay;
	}
	public static void AutoSave() {
		if ((FileAccess.FileExists(_path + "personal_bests/" + lastPlayedMap + ".json") && GetReplay().Length > bufferReplay.Length) || !FileAccess.FileExists(_path + "personal_bests/" + lastPlayedMap + ".json")) {
			GD.Print("New PB!");
			GD.Print("AutoSave");
			SaveReplay("personal_bests/" + lastPlayedMap);
			mapUnlocak();
		}

	}
	public static void SaveReplay() {
		SaveReplay(lastPlayedMap);
	}
	public static void SaveReplay(String path) {
		GD.Print("Saving Replay");
		// :c its working, but the class is not "fency" (i had to remove arrays from "RecordFormat" that i used for "Vector2"s)
		String jsonString = JsonSerializer.Serialize(new {replay = bufferReplay, user = server.user_name, user_id = server.user_id, map = lastPlayedMap, date = DateTime.UtcNow.ToString(), tps = (int)ProjectSettings.GetSetting("physics/common/physics_ticks_per_second"), ticks = bufferReplay.Length});

		using var file = FileAccess.Open(_path + path + _filenameExtension, FileAccess.ModeFlags.Write);
		file.StoreString(jsonString);

		GD.Print("Replay Saved");
	}
}
