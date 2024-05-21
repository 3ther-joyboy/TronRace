using Godot;
using System;

public partial class button_sounds : Node
{
	private Godot.Collections.Array<Node> _node_list = new Godot.Collections.Array<Node>();

    public override void _Ready()
    {
		var parent = GetParent();
		_add_all_children(parent);

		foreach (var node in _node_list)
		{
			if (node.GetType() == typeof(Button)) ((Button) node).Pressed += PlaySound;
		}
    }

	private void _add_all_children(Node node)
	{
		foreach (Node n in node.GetChildren())
		{
			_node_list.Add(n);
			_add_all_children(n);
		}	
	}

	private void PlaySound()
	{
		audio_player.sounds.Play();
	}
}
