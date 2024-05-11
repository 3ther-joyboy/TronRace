using Godot;
using System;

public partial class pause : Control
{
	private Control _menu;

    public override void _Ready()
    {
        _menu = GetNode<Control>("Menu");
    }

    private void Pause()
	{
		GetTree().Paused = !GetTree().Paused;
		Hide();
		_menu.Show();
	}
}
