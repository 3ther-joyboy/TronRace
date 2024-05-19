using Godot;
using System;
using System.Collections.Generic;

public partial class server : Node
{
	// config data
	public static ConfigFile conf = new ConfigFile();
	public static int user_id;
	public static string user_name;
	public static string custom_ip;

	// server data
	// public static string default_ip = "141.145.207.166";
	public static string default_ip = "127.0.0.1";
	public static int port = 25565;
	public static List<PackedScene> maps;

	// client
	private ENetMultiplayerPeer _peer;
	private int _connection_id;

	public override void _Ready()
	{
		maps = new List<PackedScene>();

		LoadConfig();

		Multiplayer.PeerConnected += PlayerConnected;
		Multiplayer.PeerDisconnected += PlayerDisconnected;
		Multiplayer.ConnectedToServer += ConnectionSuccessful;
		Multiplayer.ConnectionFailed += ConnectionFailed;

		ConnectToServer();
	}

	public void LoadConfig()
	{
		var err = conf.Load("user://config.ini");

		if (err != Error.Ok)
		{
			// create config if it doesn't exist
			Random ran = new Random();
			int id = ran.Next(100000000, 999999999);
			conf.SetValue("Player", "id", id);
			conf.SetValue("Player", "username", id);
			conf.SetValue("Server", "ip", "127.0.0.1");

			conf.Save("user://config.ini");
		}

		// load config data
		user_id = (int) conf.GetValue("Player", "id");
		user_name = (string) conf.GetValue("Player", "username");
		custom_ip = (string) conf.GetValue("Server", "ip");
	}

	private void ConnectToServer()
	{
		_peer = new ENetMultiplayerPeer();
		var status = _peer.CreateClient(default_ip, port);

		if (status != Error.Ok)
		{
			GD.PrintErr("could not create client");
			return;
		}

		_peer.Host.Compress(ENetConnection.CompressionMode.Zlib);
		Multiplayer.MultiplayerPeer = _peer;
	}

	private void PlayerConnected(long id) { GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn"); }

	private void PlayerDisconnected(long id)
	{
		GetTree().ChangeSceneToFile("res://scenes/connecting.tscn");
		ConnectToServer();
	}
	private void ConnectionFailed()
	{
		GD.Print("could not connect to the server");
		ConnectToServer();
	}

	private void ConnectionSuccessful()
	{
<<<<<<< HEAD
		RpcId(1, "handleUserInfo", _peer.GetUniqueId(), user_id, user_name);
	}

	// user rpc function definitions
	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void handleOfficialMap(string map)
	{
		// strore map as temporal scene file
		var file = FileAccess.Open("user://temp.tscn", FileAccess.ModeFlags.Write);
		file.StoreString(map);
		// make scene from temporal scene file
		var scene = ResourceLoader.Load<PackedScene>("user://temp.tscn");

		GetTree().ChangeSceneToPacked(scene);
		// maps.Add(scene);
	}

	// server rpc function declarations
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	extern private void handleUserInfo(int con_id, int id, string name);
=======
		_connection_id = Multiplayer.GetUniqueId();
		GD.Print("id: " + _connection_id);
	}
>>>>>>> f00539abfe10eac495d16bbcc6b048b2a00aea46
}
