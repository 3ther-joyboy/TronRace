using Godot;
using System;

public partial class autoload : Node
{
	public String replayJson;

	public override void _Ready(){

	}


	public RecordFormat[] GetReplay() {
		//		Variant _out = Json.ParseString(replayJson);
		//		GD.Print(_out.VariantType == Variant.Type.Nil ? "No Replay input": "Replay sucesfully loaded");
		return null;
	}
	public void SaveReplay(RecordFormat[] replay) {
		GD.Print("Saving Replay");

		using var file = FileAccess.Open("res://replay_test.json", FileAccess.ModeFlags.Write);
		String objs = "";
		for (int i = 0; i < replay.Length; i++)
			objs += "\"time\":" + replay[i].time + ", \"dir\":[" + replay[i].dir.X + ", " + replay[i].dir.Y + "],\n";
		file.StoreLine("{[ \n" + objs + "]}");
	}
}
