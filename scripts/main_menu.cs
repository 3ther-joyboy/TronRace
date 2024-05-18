using Godot;
using System;

public partial class main_menu : Control
{
	private PanelContainer _menu;
	private PanelContainer _official;
	private PanelContainer _community;
	private PanelContainer _settings;

	public override void _Ready()
	{
		_menu = GetNode<PanelContainer>("VBoxContainer/Main");
		_official = GetNode<PanelContainer>("VBoxContainer/Official");
		_community = GetNode<PanelContainer>("VBoxContainer/Community");
		_settings = GetNode<PanelContainer>("VBoxContainer/Settings");
	}

	private void ShowOfficial() { _menu.Hide(); _official.Show(); }
	private void ShowCommunity() { _menu.Hide(); _community.Show(); }


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey) {
			if (eventKey.Keycode == (Key)4194338) // f7 tho idk how to acces Key. object
				ShowEditor();
		}
	}

	private void ShowEditor()
	{
		_menu.Hide();
		GetTree().ChangeSceneToFile("res://scenes/lvl_editor/lvl_editor.tscn");
	}

	// tmp function for starting level
	private void SoloPlay(string name)
	{
		replay_handler.lastPlayedMap = name;
		//lukaš  ^^
		//franta vv
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
