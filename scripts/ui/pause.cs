using Godot;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

public partial class pause : Control
{
	private Control _menu;
	private TextureButton _button;

    public override void _Ready()
    {
        _menu = GetNode<Control>("Menu");
		_button = GetNode<TextureButton>("ButtonPause");
    }

    private void Pause()
	{
		_button.Hide();
		_menu.Show();
		GetTree().Paused = !GetTree().Paused;
	}

	private void UnPause()
	{
		GetTree().Paused = !GetTree().Paused;
		GD.Print("nevim");
		_menu.Hide();
		_button.Show();
	}
}
