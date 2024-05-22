using Godot;
using System;

public partial class audio_player : Node
{
	public static AudioStreamPlayer music;
	public static AudioStreamPlayer sounds;
	private static string[] _music_list = {
		"res://assets/audio/music/game-calm.mp3",
		"res://assets/audio/music/game-medium.mp3",
		"res://assets/audio/music/game-intensive.mp3"
	};

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		music = new AudioStreamPlayer();
		music.ProcessMode = ProcessModeEnum.Always;
		music.Bus = "Music";
		AddChild(music);
		music.Play();

		sounds = new AudioStreamPlayer();
		sounds.ProcessMode = ProcessModeEnum.Always;
		sounds.Bus = "SFX";
		audio_player.sounds.Stream = ResourceLoader.Load<AudioStream>("res://assets/audio/click.mp3");
		AddChild(sounds);
	}

	public static void SetMusic(string path)
	{
		music.Stream = ResourceLoader.Load<AudioStream>(path);
		music.Play();
	}

	public static void GetRandomMusic()
	{
		Random rnd = new Random();
		music.Stream = ResourceLoader.Load<AudioStream>(_music_list[rnd.Next(0, _music_list.Length)]);
		music.Play();
	}
}
