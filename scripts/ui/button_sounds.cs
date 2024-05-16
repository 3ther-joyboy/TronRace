using Godot;
using System;

public partial class button_sounds : Node
{
    public override void _Ready()
    {
		var parent = GetParent();
		var nodes = parent.GetChildren();

		foreach (var node in nodes)
		{
			if (node.GetType() == typeof(Button))
			{
				((Button) node).Pressed += PlaySound;
			}
		}
    }

	private void PlaySound()
	{
		
	}
}
