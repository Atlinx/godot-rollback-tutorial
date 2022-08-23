using Godot;

namespace GodotRollbackNetcode
{
    public class CSharpSyncManagerInit : Node
    {
        public override void _Ready()
        {
            SyncManager.Init(this);
        }
    }
}
