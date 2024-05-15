using Godot;
using System;

public partial class end : Area2D
{
	[ExportCategory("Audio")]
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3 beep;
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3 finish;

	int countDown = 3;

	CanvasLayer gui;
	Timer timer;
	Label label;
	AudioStreamPlayer audioPlayer;

	public override void _Ready(){
		audioPlayer = GetNode<AudioStreamPlayer>("Beep");
		gui = GetNode<CanvasLayer>("Start");
		timer = gui.GetNode<Timer>("countdown");
		label = gui.GetNode("Control").GetNode<Label>("Label");

		gui.Visible = true;

		timer.Start();
		label.Text = "" + countDown;

		GetTree().Paused = true;

		audioPlayer.Stream = beep;
		audioPlayer.Playing = true;
	}


	public override void _Process(double delta){
		label.Scale = Vector2.One * (float)(1 +timer.TimeLeft % 1);
	}
	private void Start(){
		countDown--;
		label.Text = "" + countDown;

		if (countDown <= 0) {
			audioPlayer.Stream = finish;
			gui.QueueFree();
			GetTree().Paused = false;
			this.SetProcess(false);
		} else {
			timer.Start();
		}

		audioPlayer.Playing = false;
		audioPlayer.Playing = true;
	}

	private void Entered(Node2D player) {
		if (player.HasNode("Player") ) {
			if (HasNode("../flag control") ) {
				if (GetNode<flag_control>("../flag control").Complete()) Finish(player);
				else GD.PrintRich("[pulse] nu - uh [/pulse]");
			}
			
			else Finish(player);
		}
	}
	private void Finish(Node player) {

		if (player.HasNode("Replay") ) {
			GetNode<autoload>("/root/Autoload").SaveReplay(player.GetNode<replay_component>("Replay").recording);

			int ticksPassed = player.GetNode<replay_component>("Replay").Length();
			float tickPerSecond = (float)ProjectSettings.GetSetting("physics/common/physics_ticks_per_second");
			GD.Print("Finished in: " + (ticksPassed / tickPerSecond) + " S, (" + ticksPassed + " ticks, " + tickPerSecond + " T/S)" ); 

			autoload.last_time = ticksPassed / tickPerSecond;
		} else
			GD.Print("Finished"); 
	} 
}
