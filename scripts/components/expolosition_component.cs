using Godot;
using System;

public partial class expolosition_component : CpuParticles2D
{
	[ExportCategory("Audio")]
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3 _sound;

	private bool _timePassed = false;


	public override void _Ready(){
		this.Emitting = true;
		var aug = GetNode<AudioStreamPlayer2D>("Audio");
		aug.Stream = _sound;
		aug.Play();
		GetNode<Timer>("Timer").Start();
	}
	private void _Force(){
		GetNode<Area2D>("PushFeald").QueueFree();
	}
	private void _TimePassed(){
		this.QueueFree();
	}

}
