using Godot;
using System;

public partial class server : Node
{
	public static ConfigFile conf = new ConfigFile();
	public static int user_id;
	public static string user_name;
	public static string default_ip = "0.0.0.0";
	public static string custom_ip;
	public static int port = 25565;

	public override void _Ready()
	{
		var err = conf.Load("user://config.ini");

		if (err != Error.Ok)
		{
			// create config if it doesn't exist
			Random ran = new Random();
			int id = ran.Next(100000000, 999999999);
			conf.SetValue("Player", "id", id);
			conf.SetValue("Player", "username", id);
			conf.SetValue("Server", "ip", "");

			conf.Save("user://config.ini");
		}

		// load config
		user_id = (int) conf.GetValue("Player", "id");
		user_name = (string) conf.GetValue("Player", "username");
		custom_ip = (string) conf.GetValue("Server", "ip");
	}
}
