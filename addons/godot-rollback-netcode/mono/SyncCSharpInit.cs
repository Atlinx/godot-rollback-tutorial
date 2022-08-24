using Godot;

namespace GodotRollbackNetcode
{
    public class SyncCSharpInit : Node
    {
        public override void _Ready()
        {
            SyncManager.Init(this);
            SyncReplay.Init(this);
        }
    }
}
