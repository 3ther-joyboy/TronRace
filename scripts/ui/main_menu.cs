using Godot;
using System;

public partial class main_menu : Control
{
	private PanelContainer _menu;
	private PanelContainer _official;
	private PanelContainer _community;
	private PanelContainer _settings;
	private PanelContainer _replay;


	public static String[] dirs = {"Easy","Normal","Tron","Dev"};

	public static String[,] maps = {
		{"first_steps","citty_edge","town_square"},
		{"kokot",null,null},
		{"idk",null,null},
		{"test_world",null,null}
	};

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
		var lists = GetNode<TabContainer>("VBoxContainer/Official/VBoxContainer/TabContainer");

		Godot.Collections.Array<Godot.Node> oldList = lists.GetChildren();

		for (int y = 0; y < oldList.Count; y++)
			lists.RemoveChild(oldList[y]);

		var texture = new PlaceholderTexture2D();
		texture.Size = Vector2.Zero;

		for (int i = 0; i < dirs.Length; i++){
			ItemList list = new ItemList();
			list.Name = dirs[i];
			list.CustomMinimumSize = new Vector2(0,300);

			for (int y = 0; y < maps.GetLength(1); y++) {
				try { list.AddItem(maps[i, y], texture, true);}
				catch {}
			}

			list.ItemSelected += (index) => _PlayMap((int)index ,list.Name);

			lists.AddChild(list);
		}
		for (int i = 0; i < dirs.Length; i++)
			if ((DirAccess.Open(replay_handler._path)).FileExists(dirs[i])) {
				if (dirs[i] == "Dev") 
					lists.SetTabHidden(i,true);
				lists.SetTabDisabled(i,true);
			}


		_menu.Hide(); _official.Show(); 
	}

	private void _PlayMap(int index,String path) {

		String name = GetNode<ItemList>("VBoxContainer/Official/VBoxContainer/TabContainer/" + path).GetItemText(index);

		GetTree().Paused = true;

		replay_handler.lastPlayedMap = path + "/" + name;
		GetTree().ChangeSceneToFile("res://scenes/maps/" + replay_handler.lastPlayedMap + ".tscn");
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

		for (int file = 0; file < dirs.Length; file++) {
			String[] jsonReplays = DirAccess.GetFilesAt(replay_handler._path + "personal_bests/" + dirs[file] + "/");

			// get best times
			for (int i = 0; i < jsonReplays.Length; i++)
				list.AddItem("personal_bests/" + dirs[file] + "/" + jsonReplays[i].Remove(jsonReplays[i].Length - replay_handler._filenameExtension.Length), texture, true);

		}

		// others
		String[] replays = DirAccess.Open(replay_handler._path).GetFiles();

		for (int i = 0; i < replays.Length; i++)
			if(replays[i].EndsWith(replay_handler._filenameExtension))
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
