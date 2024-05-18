using Godot;
using System;

public partial class finish : Control
{
	private RecordFormat[] _current = replay_handler.bufferReplay;	
	private float _tps = (float)ProjectSettings.GetSetting("physics/common/physics_ticks_per_second",2);

	public override void _Ready(){
		replay_handler.AutoSave();

		int pb = replay_handler.GetReplay().Length ;	
		var PBlabel = GetNode<Label>("FromPB");
		if (pb < _current.Length) {
			PBlabel.Text = "+";
			PBlabel.LabelSettings.FontColor = new Color(1f,0f,0f);
		} else if (pb < _current.Length) {
			PBlabel.Text = "-";
			PBlabel.LabelSettings.FontColor = new Color(0f,0f,1f);
		} else { 
			PBlabel.Text = "";
			PBlabel.LabelSettings.FontColor = new Color(.3f,.3f,.3f);
		}

		GD.Print(Math.Abs((float)(_current.Length - pb)) / _tps);
		PBlabel.Text += " " + (Math.Abs((float)(_current.Length - pb)) / _tps) + " S";


		GetNode<Label>("Time/Time").Text = Math.Floor(_current.Length / (_tps * 60)) + " : " + Math.Floor(_current.Length / _tps) + " . " + _current.Length % _tps ;
	}

	private void _PlayBuffer() {
		GetTree().Root.GetNode<replay_handler>("ReplayHandler").PlayBuffer();
	}
	private void _Save() {
		replay_handler.SaveReplay();
	}
	private void _Menu() {
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
	private void _Restart() {
		GetTree().ChangeSceneToFile("res://scenes/maps/" + replay_handler.lastPlayedMap + ".tscn");
	}
}
