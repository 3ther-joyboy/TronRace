using Godot;
using System;

public partial class gameMenu : Control
{

	[Export(PropertyHint.File, "*.tscn,")]
	String menuScene;
	[Export(PropertyHint.File, "*.mp3,")]
	AudioStreamMP3 menuSoundEffect;

	public override void _Ready(){
		GetNode<AudioStreamPlayer>("Audio").Stream = menuSoundEffect;
		//Bus setup

		String[] names = {"Master","SFX","SFXUI","Music"};
		var ControlAudioNode = GetNode<Control>("AudioSettings");
		for(int i = 0;i<names.Length;i++){
			// actually.. dont be dum and set sliders after BUSes
			ControlAudioNode.GetNode("Audio"+(i%2+1)).GetNode<VSlider>(names[i]).Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex(names[i])));
		}
	}
	private void Beep(){
		GetNode<AudioStreamPlayer>("Audio").Play();
	}
	// functions
	private void Toggle(String node){
		var obj = GetNode<Control>(node);
		obj.Visible = !obj.Visible;
		Beep();
	}
	// true => visible  , false => in-visible
	private void Hide(String node, bool state){
		Beep();
		GetNode<Control>(node).Visible = state;
	}
	// menu
	private void Pause(){
		if(!GetTree().Paused)
			TogglePause();
	}
	private void TogglePause(){
		Beep();
		bool paused = GetTree().Paused;
		GetTree().Paused = !paused;
		Hide("PausedMenu",!paused);
		Hide("Pause",paused);
	}

	private void Restart(){
		Beep();
		UnPause();
		GetTree().ReloadCurrentScene();
	}

	private void UnPause(){
		Beep();
		GetTree().Paused = false;
		Hide("PausedMenu",false);
		Hide("Pause",true);
	}
	private void Menu(){
		Beep();
		UnPause();
		GetTree().ChangeSceneToFile(menuScene);
	}

	private void FromSettingsToMenu(){
		Beep();
		Hide("Settings", false);
		Hide("PausedMenu",true);
	}


	// settings
	private void Settings(){
		Hide("PausedMenu",false);
		Hide("VideoSettings",false);
		Hide("AudioSettings",false);
		Hide("Settings",true);
	}
	private void ToVideoSettings(){
		Beep();
		Hide("Settings", false);
		Hide("VideoSettings", true);
	}
	private void ToAudioSettings(){
		Beep();
		Hide("Settings", false);
		Hide("AudioSettings", true);
	}

	// VideoSettings
	// enums from DOCs https://docs.godotengine.org/en/stable/classes/class_displayserver.html#enum-displayserver-windowmode
	private void FullScreeen(bool mode){
		Beep();
		if(mode){
			DisplayServer.WindowSetMode((Godot.DisplayServer.WindowMode)3,0);
		}else{
			DisplayServer.WindowSetMode((Godot.DisplayServer.WindowMode)0,0);
		}
	}
	private void BackToVideoSettings(){
	}

	private void ChangeFPS(int FPSMode){
		int fps = 60;
		switch (FPSMode)
		{
			case 0:fps=15;break; 
			case 1:fps=30;break; 
			case 2:fps=60;break; 
			case 3:fps=69;break; 
			case 4:fps=120;break; 
			case 5:fps=240;break; 
			default:break;
		}
		Engine.MaxFps = fps;
	}
	// AudioSettings
	private void Audio(float val,String name){
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(name),Mathf.LinearToDb(val));
		GD.Print("Bus "+ name +" was set to " + Mathf.LinearToDb(val) + "db");
	}
	private void AudioMain(float val){
		Audio(val,"Master");
	}
	private void AudioUI(float val){
		Audio(val,"SFXUI");
	}
	private void AudioMusick(float val){
		Audio(val,"Music");
	}
	private void AudioSFX(float val){
		Audio(val,"SFX");
	}
}
