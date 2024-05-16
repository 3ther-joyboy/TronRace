using Godot;
using System;

public partial class lvl_editor_place_holder : Node2D
{
	[Export(PropertyHint.File, "*.tscn,")]
	private PackedScene _packed;
	[Export]
	private Godot.Texture2D _texture;


	public override void _Ready(){
		AddToGroup("editor_placeholder");
	}
	public Godot.Texture2D GetTexture() {
		return _texture;
	}
	public void Save() {
		if (HasNode("visual"))
			GetNode<visual_component>("visual").QueueFree();

		Godot.Collections.Array<Godot.Node> _childerns = this.GetChildren();

		Node2D _spawn = (Node2D)_packed.Instantiate();
		_spawn.Rotation = this.Rotation;
		_spawn.Position = this.Position;

		GetParent().AddChild(_spawn);

		this.QueueFree();
	}
}
