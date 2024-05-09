using Godot;
using System;

public partial class rotate_to_component : Area2D
{	
	// node "CurrentAim" is just visual note for debbuging
	[ExportCategory("Aiming")]
	[Export]
	bool seeOverLand = false;
	[Export(PropertyHint.Range, "0,10,1,or_greater")]
	int AimingSpeedDegS = 90;
	[Export(PropertyHint.Range, "-3,3,0.1,or_greater,or_lesser")]
	float velocityAim = 0;
	[Export(PropertyHint.Range, "-45,45,5,or_greater,or_lesser")]
	float aimOffSet = 0;

	[Export]
	Vector2 aimingPosition = Vector2.Right;

	[ExportCategory("Target")]
	[Export]
	String name = "Player";

	private RigidBody2D PlayerInSight(Godot.Collections.Array<Godot.Node2D> objs){
		RigidBody2D player = null;
		// player in range
		for (int i = 0; i < objs.Count; i++)
		{
			Node2D obj = (Node2D)(objs[i]);
			if(obj.HasNode(name)){
				// player in sight
				var cast = GetNode<RayCast2D>("Cast");
				cast.GlobalPosition = this.GlobalPosition;
				cast.TargetPosition = obj.GlobalPosition - cast.GlobalPosition;

				var castedPlayer = cast.GetCollider();
				if(castedPlayer != null && castedPlayer.IsClass("RigidBody2D") && ((RigidBody2D)castedPlayer).HasNode(name)){
					player = (RigidBody2D)castedPlayer;
					aimingPosition = player.GlobalPosition;
					break;
				}
			}
		}
		return player;
	}

	private int ShortestRotation(float fromAngle,float toAngle, double deltaTime){
		float delta = (float)deltaTime;	
		float angle = fromAngle - toAngle;
		bool up = (Math.Abs(angle)<Math.PI)?angle > 0:angle < 0;
		if(Math.Abs(angle) > delta*Godot.Mathf.DegToRad(AimingSpeedDegS)){
			return up?-1:1;
		}else{
			return 0;
		}
	}




	public override void _Ready(){
		this.GetNode<CollisionShape2D>("CollisionShape2D").DebugColor = new Color(0.784f,0.416f,0.337f,0.15f);
		aimingPosition += this.GlobalPosition;
	}
	public override void _PhysicsProcess(double delta){
		RigidBody2D target = PlayerInSight(this.GetOverlappingBodies());
		Vector2 targetSpeed = Vector2.Zero;
		if(target != null)
			targetSpeed = target.LinearVelocity;
		var parr = GetParent<Node2D>();
		int direction = ShortestRotation(parr.GlobalRotation,this.GlobalPosition.AngleToPoint(aimingPosition + targetSpeed * velocityAim) + Godot.Mathf.DegToRad(aimOffSet), delta);
		if(direction != 0){
		parr.Rotation += (float)delta * Godot.Mathf.DegToRad(AimingSpeedDegS) * direction;
		}else{
			parr.LookAt(aimingPosition);
		}
	}
}
