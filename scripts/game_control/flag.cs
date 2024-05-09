using Godot;
using System;

public partial class flag : Area2D
{
	private Node par;
	private int touched = 0;	

	public override void _Ready(){
		par = GetParent();
		par.Call("Add");
	}

	private int isLoopedInt(){
		return (int)par.Call("GetCount") / (int)par.Call("GetTotal");
	}
	private void BodyColl(Node2D collider) {
		bool isPlayer = collider.HasNode("Player");
		bool isLooped = ( touched <= isLoopedInt() + 1 );

		if(isPlayer && isLooped) {
			par.Call("Count");
			touched++;

			if(touched <= isLoopedInt())
				touched++;
		}
		GD.Print("Points: " + (int)(touched % (int)par.Call("GetTotal")) + "/" + (int)par.Call("GetLoops") );
	}
}
