using Godot;
using System;

public partial class gun_component : Area2D
{
	[ExportCategory("Shooting")]
	[Export]
	PackedScene projectiles;
	[Export(PropertyHint.Range, "0,5,0.05,or_greater")]
	float shootInterval = 0.5f;
	[Export(PropertyHint.Range, "1,10,1,or_greater")]
	// how manny projectiles it will shoot in one interval
		int barrage = 1;
	int currentBarrageRound = 0;
	[Export(PropertyHint.Range, "0,5,0.05,or_greater")]
	float barrageInterval = 0.3f;
	[Export(PropertyHint.File, "*.mp3,")]
	private AudioStreamMP3[] _shootingSound;

	[ExportCategory("Aiming")]
	[Export]
	String name = "Player";
	[Export]
	bool seeOverLand = false;

	bool playerIsObservet = false;

	public override void _Ready(){


		//	GetNode<RayCast2D>("Cast").
		GetNode<Timer>("Shoot").WaitTime = shootInterval;
		GetNode<Timer>("Barrage").WaitTime = barrageInterval;
	}
	public override void _PhysicsProcess(double delta){
		RigidBody2D player = PlayerInSight(this.GetOverlappingBodies());
		if(player != null) {

			var timer = GetNode<Timer>("Shoot");
			if(timer.IsStopped() && GetNode<Timer>("Barrage").IsStopped())
				timer.Start();
		}
	}
	private void StartShooting(){
		var timer = GetNode<Timer>("Barrage");
		if(timer.IsStopped())
			timer.Start();
	}
	private void Shoot(){
		currentBarrageRound++;
		if(currentBarrageRound < barrage){
			var timer = GetNode<Timer>("Barrage");
			if(timer.IsStopped())
				timer.Start();

		}else{
			currentBarrageRound = 0;
		}
		move_component spawn = (move_component)projectiles.Instantiate();
		spawn.GlobalPosition = this.GlobalPosition;
		spawn.GlobalRotation = this.GlobalRotation;
		spawn.startingVelocity = spawn.startingVelocity.Rotated(this.GlobalRotation);

		
		GetTree().CurrentScene.AddChild(spawn);

		if (_shootingSound.Length > 0) {
			AudioStreamPlayer2D _audioPlayer = this.GetNode<AudioStreamPlayer2D>("Audio");
			Random rnd = new Random();
			_audioPlayer.Stream = _shootingSound[rnd.Next(0,_shootingSound.Length)];
			_audioPlayer.Play();
		}
	}

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
				if(castedPlayer != null && castedPlayer.IsClass("RigidBody2D") &&((RigidBody2D)castedPlayer).HasNode("Player")){
					player = (RigidBody2D)castedPlayer;
					break;
				}
			}
		}
		return player;
	}
}
