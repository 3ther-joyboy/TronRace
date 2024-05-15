using Godot;
using System;

public partial class lvl_editor : Control
{
	private String _path = "res://scenes/lvl_editor/editor_components/";
	private String[] _objects;

	public override void _Ready(){
		_objects = DirAccess.GetFilesAt(_path);
		ItemList target = GetNode<ItemList>("Bottom/TabContainer/Objects/ItemList");
		target.Clear();

		for (int i = 0; i < _objects.Length; i++) {
			// Gets _texture from lvl_editor_place_holder object in _path
			Godot.Texture2D _texture = ((lvl_editor_place_holder)( ((PackedScene)GD.Load(_path + _objects[i]) ).Instantiate() )).GetTexture();;
			target.AddItem(_objects[i].Remove(_objects[i].Length-17), _texture);
		}
	}

}
