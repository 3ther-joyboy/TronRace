using Godot;
using System;

public partial class move_component : RigidBody2D
{	
	[ExportCategory("Physic")]
	[Export]
	private Vector2 startingVelocity = new Vector2(0,0);
	[Export]
	private bool movingState = false;
	[Export]
	private bool lockedToRotation = true;
	[Export]
	private Vector2 direction = Vector2.Up;
	[Export]
	private int acceleration = 10; 
//	[Export(PropertyHint.Range, "0,1,or_greater")]
//	private float friction = 0.05f;

	[ExportCategory("Collisions")]
	[Export(PropertyHint.Range, "0,1,or_greater")]
	// speed threshold to take damage
	private int collisionThreshold = 0;
	// no - 0, add.. - 1, set - 2
	[Export(PropertyHint.Enum, "No,Additional,Set")]
	private int additionalCollision = 0;

	[Export(PropertyHint.Range, "1,10,or_greater")]
	private int pushForce = 100;

	[ExportCategory("Audio")]
	private bool audioPlaing = false;
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3 movingStartSoundEffect;
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3 movingGoingSoundEffect;

	private AudioStreamPlayer2D audioNode = new AudioStreamPlayer2D();


	public override void _Ready(){
		AddChild(audioNode);
		audioNode.Stream = movingStartSoundEffect;
		audioNode.Bus = "SFX";
		// dont ask
		this.ApplyCentralForce(lockedToRotation ? startingVelocity.Length() * Vector2.FromAngle(startingVelocity.Angle() + this.GlobalRotation) : startingVelocity);

	}

	public override void _PhysicsProcess(double delta){
		CollisionDamage(delta);
		if(lockedToRotation)
			direction = Vector2.FromAngle(this.Rotation);
		// accelereation
		if (movingState)
			ApplyCentralForce((float)delta*acceleration*direction);
		Audio();
	}
	private void Audio(){
		if(movingState){

			if(!audioNode.Playing && audioPlaing){
				audioNode.Stream = movingGoingSoundEffect;
			}

			if(!audioNode.Playing){
				audioNode.Play();
				audioPlaing = true;
			}
		}else{
			if(audioNode.Playing){
				audioNode.Stop();
				audioPlaing = false;
				audioNode.Stream = movingStartSoundEffect;
			}
		}
	}
	private void Collision(Node coll){
		if(coll.GetClass() == "RigidBody2D"){
			RigidBody2D collider = (RigidBody2D)coll;
			Vector2 additionalPush = this.GlobalPosition.DirectionTo(collider.GlobalPosition)*pushForce;

			switch (additionalCollision)
			{
				case 1: 
					collider.ApplyCentralForce(additionalPush);
					;break; 
				case 2: 
					collider.LinearVelocity = additionalPush;
					;break; 
				default:;break;
			}
		}
	}
	private void CollisionDamage(double delta){
		KinematicCollision2D coll = this.MoveAndCollide((float)delta*LinearVelocity,true); 

		if(coll != null){
			CollisionDamageCalculation(coll.GetNormal(),coll.GetColliderVelocity());

			if(coll.GetCollider().HasMethod("CollisionDamageCalculation"))
				coll.GetCollider().Call("CollisionDamageCalculation",coll.GetNormal(),this.LinearVelocity);
		}
	}
	public void CollisionDamageCalculation(Vector2 collisionAngle,Vector2 colliderVel){
		// gets velocity in collision angle          |vell1 - vell2| >	max
		if(
				this.HasNode("health") 
				&&
				(this.LinearVelocity.Project(collisionAngle) - colliderVel.Project(collisionAngle)).Length() > collisionThreshold
		  )
			this.GetNode("health").Call("TakeDamage",1);

	}
	public void DirectionSet(float angle) {direction = Vector2.FromAngle(angle);}
	public Vector2 DirectionGet() {return direction;}
	public void DirectionSet(Vector2 angle) {direction = angle;}
	public void MovingStateSet(bool x) {movingState = x;}
	public bool MovingStateGet() {return movingState;}
	public bool GetRotationMode() {return lockedToRotation;}
	public int GetAcceleration() {return acceleration;}
}
