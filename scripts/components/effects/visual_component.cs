using Godot;
using System;

public partial class visual_component : Node2D
{
	[Export(PropertyHint.Range, "0,3,0.1")]	
		private float blinkSeconds { get; set; } = 0.2f;
	[Export(PropertyHint.ColorNoAlpha)]
	// while white it will not do change color
		private Color blinkColor { get; set; } = new Color(1,0,0);

	// Blink
	public void Blink(){
		if(this.HasNode("variable"))
		Blink(blinkSeconds,blinkColor);
	}
	public void Blink(float seconds, Color color){
		if(blinkSeconds > 0){
			if(color != new Color(1,1,1)){
				if(HasNode("variable")){	
					GetNode<Node2D>("variable").Modulate = color;
					var timer = GetNode<Timer>("blink");
					timer.WaitTime = seconds;
					timer.Start();
				}
			}
		}
	}

	private void BlinkEnd(){
		GetNode<Node2D>("variable").Modulate = new Color(1,1,1);
	}
	public void RotateDir(Vector2 dir){
		this.Rotation = dir.Angle();
	}
	public void RotationAngle(float angle){
		this.Rotation = angle;
	}
}
