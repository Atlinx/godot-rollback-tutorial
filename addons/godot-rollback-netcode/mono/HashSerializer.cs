namespace GodotRollbackNetcode
{
    public abstract class HashSerializer : Godot.Object
    {
        private Godot.Collections.Dictionary serialize(Godot.Collections.Dictionary value) => Serialize(value);

        public abstract Godot.Collections.Dictionary Serialize(Godot.Collections.Dictionary value);

        private Godot.Collections.Dictionary unserialize(Godot.Collections.Dictionary value) => Unserialize(value);

        public abstract Godot.Collections.Dictionary Unserialize(Godot.Collections.Dictionary value);
    }
}
