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
		AddChild(music);
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/audio/music/menu.mp3");
		music.Play();

		sounds = new AudioStreamPlayer();
		AddChild(sounds);
	}
}
