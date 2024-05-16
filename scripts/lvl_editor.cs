using Godot;
using System;

public partial class lvl_editor : Control
{
	private String _path = "res://scenes/lvl_editor/editor_components/";
	private String _editedLvl = "user://custom_map.tscn";
	private PackedScene _editorScene;

	private String[] _objects;
	private int _mode;
	private String _selected;

	enum Mode 
	{
		ObjectSpawning = 1,
	}

	public override void _Ready(){

		var lvl = GetNode<Node2D>("lvl");
		if (FileAccess.FileExists(_editedLvl))
			_editorScene = ResourceLoader.Load<PackedScene>(_editedLvl);
			
		else {
			PackedScene scene = new PackedScene();
			scene.Pack(lvl);
		}	




		_mode = (int)Mode.ObjectSpawning;

		_objects = DirAccess.GetFilesAt(_path);
		ItemList target = GetNode<ItemList>("Margin/Bottom/TabContainer/Objects/ItemList");
		target.Clear();

		for (int i = 0; i < _objects.Length; i++) {
			// Gets _texture from lvl_editor_place_holder object in _path
			Godot.Texture2D _texture = ((lvl_editor_place_holder)( ((PackedScene)GD.Load(_path + _objects[i]) ).Instantiate() )).GetTexture();;
			target.AddItem(_objects[i].Remove(_objects[i].Length-17), _texture);
		}
	}

	private void _SelectObject(int index,Vector2 clickPosition,int mouseButton) {
		_selected = _objects[index];
	}
	private void _WorldInput(InputEvent input) {
		if(Input.IsActionJustPressed("tap") && _selected != null) {
			// gets instance of lvl placeholder
			var spawn = (lvl_editor_place_holder )(((PackedScene)GD.Load(_path + _selected)).Instantiate());; 
			spawn.GlobalPosition = GetViewport().GetMousePosition();
			GetNode<Node2D>("lvl").AddChild(spawn);

		}

	}
	private void _GoBack() {

//		var lvl = GetNode<Node2D>("lvl");
//		var scene = new PackedScene();
//		Error result = scene.Pack(lvl);
//		if (result == Godot.Error.Ok)
//		{
//			Error error = ResourceSaver.Save(scene, _editedLvl);
//			if (error != Godot.Error.Ok)
//			{
//				GD.PushError("An error occurred while saving the scene to disk.");
//			}
//		}

		GetTree().ChangeSceneToFile((String)ProjectSettings.GetSetting("application/run/main_scene"));
	}
	private void _StartLvl() {
		GetTree().CallGroup("editor_placeholder", "Save");
		GetNode<Node2D>("lvl").Reparent(GetTree().Root.GetChild(0));
		this.QueueFree();
	}

}
