using Godot;
using System;
using System.Xml;

public partial class main_menu : Control
{
	private PanelContainer _menu;
	private PanelContainer _official;
	private PanelContainer _community;
	private PanelContainer _editor;
	private PanelContainer _settings;

	public override void _Ready()
	{
		_menu = GetNode<PanelContainer>("VBoxContainer/Main");
		_official = GetNode<PanelContainer>("VBoxContainer/Official");
		_community = GetNode<PanelContainer>("VBoxContainer/Community");
		_editor = GetNode<PanelContainer>("VBoxContainer/Editor");
		_settings = GetNode<PanelContainer>("VBoxContainer/Settings");
	}

	private void ShowOfficial() { _menu.Hide(); _official.Show(); }
	private void ShowCommunity() { _menu.Hide(); _community.Show(); }
	private void ShowEditor()
	{
		_menu.Hide();
		_editor.Show();
		GetTree().ChangeSceneToFile("res://scenes/lvl_editor/lvl_editor.tscn");
	}

	// tmp function for starting level
	private void SoloPlay(string name)
	{
		GetTree().ChangeSceneToFile("res://scenes/maps/" + name + ".tscn");
	}

	private void ShowSettings()
	{
		_menu.Hide();
		GetNode<LineEdit>("VBoxContainer/Settings/VBoxContainer/LineEdit").Text = server.user_name;
		GetNode<LineEdit>("VBoxContainer/Settings/VBoxContainer/LineEdit2").Text = server.custom_ip;
		_settings.Show();
	}

	private void ServerOptionSelected(int index)
	{
		if (index == 1) GetNode<LineEdit>("VBoxContainer/Settings/VBoxContainer/LineEdit2").Show();
		else GetNode<LineEdit>("VBoxContainer/Settings/VBoxContainer/LineEdit2").Hide();
	}

	private void Back()
	{
		_official.Hide();
		_community.Hide();
		_editor.Hide();
		_settings.Hide();

		_menu.Show();
	}

	private void BackSettings()
	{
		string input = GetNode<LineEdit>("VBoxContainer/Settings/VBoxContainer/LineEdit").Text;

		if (input != "")
		{
			server.user_name = input;
			server.conf.SetValue("Player", "username", input);
			server.conf.SetValue("Server", "ip", GetNode<LineEdit>("VBoxContainer/Settings/VBoxContainer/LineEdit2").Text);
			server.conf.Save("user://config.ini");
			Back();
		} 

		else GD.Print("invalid input!");
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}
}
