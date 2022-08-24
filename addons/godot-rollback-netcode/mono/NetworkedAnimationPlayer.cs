namespace GodotRollbackNetcode
{
    public class NetworkedAnimationPlayer : GDScriptWrapper
    {
        public NetworkedAnimationPlayer() : base() { }
        public NetworkedAnimationPlayer(Godot.Object source) : base(source) { }

        public bool AutoReset
        {
            get => (bool)Source.Get("auto_reset");
            set => Source.Set("auto_reset", value);
        }
    }
}
