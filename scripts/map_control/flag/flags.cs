using Godot;
using System;

public partial class flags : Node
{
	[Export(PropertyHint.Range,"1,10,or_grater")]
	private int loop = 1;
	private int count = 0;
	private int current = 0;

	public void Add(){
		count++;
	}
	public void Count(){
		current++;
		GD.Print( "Loop: " + current + "/" + count*loop );
	}
	public int GetTotal(){
		return count;
	}
	public int GetCount(){
		return current;
	}
	public int GetLoops(){
		return loop;
	}

}
