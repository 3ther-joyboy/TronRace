using Godot;
using System;

public partial class button_sounds : Node
{
    public override void _EnterTree()
    {
		var nodes = GetChildren();

		foreach (var node in nodes)
		{
			if (node.GetType() == typeof(Button))
			{
				
			}
		}
    }
}
