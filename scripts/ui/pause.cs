using Godot;
using System;

public partial class pause : Control
{
	private Control _menu;
	private TextureButton _button;

    public override void _Ready()
    {
        _menu = GetNode<Control>("MenuMain");
		_button = GetNode<TextureButton>("ButtonPause");
		if (!replay_component.playBackMode)
			GetNode<Label>("Replay").Show();

		var audio = GetNode("MenuMain/VBoxContainer/Audio/VBoxContainer");
		audio.GetNode<HSlider>("Master").Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb( AudioServer.GetBusIndex("Master") ));
		audio.GetNode<HSlider>("Music").Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb( AudioServer.GetBusIndex("Music") ));
		audio.GetNode<HSlider>("SFX").Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb( AudioServer.GetBusIndex("SFX") ));
    }

    private void Pause()
	{
		if (!GetTree().Paused)
		{
			_button.Hide();
			_menu.Show();
			GetTree().Paused = true;
		}
	}

	private void UnPause()
	{
		_menu.Hide();
		_button.Show();
		GetTree().Paused = false;
	}

	private void Restart()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	private void GoHome()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void SetVolume(float val, String name)
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(name),Mathf.LinearToDb(val));
	}
	
	private void SetVolumeMaster(float val) { SetVolume(val, "Master"); }
	private void SetVolumeMusic(float val) { SetVolume(val, "Music"); }
	private void SetVolumeSFX(float val) { SetVolume(val, "SFX"); }



// lukaš test, smazat

	private void PlayBackTest(){
		GetTree().Root.GetNode<replay_handler>("ReplayHandler").Play();
	}
}
