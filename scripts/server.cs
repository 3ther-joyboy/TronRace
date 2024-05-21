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
	public static bool status = false;

	// client
	private ENetMultiplayerPeer _peer;
	private int _connection_id;

	public override void _Ready()
	{
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

		_peer.Host.Compress(ENetConnection.CompressionMode.None);
		Multiplayer.MultiplayerPeer = _peer;
	}

	private void PlayerConnected(long id) { status = true; }

	private void PlayerDisconnected(long id)
	{
		status = false;
		GetNode<Label>("../MainMenu/VBoxContainer/Label").Show();
		ConnectToServer();
	}
	private void ConnectionFailed()
	{
		GD.Print("could not connect to the server");
		status = false;
		GetNode<Label>("../MainMenu/VBoxContainer/Label").Show();
		loadResourcePack();
		ConnectToServer();
	}

	private void ConnectionSuccessful()
	{
		RpcId(1, "handleUserInfo", _peer.GetUniqueId(), user_id, user_name);
		GetNode<Label>("../MainMenu/VBoxContainer/Label").Hide();
	}

	// user rpc function definitions
	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void downloadPatch(string patch_base64)
	{
		byte[] patch_bytes = Convert.FromBase64String(patch_base64);
		var patch_zip = FileAccess.Open("user://patch.zip", FileAccess.ModeFlags.Write);
		patch_zip.StoreBuffer(patch_bytes);
		patch_zip.Close();

		loadResourcePack();
	}

	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void loadResourcePack()
	{
		if (DirAccess.DirExistsAbsolute("user://patch.zip")) ProjectSettings.LoadResourcePack("user://patch.zip");
	}

	// server rpc function definitions
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void handleUserInfo(int con_id, int user_id, string user_name)
	{
		GD.Print("\tuser_id\t\t" + user_id);
		GD.Print("\tuser_name\t" + user_name);

		// users.Load("user://users.ini");
		// users.SetValue(user_id.ToString(), "name", user_name);
		// users.Save("user://users.ini");

		byte[] patch_zip = FileAccess.GetFileAsBytes("user://patch.zip");
		if (!patch_zip.IsEmpty())
		{
			string patch_base64 = Convert.ToBase64String(patch_zip);
			RpcId(con_id, "downloadPatch", patch_base64);
		}
	}
}
