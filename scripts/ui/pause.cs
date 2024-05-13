using Godot;
using System;

public partial class pause : Control
{
	private Label _timer;
	private Control _menu;
	private TextureButton _button;

    public override void _Ready()
    {
		_timer = GetNode<Label>("Timer");
        _menu = GetNode<Control>("MenuMain");
		_button = GetNode<TextureButton>("ButtonPause");
    }

    private void Pause()
	{
		_button.Hide();
		_menu.Show();
		GetTree().Paused = true;
	}

	private void UnPause()
	{
		_menu.Hide();
		_button.Show();
		GetTree().Paused = false;
	}

	private void SetVolume(float val, String name)
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(name),Mathf.LinearToDb(val));
	}
	
	private void SetVolumeMain(float val) { SetVolume(val, "Master"); }
	private void SetVolumeUI(float val) { SetVolume(val, "SFXUI"); }
	private void SetVolumeMusick(float val) { SetVolume(val, "Music"); }
	private void SetVolumeSFX(float val) { SetVolume(val, "SFX"); }
}
