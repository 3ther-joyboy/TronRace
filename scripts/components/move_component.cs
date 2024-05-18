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
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3 _crashSoundEffect;

	private AudioStreamPlayer2D _engineAudioNode = new AudioStreamPlayer2D();
	private AudioStreamPlayer2D _crashAudioNode = new AudioStreamPlayer2D();


	public override void _Ready(){
		AddChild(_crashAudioNode);
		_crashAudioNode.Stream = _crashSoundEffect;
		_crashAudioNode.Bus = "SFX";

		AddChild(_engineAudioNode);
		_engineAudioNode.Stream = movingStartSoundEffect;
		_engineAudioNode.Bus = "SFX";
		// dont ask
		this.ApplyCentralForce(startingVelocity);

	}

	public override void _PhysicsProcess(double delta){
		CollisionDamage(delta);

		// accelereation
		if (movingState)
			ApplyCentralForce((float)delta * acceleration * Vector2.FromAngle(this.Rotation));
		Audio();
	}
	private void Audio(){
		if(movingState){

			if(!_engineAudioNode.Playing && audioPlaing){
				_engineAudioNode.Stream = movingGoingSoundEffect;
			}

			if(!_engineAudioNode.Playing){
				_engineAudioNode.Play();
				audioPlaing = true;
			}
		}else{
			if(_engineAudioNode.Playing){
				_engineAudioNode.Stop();
				audioPlaing = false;
				_engineAudioNode.Stream = movingStartSoundEffect;
			}
		}
	}
	private void Collision(Node coll){
		_crashAudioNode.Play();
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
	public void MovingStateSet(bool x) {movingState = x;}
	public bool MovingStateGet() {return movingState;}
	public int GetAcceleration() {return acceleration;}
}
