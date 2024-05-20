using Godot;
using System;

public partial class main_menu : Control
{
	private PanelContainer _menu;
	private PanelContainer _official;
	private PanelContainer _community;
	private PanelContainer _settings;
	private PanelContainer _replay;

	public override void _Ready()
	{
		_menu = GetNode<PanelContainer>("VBoxContainer/Main");
		_official = GetNode<PanelContainer>("VBoxContainer/Official");
		_community = GetNode<PanelContainer>("VBoxContainer/Community");
		_settings = GetNode<PanelContainer>("VBoxContainer/Settings");
		_replay = GetNode<PanelContainer>("VBoxContainer/Replas");


	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey) {
			if (eventKey.Keycode == (Key)4194338) // f7 tho idk how to acces Key. object
				ShowEditor();
			if (eventKey.Keycode == (Key)4194332) // f1
				_ShowReplays();
		}
	}



	private void ShowCommunity() { _menu.Hide(); _community.Show(); }


	private void ShowOfficial() { 

		var list = GetNode<ItemList>("VBoxContainer/Official/VBoxContainer/ItemList");
		list.Clear();
		var texture = new PlaceholderTexture2D();
		texture.Size = Vector2.Zero;

		using var dir = DirAccess.Open("res://scenes/maps/");
		String[] replays = dir.GetFiles();
		replays = dir.GetFiles();

		for (int i = 0; i < replays.Length; i++)
			list.AddItem(replays[i].Remove(replays[i].Length - ".tscn".Length), texture, true);

		_menu.Hide(); _official.Show(); 
	}

	private void _PlayMap(int index) {

		String name = _official.GetNode<ItemList>("VBoxContainer/ItemList").GetItemText(index);

		GetTree().Paused = true;

		replay_handler.lastPlayedMap = name;
		GetTree().ChangeSceneToFile("res://scenes/maps/" + name + ".tscn");
	}

	private void _PlayReplay(int index) {
		String what = _replay.GetNode<ItemList>("VBoxContainer/ItemList").GetItemText(index);
		GetTree().Root.GetNode<replay_handler>("ReplayHandler").Play(what);
	}
	private void _ShowReplays(){
// lukaš moment
		var list = _replay.GetNode<ItemList>("VBoxContainer/ItemList");
		list.Clear();
		var texture = new PlaceholderTexture2D();
		texture.Size = Vector2.Zero;

		using var dir = DirAccess.Open(replay_handler._path + "personal_bests/");
		String[] replays = dir.GetFiles();

		// get best times
		for (int i = 0; i < replays.Length; i++)
			list.AddItem("personal_bests/" + replays[i].Remove(replays[i].Length - replay_handler._filenameExtension.Length), texture, true);

		// others
		dir.ChangeDir("..");
		replays = dir.GetFiles();

		for (int i = 0; i < replays.Length; i++)
			list.AddItem(replays[i].Remove(replays[i].Length - replay_handler._filenameExtension.Length), texture, true);

		_menu.Hide();
		_replay.Show();
	}

	private void ShowEditor()
	{
		_menu.Hide();
		GetTree().ChangeSceneToFile("res://scenes/lvl_editor/lvl_editor.tscn");
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
		_replay.Hide();

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
