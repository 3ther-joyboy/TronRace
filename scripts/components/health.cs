using Godot;
using System;
public partial class health : Node
{
	[ExportCategory("Health")]
	[Export(PropertyHint.Range, "1,10,1,or_greater")]
	// max health points, if reaches zero parrent will be killed >:D
		private int hp { get; set; }= 1;
	private int maxHealth;

	//	[Export(PropertyHint.Range, "0,1,0.1,or_Greater")]
	//	// every (1/x) second will trye to heal
	//	private float regenerationPerSecond { get; set; } = 0;

	[Export(PropertyHint.Range, "0,3,0.1")]	
		// on hit there will be small window where a nother hits will not count 
		private float invincibilitySeconds { get; set; } = 0f;
	private bool invincibilityState { get; set; } = false;

	[ExportCategory("Death")]
	[Export]
	// on death will spawn a copy of a visual node of parent noede
		private bool ragdollOnDeath { get; set; } = false;
	[Export]
	// ragdol will delete it self after x amount of seconds
		private float ragdollSeconds { get; set; } = 3f;
	// this will spawn any packedscene after death
	[Export]
	private PackedScene onDeathSpawn { get; set; } = null;

	public override void _Ready(){
		maxHealth = hp;
	}

	public void TakeDamage(int damage){
		if(!invincibilityState){
			hp -= damage;
			Invincibility(invincibilitySeconds);
			if(hp<= 0) Die();
		}
		if(GetParent().HasNode("visual"))GetParent().GetNode<Node2D>("visual").Call("Blink");
	}

	// Invincibility
	public void Invincibility(float seconds){
		if(seconds > 0){
			invincibilityState = true;
			var timer = GetNode<Timer>("invincibility");
			timer.WaitTime = seconds;
			timer.Start();
		}
	}
	private void InvincibilityEnd(){
		invincibilityState = false;
	}

	// Dying
	// only function that should be able to kill nodes [dont create other]
	private void Die(){
		// spawns visual part of parrent node 
		if(ragdollOnDeath){

		}
		// spawns a packedscene on death 
		// no recursions please
		if(onDeathSpawn != null){
			Node2D spawn = (Node2D)onDeathSpawn.Instantiate();
			spawn.GlobalPosition = GetParent<Node2D>().GlobalPosition;
			spawn.GlobalRotation = GetParent<Node2D>().GlobalRotation;
			GetParent().GetParent().AddChild(spawn);
		}
		GetNode<Timer>("die").Start();
	}
	// uhh dont ask why
	private void QF(){
		GetParent().QueueFree();
	}
}
