using Godot;
using System;
using System.Xml;

public partial class main_menu : Control
{
	private PanelContainer _menu;
	private PanelContainer _solo;
	private PanelContainer _online;
	private PanelContainer _editor;
	private PanelContainer _profile;

	public override void _Ready()
	{
		_menu = GetNode<PanelContainer>("VBoxContainer/Main");
		_solo = GetNode<PanelContainer>("VBoxContainer/Solo");
		_online = GetNode<PanelContainer>("VBoxContainer/Online");
		_editor = GetNode<PanelContainer>("VBoxContainer/Editor");
		_profile = GetNode<PanelContainer>("VBoxContainer/Profile");
	}

	private void SoloMenu()
	{
		_menu.Hide();
		_solo.Show();
	}

	private void SoloPlay(string name)
	{
		GetTree().ChangeSceneToFile("res://scenes/maps/" + name + ".tscn");
	}

	private void OnlineMenu()
	{
		_menu.Hide();
		_online.Show();
	}

	private void EditorMenu()
	{
		_menu.Hide();
		_editor.Show();
	}

	private void ProfileMenu()
	{
		_menu.Hide();
		GetNode<LineEdit>("VBoxContainer/Profile/VBoxContainer/LineEdit").Text = config.user_name;
		_profile.Show();
	}

	private void Back()
	{
		_solo.Hide();
		_online.Hide();
		_editor.Hide();
		_profile.Hide();

		_menu.Show();
	}

	private void BackProfile()
	{
		string input = GetNode<LineEdit>("VBoxContainer/Profile/VBoxContainer/LineEdit").Text;

		if (input != "")
		{
			config.user_name = input;
			config.conf.SetValue("Player", "username", input);
			Back();
		} 

		else GD.Print("invalid input!");
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}
}
