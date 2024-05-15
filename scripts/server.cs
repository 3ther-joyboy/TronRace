using Godot;
using System;

public partial class server : Node
{
	public static ConfigFile conf = new ConfigFile();
	public static int user_id;
	public static string user_name;

	public override void _Ready()
	{
		var err = conf.Load("user://config.ini");

		if (err != Error.Ok)
		{
			Random ran = new Random();
			int id = ran.Next(100000000, 999999999);
			conf.SetValue("Player", "id", id);
			conf.SetValue("Player", "username", id);

			conf.Save("user://config.ini");
		}

		user_id = (int) conf.GetValue("Player", "id");
		user_name = (string) conf.GetValue("Player", "username");
	}
}