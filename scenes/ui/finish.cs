using Godot;
using System;

public partial class finish : Control
{
	private RecordFormat[] _current = replay_handler.bufferReplay;	
	private float _tps = (float)ProjectSettings.GetSetting("physics/common/physics_ticks_per_second",2);


	private LineEdit _input;
	private Button _save;

	public override void _Ready(){

		_input = GetNode<LineEdit>("VBoxContainer/Options/VBoxContainer/Name");
		_save = GetNode<Button>("VBoxContainer/Options/VBoxContainer/Save");

		int pb = 0;	
		try {
			pb = replay_handler.GetReplay().Length ;
		}
		catch {}

		var PBlabel = GetNode<Label>("VBoxContainer/Time/FromPB");
		if (pb < _current.Length && pb != 0) {
			PBlabel.Text = "+";
			PBlabel.LabelSettings.FontColor = new Color(1f,0f,0f);
		} else if (pb > _current.Length && pb != 0) {
			PBlabel.Text = "-";
			PBlabel.LabelSettings.FontColor = new Color(0f,0f,1f);
		}
		PBlabel.Text += " " + (Math.Abs((float)(_current.Length - pb)) / _tps) + " S";


		GetNode<Label>("VBoxContainer/Time/Time").Text = Math.Floor(_current.Length / (_tps * 60)) + " : " + Math.Floor(_current.Length / _tps) + " . " + _current.Length % _tps ;

		if (pb == _current.Length || pb == 0) { 
			PBlabel.Text = "";
		}

		// auto save run last
		replay_handler.AutoSave();
	}
	private void _NameInput(String name) {
		bool now = false;
		String badCharacters = " \"\'\\./°#&@{}=|?:%*<>";
		for (int i = 0; i < name.Length; i++)
			for (int y = 0; y < badCharacters.Length; y++)
				if (name[i] == badCharacters[y])
					now = true;
		if (name == "")
			now = true;
		_save.Disabled = now;
	}
	private void _PlayBuffer() {
		GetTree().Root.GetNode<replay_handler>("ReplayHandler").PlayBuffer();
	}
	private void _Save() {
		replay_handler.SaveReplay(_input.Text);
	}
	private void _Menu() {
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
	private void _Restart() {
		GetTree().ChangeSceneToFile("res://scenes/maps/" + replay_handler.lastPlayedMap + ".tscn");
	}
}
