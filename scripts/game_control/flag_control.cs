using Godot;
using System;

public partial class flag_control : Node
{
	[Export(PropertyHint.Range,"1,10,or_grater")]
	private int _loop = 1;
	private int _currentLoop = 0;
	private int _touchedFlags = 0;

	public void Count(){
		_touchedFlags++;
		GD.Print( (_currentLoop * GetChildCount() + _touchedFlags) + " / " + (_loop * GetChildCount()) );
		if (_touchedFlags >= this.GetChildCount() )
			_Looped();
	}
	private void _Looped(){
		_touchedFlags = 0;
		_currentLoop++;
		var _childrns = GetChildren();
		for (int i = 0; i < this.GetChildCount(); i++)
			((flag)_childrns[i]).Restart();
			
	}

	public bool Complete() {
		return _currentLoop >= _loop;
	}
}
