using Godot;
using System;

public partial class audio_player : Node
{
	public static AudioStreamPlayer music;
	public static AudioStreamPlayer sounds;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		music = new AudioStreamPlayer();
		music.ProcessMode = ProcessModeEnum.Always;
		AddChild(music);
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/audio/music/menu.mp3");
		music.Play();

		sounds = new AudioStreamPlayer();
		sounds.ProcessMode = ProcessModeEnum.Always;
		audio_player.sounds.Stream = ResourceLoader.Load<AudioStream>("res://assets/audio/click.mp3");
		AddChild(sounds);
	}
}
