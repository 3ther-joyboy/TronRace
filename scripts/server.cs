using Godot;
using System;

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

		_peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = _peer;
	}

	private void PlayerConnected(long id) { GD.Print("player connected: "); }

	private void PlayerDisconnected(long id) { GD.Print("player disconnected"); }
	private void ConnectionFailed() { GD.Print("could not connect to the server"); }

	private void ConnectionSuccessful()
	{
		_connection_id = Multiplayer.GetUniqueId();
		GD.Print("id: " + _connection_id);
	}
}
