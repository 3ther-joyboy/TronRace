using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class RecordFormat
{
	public bool active {get; set;}

	public float rotation {get; set;}
	public float rotationVel {get; set;}

	public float positionX {get; set;}
	public float positionY {get; set;}

	public float velocityX {get; set;}
	public float velocityY {get; set;}

	public RecordFormat(JsonNode replay) {
		this.active			= replay["active"].GetValue<bool>();

		this.rotation		= replay["rotation"].GetValue<float>(); 
		this.rotationVel	= replay["rotationVel"].GetValue<float>(); 

		this.positionX		= replay["positionX"].GetValue<float>();
		this.positionY		= replay["positionY"].GetValue<float>();

		this.velocityX		= replay["velocityX"].GetValue<float>();
		this.velocityY		= replay["velocityY"].GetValue<float>();
	}
	public RecordFormat(bool active, float rotation, float rotationVel,float positionX, float positionY, float velocityX, float velocityY) {
		this.active			= active;

		this.rotation		= rotation;
		this.rotationVel	= rotationVel;

		this.positionX		= positionX;
		this.positionY		= positionY;

		this.velocityX		= velocityX;
		this.velocityY		= velocityY;
	}
}
