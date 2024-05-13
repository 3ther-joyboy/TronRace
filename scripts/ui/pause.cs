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
}
